using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public class NeuronGate : Gate
{
    ValueGate bias;
    ValueGate[] weights = new ValueGate[0];

    public void Start()
    {
        bias = gameObject.AddComponent<ValueGate>();
        bias.display = false;
//        bias.value = Random.Range(-0.1f, 0.1f);
        bias.value = UnityEngine.Random.Range(-2f, 2f);

        weights = new ValueGate[input.Length];
        for (int i = 0; i < input.Length; i++)
        {
            weights[i] = gameObject.AddComponent<ValueGate>();
//            weights[i].value = Random.Range(-0.1f, 0.1f);
            weights[i].value = UnityEngine.Random.Range(-0.5f, 0.5f);
            weights[i].display = false;
        }
//        parameter = new float[input.Length + 1];
//        parameterGradient = new float[input.Length + 1];
//
//        for (int i = 0; i < parameter.Length; i++)
//        {
//            parameter[i] = Random.Range(-0.5f, 0.5f);
//        }
    }
    double Calculate()
    {
        if (bias == null)
        {
            return 0;
        }
        var value = bias.value;
//        return bias.value + Mathf.Exp(
//        var v = bias.value;
        for (int i = 0; i < weights.Length; i++)
        {
            value += input[i].value * weights[i].value;
        }
        value = System.Math.Tanh(value);
//        value = 1 / (1 + Math.Exp(-value));
//        v = Mathf.PingPong(v, 0.1f);
//        v = 1 / (1 + Mathf.Pow(-v, 2));
//        v = 1 / (1 + Mathf.Exp(-v));
//        v = (v - 0.5f) * 2f;
        return value;

//        var g = (2f * Mathf.Exp(v)) / (Mathf.Exp(v) + 1).Squared());
//        a = -1f / (a * a);

    }
    public override void Forward(params Gate[] v)
    {
        base.Forward(v);

        value = Calculate();

//        value = Calculate;
//        value = 1 / (1 + Mathf.Exp(-v[0].value));
    }
    float log;
    public override void Backward()
    {
        if (bias == null)
        {
            return;
        }
        // derivative for connections and bias
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i].gradient = weights[i].value * gradient;
        }
        bias.gradient = 1 * gradient;

        // derivative for tanh function
        var value = bias.value;
        for (int i = 0; i < weights.Length; i++)
        {
            value += input[i].value * weights[i].value;
        }
        var tanhD = 1 - (System.Math.Tanh(value)).Squared();
//        var s = 1 / (1 + Math.Exp(-value));
//        var tanhD = s * (1 - s);

        // Apply tanh derivative (chain rule)
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i].gradient *= tanhD;
            input[i].gradient += weights[i].gradient * weights[i].value;
        }
        bias.gradient *= tanhD;

//        const float step = 0.01f;

