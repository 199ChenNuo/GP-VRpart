﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    // ==============================================
    // attributes
    // ==============================================

    public enum Type { Border, Mountain, Valley, Facet, Unknown }
    public long index;

    // pre-set attribute
    public float k_axial;
    public float k_crease;

    public Node p3 = new Node();    // p3
    public Node p4 = new Node();    // p4
    public Node neigh_p1 = new Node();  //p1
    public Node neigh_p2 = new Node();  //p2

    public Type type;

    // face consists of [p1, p3, p4]
    public Face f1 = new Face();
    // face consists of [p2, p3, p4]
    public Face f2 = new Face();

    // current theta between 2 faces
    public float theta;
    // desired theta between 2 faces
    public float theta_target;

    // current length
    public float l;
    // original length
    public float l_0;

    private float EA = 20;

    // ==============================================
    // functions: set up beam
    // ==============================================

    public void SetNode1(Node _n1)
    {
        p3 = _n1;
    }

    public void SetNode2(Node _n2)
    {
        p4 = _n2;
    }

    public void SetP1(Node n)
    {
        neigh_p1 = n;
    }

    public void SetP2(Node n)
    {
        neigh_p2 = n;
    }

    public void SetNode(Node n){
        if(p3 == null)
            p3 = n;
        else
            p4 = n;
    }

    public Node getOther(Node n)
    {
        if (n == p3)
            return p4;
        return p3;
    }

    public void SetL(float _l)
    {
        l = _l;
    }

    public void SetL_0(float _l_0)
    {
        l_0 = _l_0;
        k_axial = EA / l_0;
    }

    public void SetIndex(long i)
    {
        index = i;
    }

    public void SetTheta(float a)

    {
        theta = a;
    }


    public void SetThetaTarget(float a)
    {
        // <0   mountain crease
        // >0   valley crease
        // 0      facet crease
        if (this.type == Type.Mountain)
            theta_target = a > 0 ? a - 180f : a;
        else if (this.type == Type.Valley)
            theta_target = a > 0 ? a : a + 180f;
        else
            theta_target = 0;
    }

    // k_crease is also set in this function
    public void SetType(string s)
    {
        if (s == "\"B\"")
        {
            type = Type.Border;
            k_crease = 0;
        }
        else if (s == "\"M\"")
        {
            type = Type.Mountain;
            k_crease = l_0 * 0.7f;
        }
        else if (s == "\"V\"")
        {
            type = Type.Valley;
            k_crease = l_0 * 0.7f;
        }
        else if (s == "\"F\"")
        {
            type = Type.Facet;
            k_crease = l_0 * 0.7f;
        }
        else
        {
            type = Type.Unknown;
            k_crease = 0;
        }
    }

    // ==============================================
    // functions: called by Node.cs
    //              calculate F_axial for one beam on a node
    // ==============================================

    public Vector3 getF(Node n)
    {
        // I_12 is the unit vector from node 1 to node 2
        Vector3 I12 = (p4.position - p3.position).normalized;
        if (n == p3)
        {
            I12 = -I12;
        }
               
        return -k_axial * (l - l_0) * I12;
    }

    // ==============================================
    // functions: called by EventController
    //              update F_crease and F_dumping for nodes
    // ==============================================
    public void updateF_crease()
    {
        // height of two faces
        float h1 = DisPoint2Line(neigh_p1.position, p3.position, p4.position);
        float h2 = DisPoint2Line(neigh_p2.position, p3.position, p4.position);

        // normal of 2 faces
        // Vector3 n1 = Vector3.Cross(p3.position - neigh_p1.position, p4.position - neigh_p1.position).normalized;
        // Vector3 n2 = Vector3.Cross(p3.position - neigh_p2.position, p4.position - neigh_p2.position).normalized;
        Vector3 n1 = Vector3.Cross(neigh_p1.position - p3.position, neigh_p1.position - p4.position);
        Vector3 n2 = Vector3.Cross(neigh_p2.position - p4.position, neigh_p2.position - p3.position);
        
        // update theta
        float angle = getAngle(n1, n2);
        if (this.type == Type.Mountain)
        {
            theta = rounddown(angle);

        }
        else if (this.type == Type.Valley)
        {
            theta = roundup(angle);

        }
        else if (this.type == Type.Facet)
        {
            theta = angle;
        }
        else
            return;


        // 4 angle in 2 faces
        float alpha3_14 = getAngle(p3.position - neigh_p1.position, p3.position - p4.position);
        float alpha3_42 = getAngle(p3.position - p4.position, p3.position - neigh_p2.position);
        float alpha4_31 = getAngle(p4.position - p3.position, p4.position - neigh_p1.position);
        float alpha4_23 = getAngle(p4.position - neigh_p2.position, p4.position - p3.position);


        float k = -k_crease * theta * Mathf.Deg2Rad;
        // k *= 0.1f;
        // update F_crease for 4 beams
        neigh_p1.F_crease += k * getODE1(n1, h1);
        neigh_p2.F_crease += k * getODE2(n2, h2);
        p3.F_crease += k * getODE3(n1, n2, h1, h2, alpha4_31, alpha3_14, alpha4_23, alpha3_42);
        p4.F_crease += k * getODE4(n1, n2, h1, h2, alpha3_14, alpha4_31, alpha3_42, alpha4_23);
    }

    public void updateF_dumping()
    {
        //previous defination
        float zeta = 0.1f;     //0.01~0.5
        float c = 2 * zeta * Mathf.Sqrt(k_axial*p3.mass);//mass=1

        // get velocity 
        // here simply use the other node of the beam as the neighbour node
        Vector3 v3 = p3.vel;
        Vector3 v4 = p4.vel;

        //F_dumping
        p3.F_dumping += c * (v4 - v3);
        p4.F_dumping += c * (v3 - v4); 

        // 那个字母叫zeta
    }

    public void updateL()
    {
        l = Vector3.Distance(p3.position, p4.position);
    }

    // ==============================================
    // functions: utils that helps doing calculate
    // ==============================================

    public Vector3 getODE1(Vector3 n1, float h1)
    {
        Vector3 tmp = n1 / h1;
        return tmp;

    }

    public float round(float f)
    {
        if (f < -0.5)
            return -0.5f;
        if (f > 0.5)
            return 0.5f;
        return f;
    }

    public Vector3 getODE2(Vector3 n2, float h2)
    {
        Vector3 tmp = n2 / h2;
        return tmp;

    }

    public Vector3 getODE3(Vector3 n1, Vector3 n2, float h1, float h2,
        float alpha4_31, float alpha3_14, float alpha4_23, float alpha3_42)
    {
        float cot4_31 = alpha4_31 == 90 ? 0 : 1 / Mathf.Tan(alpha4_31 * Mathf.Deg2Rad);
        float cot3_14 = alpha3_14 == 90 ? 0 : 1 / Mathf.Tan(alpha3_14 * Mathf.Deg2Rad);
        float cot4_23 = alpha4_23 == 90 ? 0 : 1 / Mathf.Tan(alpha4_23 * Mathf.Deg2Rad);
        float cot3_42 = alpha3_42 == 90 ? 0 : 1 / Mathf.Tan(alpha3_42 * Mathf.Deg2Rad);

        float k1 = -cot4_31 / (cot3_14 + cot4_31);
        float k2 = -cot4_23 / (cot3_42 + cot4_23);

        Vector3 tmp = k1 * (n1 / h1) + k2 * (n2 / h2);
        return tmp;

    }

    public Vector3 getODE4(Vector3 n1, Vector3 n2, float h1, float h2,
        float alpha3_14, float alpha4_31, float alpha3_42, float alpha4_23)
    {
        float cot3_14 = alpha3_14 == 90 ? 0 : 1 / Mathf.Tan(alpha3_14 * Mathf.Deg2Rad);
        float cot4_31 = alpha4_31 == 90 ? 0 : 1 / Mathf.Tan(alpha4_31 * Mathf.Deg2Rad);
        float cot3_42 = alpha3_42 == 90 ? 0 : 1 / Mathf.Tan(alpha3_42 * Mathf.Deg2Rad);
        float cot4_23 = alpha4_23 == 90 ? 0 : 1 / Mathf.Tan(alpha4_23 * Mathf.Deg2Rad);

        float k1 = -cot3_14 / (cot3_14 + cot4_31);
        float k2 = -cot3_42 / (cot3_42 + cot4_23);

        Vector3 tmp = k1 * (n1 / h1) + k2 * (n2 / h2);
        return tmp;

    }


    public float getAngle(Vector3 v1, Vector3 v2)
    {
        float angle = Vector3.Angle(v1, v2);
        while (angle < 0)
            angle += 180f;
        while (angle > 180f)
            angle -= 180f;
        if (Mathf.Abs(angle - 90) < 0.01)
            angle = 90;
        return angle;
    }

    public float initGetAngle(Vector3 v1, Vector3 v2)
    {
        float angle = Vector3.Angle(v1, v2);
        while (angle < 0)
            angle += 180f;
        while (angle > 180f)
            angle -= 180f;
        if (Mathf.Abs(angle - 90) < 0.01)
            angle = 90;
        if (Mathf.Abs(angle - 45) < 0.01)
            angle = 45;
        return angle;
    }


    /// <summary>
    /// 点到直线距离
    /// </summary>
    /// <param name="point">点坐标</param>
    /// <param name="linePoint1">直线上一个点的坐标</param>
    /// <param name="linePoint2">直线上另一个点的坐标</param>
    /// <returns></returns>
    public float DisPoint2Line(Vector3 point, Vector3 linePoint1, Vector3 linePoint2)
    {
        Vector3 vec1 = point - linePoint1;
        Vector3 vec2 = linePoint2 - linePoint1;
        Vector3 vecProj = Vector3.Project(vec1, vec2);
        float dis = Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(vec1), 2) - Mathf.Pow(Vector3.Magnitude(vecProj), 2));
        return dis;
    }

    public float rounddown(float a)
    {
        while (a >= 0)
            a -= 180;
        return a;
    }

    public float roundup(float a)
    {
        while (a <= 0)
            a += 180;
        return a;
    }
}



