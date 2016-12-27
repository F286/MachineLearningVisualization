using UnityEngine;
using System.Collections;

// Put this code in a empty Scene:
/*
using UnityEngine;
using System.Collections;
 
public class SetAudioOutputSettings : MonoBehaviour {
    public int sampleRate = 44100;
    public string level = "NameOfYourActualScene";
 
    void Start () {
        if(AudioSettings.outputSampleRate != sampleRate)
            AudioSettings.outputSampleRate = sampleRate;
        Application.LoadLevel(level);
    }
}
*/

// Apply this script on a Plane
using System;
using System.Linq;


[RequireComponent (typeof(AudioSource))]
public class CONTENT_MainManager : MonoBehaviour 
{
    [System.Serializable]
    public class NeuronData
    {
        public float Bias;
        public float[] Weight;
    }
    const int Vertical = 128;
//    const int Vertical = 1024;
    const int Horizontal = 8;
    const int NumberOfKeywords = 3;
    public enum KeyWord
    {
        One,
        Two,
        Seven,
//        Cat,
//        Sad,
//        Dog,
//        Age,
//        His,
    }
    public enum State
    {
        None,
        Learn,
        Recognize,
    }

    public float FittingLerpRate = 0.05f;
    public float FittingBiasRate = 0.01f;

    public int fftSize = 1024;
    float[] spectrum;
    Texture2D texture;
    int x = 0;

//    public Gradient Display;

    float[] intensityAverage = new float[4];
    int intensityIndex;

    bool isRecording;

    public float On = 0.2f;
    public float Off = 0.1f;

    public KeyWord keyword;
    public State state;

    [Header("direct mapping of input hyper point (1024x32 dimensions) to output two dimensional space (keyword)")]
    public NeuronData[] FirstLayer;
    public NeuronData[] SecondLayer;
    public float[] RawData;
    [Header("softmax outpt")]
    public float[] Output;
    [Header("what softmax output SHOULD be")]
    public float[] Target;

    public string Display;

    public void RunNeuralNetwork()
    {
        var total = 0f;

        int index = 0;
        foreach (var item in FirstLayer)
        {
            var neuron = 0f;
            for (int i = 0; i < item.Weight.Length; i++)
            {
                var add = item.Weight[i] * RawData[i];
                neuron += add * add;
            }
            neuron += item.Bias;

            Output[index] = neuron;
            total += neuron;
//            print((KeyWord)index + ": " + neuron * 100f);
            index++;
        }
        for (int i = 0; i < Output.Length; i++)
        {
            Output[i] /= total;
            print((KeyWord)i + ": " + Output[i]);
        }
        for (int i = 0; i < Target.Length; i++)
        {
            Target[i] = (int)keyword == i ? 1 : 0;
        }
        if (state == State.Learn)
        {
            var neuron = FirstLayer[(int)keyword];
            for (int i = 0; i < neuron.Weight.Length; i++)
            {
                neuron.Weight[i] = Mathf.Lerp(neuron.Weight[i], RawData[i], FittingLerpRate);
                neuron.Bias += neuron.Weight[i] < RawData[i] ? FittingBiasRate : -FittingBiasRate;
                neuron.Bias = Mathf.Clamp01(neuron.Bias);
//                if(RawData[i] < neuron.
            }
//            neuron.Weight
        }

        int maxIndex = 0;
        float maxValue = 0f;
        for (int i = 0; i < Output.Length; i++)
        {
            if (Output[i] > maxValue)
            {
                maxIndex = i;
                maxValue = Output[i];
            }
        }

        Display = (KeyWord)maxIndex + " : " + Mathf.RoundToInt(maxValue * 100) + "%";
    }
    void SetupNeuralNetworkData()
    {
        FirstLayer = new NeuronData[NumberOfKeywords];
        for (int i = 0; i < FirstLayer.Length; i++)
        {
            FirstLayer[i] = new NeuronData();
        }
        foreach (var item in FirstLayer)
        {
            item.Weight = new float[Vertical * Horizontal];
            for (int j = 0; j < Vertical * Horizontal; j++) 
            {
                item.Weight[j] = 0.4f + UnityEngine.Random.value * 0.2f;
            }
            item.Bias = 0.0f;
        }
        RawData = new float[Vertical * Horizontal];
        Output = new float[NumberOfKeywords];
        Target = new float[NumberOfKeywords];
    }

