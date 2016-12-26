using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;

public class CONTENT_GraphManager : MonoBehaviour 
{
    public float stepSize = 0.001f;
    public int steps = 30;

    public List<Gate> input;
    public List<Gate> all;
    public List<Gate> variables;
    public Gate output;

    public Vector2[] red;
    public Vector2[] blue;

    public void Awake()
    {
//        red = new Vector2[20];
//        blue = new Vector2[60];
        red = new Vector2[8]; 
        blue = new Vector2[8];

        for (int i = 0; i < red.Length; i++)
        {
//            red[i] = Random.insideUnitCircle * 3f;
            red[i] = UnityEngine.Random.insideUnitCircle * 1.5f;
//            red[i] = Random.insideUnitCircle * 2f;
        }
//        for (int i = 0; i < blue.Length; i++)
//        {
//            blue[i] = Random.insideUnitCircle * 2 + new Vector2(5, 0);
//        }
        for (int i = 0; i < blue.Length; i++)
        {
//            blue[i] = Random.insideUnitCircle * 3f + new Vector2(2, 0);
            blue[i] = UnityEngine.Random.insideUnitCircle.normalized * UnityEngine.Random.Range(3f, 3.5f);// + new Vector2(3, 0);
        }

    }
    public void Update()
    {
        for (int s = 0; s < steps; s++)
        {
            for (int i = 0; i < red.Length; i++)
            {
                Evaluate(red[i]);
                Train(1);
            }
            for (int i = 0; i < blue.Length; i++)
            {
                Evaluate(blue[i]);
//                Train(0);
                Train(-1);
            }
        }
    }
    public double Evaluate(Vector2 v)
    {
        input[0].value = v.x;
        input[1].value = v.y;
        //reset
        for (int i = 0; i < all.Count; i++) 
        {
            all[i].gradient = 0;
        }
        output.gradient = 1;

        //forward
        for (int index = 0; index < all.Count; index++) 
        {
            all[index].Forward(all[index].input);
        }
        for (int index = all.Count - 1; index >= 0; index--) 
        {
            all[index].Backward();
        }
        return output.value;
    }
    public void Train(float value)
    {
        var force = Math.Sign(-value - output.value) * stepSize;
        for (int i = 0; i < all.Count; i++)
        {
            all[i].AddForce(force);
        }
    }
    public void OnDrawGizmos()
    {
        Color r = Color.red;
        r.a = 0.7f;
        Color b = Color.blue;
        b.a = 0.7f;
        for (float x = -4; x < 8; x += 0.1f)
        {
            for (float y = -4; y < 4; y += 0.1f)
            {
//                Gizmos.color = Color.Lerp(r, b, Evaluate(new Vector2(x, y)));
                Gizmos.color = Color.Lerp(r, b, 2f * ((float)Evaluate(new Vector2(x, y)) - 0.5f));
                Gizmos.DrawCube(new Vector3(x, y), new Vector3(0.1f, 0.1f, 0.1f));
            }
        }
        foreach (var item in red)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(item, 0.16f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(item, 0.15f);
        }
        foreach (var item in blue)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(item, 0.16f);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(item, 0.15f);
        }
        Evaluate(new Vector2(0, 0));
//        Handles.Label(new Vector3(-1, -1), Evaluate(new Vector2(-1, -1)).ToString("+#0.000;-#0.000"), EditorStyles.whiteLabel);
//        Handles.Label(new Vector3(1, -1), Evaluate(new Vector2(1, -1)).ToString("+#0.000;-#0.000"), EditorStyles.whiteLabel);
//        Handles.Label(new Vector3(-1, 1), Evaluate(new Vector2(-1, 1)).ToString("+#0.000;-#0.000"), EditorStyles.whiteLabel);
//        Handles.Label(new Vector3(1, 1), Evaluate(new Vector2(1, 1)).ToString("+#0.000;-#0.000"), EditorStyles.whiteLabel);
    }
}
