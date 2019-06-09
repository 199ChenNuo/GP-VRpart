using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : MonoBehaviour
{
    public long index;
    public List<Node> nodes = new List<Node>();

    public void addNode(Node n)
    {
        nodes.Add(n);
    }

    public void setIndex(long _index)
    {
        index = _index;
    }

    public void updateF_face()
    {
        // ...
        for(int i=0; i<nodes.Count; ++i)
        {
            nodes[i].F_face = Vector3.zero;
        }
    }

    public void refresh()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
