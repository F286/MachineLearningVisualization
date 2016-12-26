using UnityEngine;
using System.Collections;

public class CONTENT_NodeValue : Node
{
    public double _value;
    public double _derivative;
    public override double value { get { return _value; } set { _value = value; } }
    public override double derivative { get { return _derivative; } set { _derivative = value; } }

    public bool canTrain = true;

    public override void forward(params Node[] input)
    {

    }
    public override void backward(params Node[] input)
    {

    }
    public override void train(float step)
    {
        if (canTrain)
        {
            _value += derivative * step;
//            _value += derivative * System.Math.Abs(derivative) * step;
        }
    }
}