using UnityEngine;
using System.Collections;
using UnityEditor;

public class CONTENT_ManagerGraph : MonoBehaviour 
{
    const float NumericalStepSize = 0.00001f;

    public Vector4 input;
    public Vector4 weights;

    public float value;
    public float analyticalGradient;
    public float numericalGradient;

    public static Vector4 TestAxis = new Vector4(1, 0, 0, 0);

    public void Update()
    {
        value = Forward(input, weights);
        analyticalGradient = Backward(input, weights).MaskSum(TestAxis);
        numericalGradient = BackwardNumerical(input, weights);

        Debug.DrawLine(new Vector3(-5, 0, 0), new Vector3(5, 0, 0), Color.black);
        Debug.DrawLine(new Vector3(0, -5, 0), new Vector3(0, 5, 0), Color.black);

        var g = Color.green;
        g.a = 0.8f;
        var r = Color.red;
        r.a = 0.8f;

//        print(input.InverseMask(TestAxis));
        for (float i = -5; i < 5; i += 0.1f)
        {
            var a = input.InverseMask(TestAxis) + TestAxis * i;
            var b = input.InverseMask(TestAxis) + TestAxis * (i + 0.1f);

            Debug.DrawLine(new Vector3(i, Forward(a, weights)), new Vector3(i + 0.1f, Forward(b, weights)), Color.black);
            Debug.DrawLine(new Vector3(i, BackwardNumerical(a, weights)), new Vector3(i + 0.1f, BackwardNumerical(b, weights)), g);
            Debug.DrawLine(new Vector3(i, Backward(a, weights).MaskSum(TestAxis)), new Vector3(i + 0.1f, Backward(b, weights).MaskSum(TestAxis)), r);
        }
    }

    public static float Forward(Vector4 input, Vector4 weights)
    {
        float value = 0;
        // apply weights and add inputs
        value = input.x * weights.x + input.y * weights.y + input.z * weights.z + input.w;//weights.w;

        // easing curve between -1 and 1
        value = (float)System.Math.Tanh(value);

        return value;
    }
    public static Vector4 Backward(Vector4 input, Vector4 weights)
    {
        Vector4 gradient = Vector4.one;

        // apply weights and add inputs
        gradient.x *= weights.x;
        gradient.y *= weights.y;
        gradient.z *= weights.z;
        gradient.w *= 1;

        // calculate what value was right before function (can cache this)
        var value = input.x * weights.x + input.y * weights.y + input.z * weights.z + input.w;//weights.w;

        // easing curve between -1 and 1
        gradient *= 1 - ((float)System.Math.Tanh(value)).Squared();

        return gradient;
    }
    public static float BackwardNumerical(Vector4 input, Vector4 weights)
    {
        var a = Forward(input, weights);
        var b = Forward(input + NumericalStepSize * TestAxis, weights);
        return (b - a) / NumericalStepSize;
    }

    public void OnGUI()
    {
        if (GUILayout.Button("X"))
        {
            TestAxis = new Vector4(1, 0, 0, 0);
        }
        if (GUILayout.Button("Y"))
        {
            TestAxis = new Vector4(0, 1, 0, 0);
        }
        if (GUILayout.Button("Z"))
        {
            TestAxis = new Vector4(0, 0, 1, 0);
        }
        if (GUILayout.Button("W"))
        {
            TestAxis = new Vector4(0, 0, 0, 1);
        }
    }
}
