using UnityEngine;
using System.Collections;

public class AddGate : Gate
{
    public override void Forward(params Gate[] v)
    {
        base.Forward(v);
        value = 0f;
        for (int i = 0; i < v.Length; i++)
        {
            value += v[i].value;
        }
//        value = v[0].value + v[1].value;
    }
    public override void Backward()
    {
        for (int i = 0; i < input.Length; i++)
        {
            input[i].gradient = 1 * gradient;
        }
//        input[0].gradient += 1 * gradient;
//        input[1].gradient += 1 * gradient;
    }
    public override string Display()
    {
        return "[+] " + base.Display();
    }
}