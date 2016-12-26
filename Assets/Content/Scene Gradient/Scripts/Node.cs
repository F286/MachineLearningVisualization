using UnityEngine;
using System.Collections;

public abstract class Node : MonoBehaviour
{
    public abstract double value { get; set; }
    public abstract double derivative { get; set; }

//    INode[] input { get; }

    public abstract void forward(params Node[] input);
    public abstract void backward(params Node[] input);
    public abstract void train(float step);

    public CONTENT_Display display;
    public Node[] DebugInputs;

    public bool CreateDisplay;

    public void Reset()
    {
        CreateDisplay = true;
    }

    public void Awake()
    {
        CONTENT_ManagerNeuron.instance.nodes.Add(this);
        if (CreateDisplay)
        {
            display = gameObject.AddComponent<CONTENT_Display>();
        }
        var g = new GameObject("Text");
        var t = g.AddComponent<TextMesh>();
        t.text = name;
        t.transform.position = transform.position + new Vector3(0, -0.35f, 0);
        t.characterSize = 0.05f;
//        if (Application.loadedLevelName == "Scene Lstm")
//        {
//            t.fontSize = 25;
//        }
//        else
//        {
            t.fontSize = 26;
//        }
        t.alignment = TextAlignment.Center;
        t.anchor = TextAnchor.MiddleCenter;
        t.color = Color.black;
    }
    public void LateUpdate()
    {
        if(display)
        {
            display.value = (float)value;
            display.derivative = (float)derivative;
        }
    }
}