using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public List<Node> node = new List<Node>();
    public List<Beam> beam = new List<Beam>();
    public List<Face> face = new List<Face>();

    // Start is called before the first frame update
    void Start()
    {
        Node n1 = new Node();
        n1.setPosition(new Vector3(0f, 0f, 0f));
        n1.setIndex(1);
        Node n2 = new Node();
        n2.setPosition(new Vector3(0f, 1f, 0f));
        n2.setIndex(2);
        Node n3 = new Node();
        n3.setPosition(new Vector3(1f, 1f, 0f));
        n3.setIndex(3);
        Node n4 = new Node();
        n4.setPosition(new Vector3(1f, 0f, 0f));
        n4.setIndex(4);

        Beam b1 = new Beam();
        b1.setIndex(1);
        Beam b2 = new Beam();
        b2.setIndex(2);
        Beam b3 = new Beam();
        b3.setIndex(3);
        Beam b4 = new Beam();
        b4.setIndex(4);
        Beam b5 = new Beam();
        b5.setIndex(5);

        b1.setNode1(n1);
        b1.setNode2(n2);

        b2.setNode1(n2);
        b2.setNode2(n3);

        b3.setNode1(n3);
        b3.setNode2(n4);

        b4.setNode1(n4);
        b4.setNode2(n1);

        b5.setNode1(n1);
        b5.setNode2(n2);

        n1.addBeam(b1);
        n1.addBeam(b4);

        n2.addBeam(b1);
        n2.addBeam(b2);

        n3.addBeam(b2);
        n3.addBeam(b3);

        n4.addBeam(b3);
        n4.addBeam(b4);

        Face f1 = new Face();
        Face f2 = new Face();

        f1.addNode(n1); f1.addNode(n3); f1.addNode(n4);
        f1.setIndex(1);
        f2.addNode(n1); f2.addNode(n2); f2.addNode(n3);
        f2.setIndex(2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void parse()
    {
        print("parse success");
    }

}
