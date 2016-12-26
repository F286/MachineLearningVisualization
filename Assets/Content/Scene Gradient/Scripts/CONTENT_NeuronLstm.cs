using UnityEngine;
using System.Collections;

public class CONTENT_NeuronLstm : MonoBehaviour 
{
    public Node[] input; 
    public Node output;

    public void Awake()
    {
        //input and squishing
        var inputAdd = gameObject.AddComponent<CONTENT_NodeAdd>();
        var rememberAdd = gameObject.AddComponent<CONTENT_NodeAdd>();
        var forgetAdd = gameObject.AddComponent<CONTENT_NodeAdd>();
        var outputAdd = gameObject.AddComponent<CONTENT_NodeAdd>();

        var inputSquish = gameObject.AddComponent<CONTENT_NodeSigmoid>();
        var rememberSquish = gameObject.AddComponent<CONTENT_NodeSigmoid>();
        var forgetSquish = gameObject.AddComponent<CONTENT_NodeSigmoid>();
        var outputSquish = gameObject.AddComponent<CONTENT_NodeSigmoid>();

        CONTENT_Connection.Create(inputAdd, inputSquish);
        CONTENT_Connection.Create(rememberAdd, rememberSquish);
        CONTENT_Connection.Create(forgetAdd, forgetSquish);
        CONTENT_Connection.Create(outputAdd, outputSquish);

        //scale input
        var inputScalar = gameObject.AddComponent<CONTENT_NodeValue>();
        var inputScaled = gameObject.AddComponent<CONTENT_NodeMultiply>();

        CONTENT_Connection.Create(inputSquish, inputScaled);
        CONTENT_Connection.Create(inputScalar, inputScaled);

        //running total
        var runningTotal = gameObject.AddComponent<CONTENT_NodeAdd>();
        CONTENT_Connection.Create(inputScalar, inputScaled);

        //recursive
        var lastFrameScaled = gameObject.AddComponent<CONTENT_NodeMultiply>();

        CONTENT_Connection.Create(lastFrameScaled, runningTotal);
        CONTENT_Connection.Create(inputScaled, runningTotal);

        CONTENT_Connection.Create(runningTotal, lastFrameScaled);
        CONTENT_Connection.Create(forgetSquish, runningTotal);

        //output
        var outp = gameObject.AddComponent<CONTENT_NodeMultiply>();

        CONTENT_Connection.Create(runningTotal, outp);
        CONTENT_Connection.Create(outputSquish, outp);

        //display
        runningTotal.display = gameObject.AddComponent<CONTENT_Display>();

        //        gameObject.AddComponent<CONTENT_ValueNode>();

        //        var threadValue = gameObject.AddComponent<CONTENT_ValueNode>();
        //        var threadMultiply = gameObject.AddComponent<CONTENT_MultiplyNode>();

//        var add = gameObject.AddComponent<CONTENT_NodeAdd>();
//
//        var bias = gameObject.AddComponent<CONTENT_NodeValue>();
//        if (CONTENT_ManagerNeuron.instance.RandomizeStartValues)
//        {
//            bias.value = Random.Range(-0.1f, 0.1f);
//        }
//        CONTENT_Connection.Create(bias, add).name = "connection bias -> add";
//
//        var squish = gameObject.AddComponent<CONTENT_NodeTanh>();
//        //        var sig = gameObject.AddComponent<CONTENT_NodeSigmoid>();
//        CONTENT_Connection.Create(add, squish);
//        squish.display = gameObject.AddComponent<CONTENT_Display>();

        //        var multiplyConnect

        input = new Node[4] { inputAdd, rememberAdd, forgetAdd, outputAdd };
        output = outp;


    }
}
