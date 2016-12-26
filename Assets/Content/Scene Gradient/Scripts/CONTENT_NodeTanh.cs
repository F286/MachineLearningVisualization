using UnityEngine;
using System.Collections;

public class CONTENT_NodeTanh : Node
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
        value = System.Math.Tanh(value);
    }
    public override void backward(params Node[] input)
    {
        var fD = 0.0;
        for (int i = 0; i < input.Length; i++)
        {
            fD += input[i].value;
        }
        fD = 1 - (System.Math.Tanh(fD)).Squared();
        for (int i = 0; i < input.Length; i++)
        {
            input[i].derivative += fD;
        }

//        for (int i = 0; i < input.Length; i++)
//        {
//            var fD = 1 - (System.Math.Tanh(input[i].value)).Squared();
//            input[i].derivative += fD * derivative;
//        }

//        var s = 1 / (1 + System.Math.Exp(-input[0].value));
//        input[0].derivative += s * (1 - s) * derivative;
    }
    public override void train(float step)
    {
        
    }
}