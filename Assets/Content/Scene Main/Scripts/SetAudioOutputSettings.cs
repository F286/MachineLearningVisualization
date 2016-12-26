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