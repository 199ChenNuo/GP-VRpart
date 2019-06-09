using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public Parser parser;
    public List<Node> nodes = new List<Node>();
    public List<Beam> beams = new List<Beam>();
    public List<Face> faces = new List<Face>();

     // Start is called before the first frame update
    void Start()
    {
        nodes = parser.node;
        beams = parser.beam;
        faces = parser.face;
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
    }
}
