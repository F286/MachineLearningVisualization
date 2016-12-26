using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CONTENT_SliderToNode : MonoBehaviour {

    public string title;
    public float min = -5;
    public float max = 5;
    public Node target;
    public Slider slider;
    public Text text;

	// Use this for initialization
	void Start () {
        text.text = title;
        slider.minValue = min;
        slider.maxValue = max;
        slider.value = (float)target.value;
	}
	
	// Update is called once per frame
	void Update () {
        target.value = slider.value;
	}
}
