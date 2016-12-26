using UnityEngine;
using System.Collections;

public class CONTENT_NodeSigmoid : Node
{
    public double _value;
    public double _derivative;
    public override double value { get { return _value; } set { _value = value; } }
    public override double derivative { get { return _derivative; } set { _derivative = value; } }

    public override void forward(params Node[] input)
    {
        value = 0.0;
        for (int i = 0; i < input.Length; i++)
        {
            value += input[i].value;
        }
       value = 1 / (1 + System.Math.Exp(-value));
    }
    public override void backward(params Node[] input)
    {
        var s = 0.0;
        for (int i = 0; i < input.Length; i++)
        {
            s += input[i].value;
        }
        s = 1 / (1 + System.Math.Exp(-s));
        s = s * (1 - s) * derivative;
        for (int i = 0; i < input.Length; i++)
        {
            input[i].derivative += s;
        }
    }
    public override void train(float step)
    {
        
    }
}