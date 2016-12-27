using UnityEngine;
using System.Collections;

public class SigmoidGate : Gate
{
    public override void Forward(params Gate[] v)
    {
        base.Forward(v);
        value = 1 / (1 + System.Math.Exp(-v[0].value));
    }
    public override void Backward()
    {
        var s = 1 / (1 + System.Math.Exp(-input[0].value));
        input[0].gradient += (s * (1 - s)) * gradient;
    }
    #if UNITY_EDITOR
    public override string Display()
    {
        return "[s] " + base.Display();
    }
    #endif
}