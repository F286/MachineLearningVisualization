using UnityEngine;
using System.Collections;

public class CONTENT_CreateConnection : MonoBehaviour 
{
    public Node from;
    public Node to;
    public bool createDisplay = true;
    public float offset;

    public void Reset()
    {
        from = gameObject.GetComponent<Node>();
    }

	public void Awake () 
    {
        CONTENT_Connection.Create(from, to, createDisplay, offset);
	}
}
