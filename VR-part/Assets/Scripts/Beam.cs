using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public long index;
    public float k_axial = 0.7f;

    public Node n1 = new Node();
    public Node n2 = new Node();

    // length
    public float l;
    public float l_0;

    public void setNode1(Node _n1)
    {
        n1 = _n1;
       //  _n1.addBeam(this);
    }

    public void setNode2(Node _n2)
    {
        n2 = _n2;
        // _n2.addBeam(this);
    }

    public void setL(float _l)
    {
        l = _l;
    }

    public void setL_0(float _l_0)
    {
        l_0 = _l_0;
    }

    public void setIndex(long i)
    {
        index = i;
    }

    public Vector3 getF(Node n)
    {
        Vector3 I12 = (n1.position - n2.position).normalized;
        if (n == n1)
        {
            I12 = (n2.position - n1.position).normalized;
        }
       
        return -k_axial * (l - l_0) * I12;
    }

    public void updateF_crease()
    {
        // ...
        n1.F_crease = Vector3.zero;
        n2.F_crease = Vector3.zero;
    }

    public void updateF_dumping()
    {
        // ...
        n1.F_dumping = Vector3.zero;
        n2.F_dumping = Vector3.zero;
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
