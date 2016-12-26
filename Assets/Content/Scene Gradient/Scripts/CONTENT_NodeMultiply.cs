using UnityEngine;
using System.Collections;

public class CONTENT_NodeMultiply : Node
{
    public double _value;
    public double _derivative;
    public override double value { get { return _value; } set { _value = value; } }
    public override double derivative { get { return _derivative; } set { _derivative = value; } }

    public override void forward(params Node[] input)
    {
        value = 1;
        for (int i = 0; i < input.Length; i++)
        {
            value *= input[i].value;
        }
//        value = input[0].value * input[1].value;
    }
    public override void backward(params Node[] input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            var addToInput = derivative;
            for (int j = 0; j < input.Length; j++) 
            {
                if (i != j)
                {
                    addToInput *= input[j].value;
                }
            }
            input[i].derivative += addToInput;
        }
//        input[0].derivative += input[1].value * derivative;
//        input[1].derivative += input[0].value * derivative;
    }
    public override void train(float step)
    {
        
    }
}