//        // analytical
//        var g = (2f * Mathf.Exp(bias.value)) / (Mathf.Exp(bias.value) + 1).Squared();
////        var g = (2f * Mathf.Exp(bias.value)) / (Mathf.Exp(bias.value) + 1).Squared();
//
//        for (int i = 0; i < weights.Length; i++)
//        {
//            var orig = weights[i].value;
//            var a = Calculate();
//            weights[i].value += step;
//            var b = Calculate();
//            weights[i].value = orig;
//
//            weights[i].gradient = (b - a) / step;
//
////            weights[i].gradient = g * input[i].value;
//        }
//        {
//            var orig = bias.value;
//            var a = Calculate();
//            bias.value += step;
//            var b = Calculate();
//            bias.value = orig;
//
//            bias.gradient = (b - a) / step;
//
////            bias.gradient = g;
////            print(bias.gradient - g);
////            log = (bias.gradient - g);
//        }
//        // TODO - try it first with numerical gradient calculation for deriviative, then use to compare against analytical gradient
////        var g = gradient;//1f / gradient;
////        g = -g;
//
//        //        var 
////        var st = EditorStyles.whiteLabel;
////        st.richText = true;
////
////        for (int i = 0; i < weights.Length; i++)
////        {
////            var p = (transform.position + input[i].transform.position) / 2f - new Vector3(1f, -1f);
////
////            Handles.Label(p, weights[i].value.ToString("+#0.000;-#0.000") + " <color=yellow>" + weights[i].gradient.ToString("+#0.000;-#0.000") + "</color>", st);
////        }
////        Handles.Label(transform.position + new Vector3(0.5f, 0.5f), 
////            bias.value.ToString("+#0.000;-#0.000") + " <color=yellow>" + bias.gradient.ToString("+#0.000;-#0.000") + "</color>", st);
//        //        if (parameter.Length > 0)
//        //        {
//        //            Handles.Label(transform.position + new Vector3(0.5f, 0.5f), parameter[0].ToString("+#0.000;-#0.000") + 
//        //                " <color=yellow>" + parameterGradient[0].ToString("+#0.000;-#0.000") + "</color>", EditorStyles.whiteLabel);
//        //        }
//
////        var total = 0f;
////        for (int i = 0; i < parameter.Length - 1; i++)
////        {
////            var s = 1 / (1 + Mathf.Exp(-input[i].value * parameter[i + 1]));
////            s = (s * (1 - s));
//////            var sigGrad = (s * (1 - s)) * g;
//////            var p = parameterGradient[i + 1] * sigGrad;
//////            p = 5;
//////            print(p);
//////            p = parameterGradient[i + 1];
////            parameterGradient[i + 1] = s * gradient;
////            input[i].gradient += s * gradient;
////            total += s * gradient;
////        }
////        total /= (parameter.Length - 1);
////        parameterGradient[0] = total;
////        parameterGradient[0] = g;
//
////        input[0].gradient += (s * (1 - s)) * gradient;
    }
    public override void AddForce(float force)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            weights[i].value += weights[i].gradient * force;
//            weights[i] += parameterGradient[i] * timeStep;
        }
        bias.value += bias.gradient * force;
//        variables[i].value += force * variables[i].gradient;
    }
    #if UNITY_EDITOR
    public override void OnDrawGizmos()
    {
//        return;
        base.OnDrawGizmos();

//        Debug.Log(log);

        var st = EditorStyles.whiteLabel;
        if (st != null)
        {
            st.richText = true;
        }

        for (int i = 0; i < weights.Length; i++)
        {
            var p = (transform.position + input[i].transform.position) / 2f - new Vector3(1f, -1f);

            Handles.Label(p, weights[i].value.ToString("+#0.000;-#0.000") + " <color=yellow>" + weights[i].gradient.ToString("+#0.000;-#0.000") + "</color>", st);
        }
        if (bias)
        {
            Handles.Label(transform.position + new Vector3(0.5f, 0.5f), 
                bias.value.ToString("+#0.000;-#0.000") + " <color=yellow>" + bias.gradient.ToString("+#0.000;-#0.000") + "</color>", st);
        }
        
//        var index = 0;
//        foreach (var item in input)
//        {
//            var a = transform.position + new Vector3(0.5f, -0.5f);
//            var b = item.transform.position + new Vector3(0.5f, -0.5f);
//            Debug.DrawLine(a, b, Color.gray);
//
//            if (index < weights.Length)
//            {
//                var st = EditorStyles.whiteLabel;
//                st.richText = true;
//                Handles.Label((a + b) / 2f - new Vector3(0.5f, -0.5f), parameter[index + 1].ToString("+#0.000;-#0.000") + 
//                    " <color=yellow>" + parameterGradient[index + 1].ToString("+#0.000;-#0.000") + "</color>", st);
//            }
//
//            index++;
//        }
//        if (parameter.Length > 0)
//        {
//            Handles.Label(transform.position + new Vector3(0.5f, 0.5f), parameter[0].ToString("+#0.000;-#0.000") + 
//                " <color=yellow>" + parameterGradient[0].ToString("+#0.000;-#0.000") + "</color>", EditorStyles.whiteLabel);
//        }
    }
    public override string Display()
    {
        return "[n] " + base.Display();
    }
    #endif
}