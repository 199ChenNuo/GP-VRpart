using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    private Parser parser;
    public List<Node> nodes = new List<Node>();
    public List<Beam> beams = new List<Beam>();
    public List<Face> faces = new List<Face>();

    // for debug

    // draw faces
    // public Material material;
    public MeshController meshController;
    public List<Vector3> vertices;
    private List<float> forces;

    private bool parsedone = false;

    // Start is called before the first frame update
    void Start()
    {
        /*parser = new Parser();
        parser.Parse();
        // parser.Print();
        nodes = parser.nodes;
        beams = parser.beams;
        faces = parser.faces;

        vertices = parser.verts;
        forces = new List<float>(vertices.Count);
        meshController.CreateMesh(vertices, parser.triangles);
    */
    }

    public void Load(string filename)
    {
        parsedone = false;
        if(parser != null)
        {
            parser.Clear();
        }
        else
        {
            parser = new Parser();
        }
        parser.filename = filename;
        parser.Parse();
        // parser.Print()       
        nodes = parser.nodes;
        beams = parser.beams;
        faces = parser.faces;

        vertices = parser.verts;
        forces = new List<float>(vertices.Count);
        meshController.CreateMesh(vertices, parser.triangles);
        parsedone = true;
    }
    public void Export()
    {
        meshController.Export(parser.filename);
    }
    // Update is called once per frame
    void Update()
    {
        if (parsedone)
        {
            // 计算 F_axial
            for (int i = 0; i < nodes.Count; ++i)
            {
                // 顺便把上一次update计算的各种力清空
                nodes[i].ClearF();
                nodes[i].updateF_axial();
            }

            // 计算 F_crease
            // 计算 F_dumping
            for (int i = 0; i < beams.Count; ++i)
            {
                // TBD
                beams[i].updateF_crease();
                beams[i].updateF_dumping();
            }

            // 计算 F_face
            for (int i = 0; i < faces.Count; ++i)
            {
                // TBD
                faces[i].updateF_face();
            }

            // 更新Node位置及速度
            // vertices.Clear();
            vertices.Clear();
            forces.Clear();
            for (int i = 0; i < nodes.Count; ++i)
            {
                nodes[i].updateVel();
                nodes[i].updatePosition();
                // vertices.Add(nodes[i].position);
                vertices.Add(nodes[i].position);
                forces.Add(nodes[i].F_total);
            }

            // 更新Beam长度
            for (int i = 0; i < beams.Count; ++i)
            {
                beams[i].updateL();
            }

            // 更新Face
            // 有点问题 TBD
            /*
            for(int i=0; i<faces.Count; ++i)
            {
                // faces[i].Draw();
            }
            */
            meshController.UpdateMesh(vertices, forces);
        }
        
    }

   
}
