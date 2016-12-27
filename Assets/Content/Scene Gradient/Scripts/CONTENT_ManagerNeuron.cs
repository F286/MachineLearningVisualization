using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;

public class CONTENT_ManagerNeuron : MonoBehaviour 
{
    [System.Serializable]
    public class NodeInput
    {
        public int[] inputs;
        public int node;

        public NodeInput(int[] i, int n)
        {
            inputs = i;
            node = n;
        }
    }
    public bool GenerateGraphFromCode = true;
    public bool RandomizeStartValues = true;

    public List<Node> nodes;
    public List<CONTENT_Connection> connections;

    public List<NodeInput> leftToRight = new List<NodeInput>();
    public Gradient gradient;

    public List<Transform> points;
    public List<SpriteRenderer> grid;

    public List<CONTENT_NodeValue> input = new List<CONTENT_NodeValue>();
    public CONTENT_NodeAdd output;

    public void Awake()
    {
        if (!GenerateGraphFromCode)
        {
            return;
        }
        Random.InitState(0);

//        return;
        points = new List<Transform>();
        for (float x = -1f; x < 1f; x += 0.1f)
        {
            for (float y = -1f; y < 1f; y += 0.1f)
            {
                var p = new GameObject("grid", typeof(SpriteRenderer));
                p.transform.parent = transform;
                p.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Box Neuron");
                p.transform.localScale = new Vector3(0.1f, 0.1f, 1f);
                grid.Add(p.GetComponent<SpriteRenderer>());
                p.transform.localPosition = new Vector3(x, y, 0);

                var c = gradient.Evaluate(0);
                c.a = 0.4f;
                p.GetComponent<SpriteRenderer>().color = c;
                p.GetComponent<SpriteRenderer>().sortingOrder = -100;
            }
        }
        for (int i = 0; i < 30; i++) 
        {
            var p = new GameObject("point", typeof(SpriteRenderer));
            p.transform.parent = transform;
            p.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Box Neuron");
            p.transform.localScale = new Vector3(0.05f, 0.05f, 1f);
            points.Add(p.transform);
            if (i < 16)
            {
                p.GetComponent<SpriteRenderer>().color = gradient.Evaluate(0);
                p.transform.localPosition = Random.insideUnitCircle * 0.5f;
//                p.transform.localPosition = Random.insideUnitCircle * 0.3f + new Vector2(-0.5f, 0);
                p.tag = "low";
            }
            else
            {
                p.GetComponent<SpriteRenderer>().color = gradient.Evaluate(1);
                p.transform.localPosition = Random.insideUnitCircle.normalized * 0.7f;
//                p.transform.localPosition = Random.insideUnitCircle.normalized * 0.3f + new Vector2(0.5f, 0);
                p.tag = "high";
            }
        }

        var layer1 = new List<CONTENT_NeuronLstm>();

        // input
        for (int i = 0; i < 2; i++)
        {
            var g = new GameObject("input 0 (" + i + ")");
            g.transform.localPosition = new Vector3(0, i);
            var v = g.AddComponent<CONTENT_NodeValue>();
            v.value = Random.Range(-0.5f, 0.5f);
            v.display = v.gameObject.AddComponent<CONTENT_Display>();
            v.canTrain = false;
            input.Add(v);
        }
        // layer 1
        for (int i = 0; i < 1; i++)
        {
            var g = new GameObject("layer 1 (" + i + ")");
            g.transform.localPosition = new Vector3(1, i * 0.4f);
            var n = g.AddComponent<CONTENT_NeuronLstm>();
            foreach (var a in input)
            {
                foreach (var b in n.input)
                {
                    CONTENT_ConnectionWeighted.Create(a, b);
                }
//                CONTENT_ConnectionWeighted.Create(item, n.input);
            }
            layer1.Add(n);
        }


//        // input
//        for (int i = 0; i < 2; i++)
//        {
//            var g = new GameObject("input 0 (" + i + ")");
//            g.transform.localPosition = new Vector3(0, i);
//            var v = g.AddComponent<CONTENT_NodeValue>();
//            v.value = Random.Range(-0.5f, 0.5f);
//            v.display = v.gameObject.AddComponent<CONTENT_Display>();
//            v.canTrain = false;
//            input.Add(v);
//        }
//
//        // layer 1
//        for (int i = 0; i < 6; i++)
//        {
//            var g = new GameObject("layer 1 (" + i + ")");
//            g.transform.localPosition = new Vector3(1, i * 0.4f);
//            var n = g.AddComponent<CONTENT_Neuron>();
//            foreach (var item in input)
//            {
//                CONTENT_ConnectionWeighted.Create(item, n.input);
//            }
//            layer1.Add(n);
//        }
//        // layer 2
//        for (int i = 0; i < 3; i++)
//        {
//            var g = new GameObject("layer 2 (" + i + ")");
//            g.transform.localPosition = new Vector3(2, i * 0.66f);
//            var n = g.AddComponent<CONTENT_Neuron>();
//            foreach (var item in layer1)
//            {
//                CONTENT_ConnectionWeighted.Create(item.output, n.input);
//            }
//            layer2.Add(n);
//        }
//
        // output
        {
            var g = new GameObject("output");
            g.transform.localPosition = new Vector3(3, 0);
            output = g.AddComponent<CONTENT_NodeAdd>();
            output.tag = "output";
            output.display = output.gameObject.AddComponent<CONTENT_Display>();
            foreach (var item in layer1)
            {
                CONTENT_ConnectionWeighted.Create(item.output, output);
//                CONTENT_Connection.Create(item.output, output, true);
            }
        }
//        CONTENT_ConnectionWeighted.Create(input[0], output);
    }
    public void Start()
    {
        EvaluateConnections(GameObject.FindGameObjectWithTag("output").GetComponent<Node>());
    }
    public void Update()
    {
//        Evaluate(new Vector2(1, 1), true, 1);
//
//        var numericalStep = 0.0001f;
//        var valueOrig = input[0].value;
//        var a = Evaluate();
//        input[0].value += numericalStep;
//        var b = Evaluate();
//        input[0].value = valueOrig;
//        print((b - a) / numericalStep);
        if (GenerateGraphFromCode)
        {
            for (int index = 0; index < trainSteps; index++)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Evaluate(points[i].position, true, points[i].CompareTag("high") ? 1 : -1, true);
                }
            }
        }
        else
        {
            Evaluate(true, null);
        }
        for (int i = 0; i < grid.Count; i++)
        {
            var c = gradient.Evaluate(Evaluate(grid[i].transform.localPosition));
            c.a = 0.4f;
            grid[i].color = c;
        }
    }
    public float trainSpeed = 1;
    public int trainSteps = 50;
    public float Evaluate(Vector2 inputSet, bool derive = false, float? target = null, bool train = false)
    {   
        input[0].value = inputSet.x;
        input[1].value = inputSet.y;
        return Evaluate(derive, target, train);
    }
    public float Evaluate(bool derive = false, float? target = null, bool train = false)
    {
        //forward
        for (int i = 0; i < leftToRight.Count; i++)
        {
            var pass = new Node[leftToRight[i].inputs.Length];
            for (int n = 0; n < pass.Length; n++) 
            {
                pass[n] = nodes[leftToRight[i].inputs[n]];
            }
            nodes[leftToRight[i].node].DebugInputs = pass;
            nodes[leftToRight[i].node].forward(pass);
        }

        if (derive)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].derivative = 0;
            }
            var o = GameObject.FindGameObjectWithTag("output").GetComponent<Node>();
            if (target.HasValue)
            {
                o.derivative = (target.Value - o.value);
                o.derivative = System.Math.Sign(o.derivative) * System.Math.Abs(o.derivative);
            }
            else
            {
                o.derivative = 1;
            }
            //backwards
            for (int i = leftToRight.Count - 1; i >= 0; i--)
            {
                var pass = new Node[leftToRight[i].inputs.Length];
                for (int n = 0; n < pass.Length; n++)
                {
                    pass[n] = nodes[leftToRight[i].inputs[n]];
                }
                nodes[leftToRight[i].node].backward(pass);
            }
        }
        if(train)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].train(trainSpeed);
            }
        }
//        return (float)output.value;
        return (float)GameObject.FindGameObjectWithTag("output").GetComponent<Node>().value;
    }
    List<Node> alreadyAdded = new List<Node>();
    void EvaluateConnections(Node node)
    {
        if (!alreadyAdded.Contains(node))
        {
            alreadyAdded.Add(node);
            var inputs = new List<int>();
            foreach (var item in GetInputs(node))
            {
                inputs.Add(GetIndex(item));
                EvaluateConnections(item);
            }
            if (inputs.Count > 0)
            {
//                print(node + " - " + inputs.Count);
                leftToRight.Add(new NodeInput(inputs.ToArray(), GetIndex(node)));
            }
        }
    }
    public IEnumerable<Node> GetInputs(Node to)
    {
        foreach (var item in connections)
        {
            if (item.to == to)
            {
                yield return item.from;
            }
        }
    }
    public int GetIndex(Node item)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i] == item)
            {
                return i;
            }
        }
        return -1;
    }
     
    static CONTENT_ManagerNeuron _inst;
    public static CONTENT_ManagerNeuron instance
    {
        get
        {
            if(_inst == null)
            {
                _inst = GameObject.FindObjectOfType<CONTENT_ManagerNeuron>();
            }
            return _inst;
        }
    }
}
