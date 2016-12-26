using UnityEngine;
using System.Collections;

public class CONTENT_Neuron : MonoBehaviour 
{
    public Node input; 
    public Node output;

    public void Awake()
    {
//        gameObject.AddComponent<CONTENT_ValueNode>();

//        var threadValue = gameObject.AddComponent<CONTENT_ValueNode>();
//        var threadMultiply = gameObject.AddComponent<CONTENT_MultiplyNode>();

        var add = gameObject.AddComponent<CONTENT_NodeAdd>();

        var bias = gameObject.AddComponent<CONTENT_NodeValue>();
        if (CONTENT_ManagerNeuron.instance.RandomizeStartValues)
        {
            bias.value = Random.Range(-0.1f, 0.1f);
        }
        CONTENT_Connection.Create(bias, add).name = "connection bias -> add";

        var squish = gameObject.AddComponent<CONTENT_NodeTanh>();
//        var sig = gameObject.AddComponent<CONTENT_NodeSigmoid>();
        CONTENT_Connection.Create(add, squish);
        squish.display = gameObject.AddComponent<CONTENT_Display>();

//        var multiplyConnect

        input = add;
        output = squish;


    }
}