    void Start() 
    {
        SetupNeuralNetworkData();

        // FFT
        spectrum = new float[fftSize];

        // Displaying the output on a texture
//        texture = new Texture2D(fftSize / 4, 32);
//        texture = new Texture2D(fftSize / 4, fftSize);
        texture = new Texture2D(fftSize / 4, Vertical);
        GetComponent<Renderer>().material.shader = Shader.Find("Unlit/Texture");
        GetComponent<Renderer>().material.mainTexture = texture;

        // Set the Texture to black
        for(x=0; x<texture.width; x++)
        {
            for(int y=0; y<texture.height; y++)
            {
                Color color = new Color(0, 0, 0);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        x = 0;

        // Init the Mic
        #if UNITY_EDITOR
        GetComponent<AudioSource>().clip = Microphone.Start(null, true, 10, 44100);
        #endif
        GetComponent<AudioSource>().loop = true; // Set the AudioClip to loop
//        GetComponent<AudioSource>().volume = 0.001f;
        //        GetComponent<AudioSource>().mute = true; // Mute to avoid feedback
        #if UNITY_EDITOR
        while (!(Microphone.GetPosition("") > 0)){} // Wait until the recording has started
        #endif
        GetComponent<AudioSource>().Play(); // Play the audio source!
    }

    int frame = 0;
    int frameStart = 0;
    int writeToRaw = 0;
    void Update() 
    {
        // Read the FFT
        spectrum = GetComponent<AudioSource>().GetSpectrumData(1024, 0, FFTWindow.BlackmanHarris);

        var average = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            average += spectrum[i];
        }
        average /= spectrum.Length;
        intensityAverage[intensityIndex++ % intensityAverage.Length] = average;

        var a = 0f;
        for (int i = 0; i < intensityAverage.Length; i++)
        {
            var hertz = (1f / 1024f) * (44100f / 2f);
            a = Mathf.Max(a, intensityAverage[i] * Mathf.Log(hertz, 2));
        }
        a /= intensityAverage.Length;
        a *= 100000f;

        if (isRecording)
        {
            if (a < Off)
            {
                isRecording = false;
                if(frame - frameStart > 8)
                {
                    print("end, total frames: " + (frame - frameStart) );
                    RunNeuralNetwork();
                }
            }
        }
        else
        {
            if (a > On)
            {
                isRecording = true;
                frameStart = frame;
//                print("start");
                for (int i = 0; i < RawData.Length; i++)
                {
                    RawData[i] = 0;
                }
                writeToRaw = 0;
            }
        }

        frame++;
        if (isRecording)
        {
            x++;
            for (int y = 0; y < texture.height; y++)
            {
                float added = 0;
                for (int i = 0; i < (1024 / Vertical); i++)
                {
                    added += spectrum[y * (1024 / Vertical) + i];
                }
                added /= (1024 / Vertical);
                float db = added * 100;
//                float db = spectrum[y] * 100;
                var r = db * 10;
                var g = Mathf.Max(0, db * 10 - 1);
                var b = Mathf.Max(0, db * 10 - 2);
                Color color = new Color(r, g, b);
                texture.SetPixel(x, y, color);

                if (writeToRaw < RawData.Length)
                {
                    RawData[writeToRaw] = added * 1000;
                }
                writeToRaw++;
//                if (isRecording && frame < Horizontal)
//                {
//                    for (int i = 0; i < Vertical; i++)
//                    {
//                        RawData[frame * Vertical + i] = spectrum[i] * 100;
//                    }
//                }
            }
        }
//        if (isRecording)
//        {
//            x++;
//            for (int y = 0; y < texture.height; y++)
//            {
//                float db = spectrum[y] * 100;
//                var r = db * 10;
//                var g = Mathf.Max(0, db * 10 - 1);
//                var b = Mathf.Max(0, db * 10 - 2);
//                Color color = new Color(r, g, b);
//                texture.SetPixel(x, y, color);
//            }
//        }
        texture.Apply();
    }

    public void OnGUI()
    {
        GUILayout.Space(40);

        GUIStyle s = new GUIStyle(GUI.skin.label);
        s.fontSize = 100;
//        myStyle.font = myFont;
//        GUI.Label(new Rect(10,10, 100, 30), "Hello World!", myStyle);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("None"))
        {
            state = State.None;
        }
        if (GUILayout.Button("Learn"))
        {
            state = State.Learn;
        }
        if (GUILayout.Button("Recognize"))
        {
            state = State.Recognize;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        foreach (var item in Enum.GetValues(typeof(KeyWord)).Cast<KeyWord>())
        {
            if (GUILayout.Button(item.ToString()))
            {
                keyword = item;
            }
        }
//        for (int i = 0; i < keyword.; i++)
//        {
//            f
//        }
//        if (GUILayout.Button("One"))
//        {
//            keyword = KeyWord.One;
//        }
//        if (GUILayout.Button("Two"))
//        {
//            keyword = KeyWord.Two;
//        }
//        if (GUILayout.Button("Three"))
//        {
//            keyword = KeyWord.Three;
//        }
//        if (GUILayout.Button("Four"))
//        {
//            keyword = KeyWord.Four;
//        }
//        if (GUILayout.Button("Five"))
//        {
//            keyword = KeyWord.Five;
//        }
        GUILayout.EndHorizontal();

        GUILayout.Label(Display, s);
    }

}

























