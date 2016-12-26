using UnityEngine;
using System.Collections;

public class CONTENT_NodeSubtract : Node
{
    public double _value;
    public double _derivative;
    public override double value { get { return _value; } set { _value = value; } }
    public override double derivative { get { return _derivative; } set { _derivative = value; } }

    public override void forward(params Node[] input)
    {
        value = input[0].value - input[1].value;
    }
    public override void backward(params Node[] input)
    {
//        for (int i = 0; i < input.Length; i++)
//        {
//            input[i].derivative += derivative;
//        }
        input[0].derivative += derivative;
        input[1].derivative -= derivative;
    }
    public override void train(float step)
    {
        
    }
}