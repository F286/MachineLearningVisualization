using UnityEngine;
using System.Collections;

public class MultiplyGate : Gate
{
    public override void Forward(params Gate[] v)
    {
        base.Forward(v);
        value = 1f;
        for (int i = 0; i < v.Length; i++)
        {
            value *= v[i].value;
        }
//        value = v[0].value * v[1].value;
    }
    public override void Backward()
    {
        for (int i = 0; i < input.Length; i++)
        {
            var g = gradient;
            for (int j = 0; j < input.Length; j++) 
            {
                if (i != j)
                {
                    g *= input[j].value;
                }
            }
            input[i].gradient += g;
        }
//        input[0].gradient += input[1].value * gradient;
//        input[1].gradient += input[0].value * gradient;
    }
    #if UNITY_EDITOR
    public override string Display()
    {
        return "[*] " + base.Display();
    }
    #endif
}