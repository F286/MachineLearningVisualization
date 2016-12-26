using UnityEngine;
using System.Collections;
using UnityEditor;

public abstract class Gate : MonoBehaviour
{
    public Gate[] input = new Gate[0];
//    public float[] parameter = new float[0];
//    public float[] parameterGradient = new float[0];

    public double value;
    public double gradient; //Derivative

    protected virtual bool ShowGradient
    {
        get
        {
            return true;
        }
    }

    public virtual void Forward(params Gate[] v)
    {
        input = v;
    }
    public virtual void Backward()
    {
        
    }
    public virtual void AddForce(float timeStep)
    {
        
    }
    public virtual void OnDrawGizmos()
    {
//        return;
//        var index = 0;
        foreach (var item in input)
        {
            var a = transform.position + new Vector3(0.5f, -0.5f);
            var b = item.transform.position + new Vector3(0.5f, -0.5f);
            Debug.DrawLine(a, b, Color.gray);

//            if (index < weight.Length)
//            {
//                var st = EditorStyles.whiteLabel;
//                st.richText = true;
//                Handles.Label((a + b) / 2f - new Vector3(0.5f, -0.5f), parameter[index + 1].ToString("+#0.000;-#0.000") + 
//                    " <color=yellow>" + parameterGradient[index + 1].ToString("+#0.000;-#0.000") + "</color>", st);
//            }

//            index++;
        }
//        if (parameter.Length > 0)
//        {
//            Handles.Label(transform.position + new Vector3(0.5f, 0.5f), parameter[0].ToString("+#0.000;-#0.000") + 
//                " <color=yellow>" + parameterGradient[0].ToString("+#0.000;-#0.000") + "</color>", EditorStyles.whiteLabel);
//        }
            
        var s = EditorStyles.whiteLargeLabel;
        s.richText = true;
        s.fontSize = 20;
        Handles.Label(transform.position, Display(), s);
    }
    public virtual string Display()
    {
        var s = "";
        if (ShowGradient)
        {
            s = "<color=red>" + value.ToString("+#0.000;-#0.000") + "</color>" + " <color=yellow>" + gradient.ToString("+#0.000;-#0.000") + "</color>";
        }
        else
        {
            s = "<color=grey>" + value.ToString("+#0.000;-#0.000") + "</color>";
        }
        return s;
    }
}