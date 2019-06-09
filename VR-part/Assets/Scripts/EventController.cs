using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public Parser parser;
    public List<Node> nodes = new List<Node>();
    public List<Beam> beams = new List<Beam>();
    public List<Face> faces = new List<Face>();

    // for debug
   // public LineRenderer line;

     // Start is called before the first frame update
    void Start()
    {
        parser = new Parser();
        parser.parse();
        nodes = parser.node;
        beams = parser.beam;
        faces = parser.face;

        // line = GameObject.Find("LineRender").GetComponent<LineRenderer>();
        // line.positionCount = 6;
        // line.startColor = Color.red;
        // line.endColor = Color.red;
        // line.startWidth = 0.1f;
        // line.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<nodes.Count; ++i)
        {
            nodes[i].updateF_axial();
        }

        for(int i=0; i<beams.Count; ++i)
        {
            // 
            beams[i].updateF_crease();
            beams[i].updateF_dumping();
        }

        for(int i=0; i<faces.Count; ++i)
        {
            //
            faces[i].updateF_face();
        }

        for(int i=0; i<nodes.Count; ++i)
        {
            nodes[i].updateVel();
            nodes[i].updatePosition();
        }

        for(int i=0; i<beams.Count; ++i)
        {
            beams[i].updateL();
        }

        for(int i=0; i<faces.Count; ++i)
        {
            faces[i].refresh();
        }
    }
}
