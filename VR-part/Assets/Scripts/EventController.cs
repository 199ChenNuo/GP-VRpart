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

    // draw faces
    public MeshController meshController;
    public List<Vector3> vertices;
    private List<float> forces;

    private void Awake()
    {
        parser = new Parser();
        
    }

    // Start is called before the first frame update
    void Start()    {
        
        parser.Parse();
        //parser.Print();
        nodes = parser.nodes;
        beams = parser.beams;
        faces = parser.faces;

        vertices = parser.verts;
        forces = new List<float>(vertices.Count);
        meshController.CreateMesh(vertices, parser.triangles);

    }

    // Update is called once per frame
    void Update()
    {
        // 计算 F_axial
        for(int i=0; i<nodes.Count; ++i)
        {
            nodes[i].updateF_axial();
        }

        // 计算 F_crease
        // 计算 F_dumping
        for(int i=0; i<beams.Count; ++i)
        {
            // TBD
            beams[i].updateF_crease();
            beams[i].updateF_dumping();
        }

        // 计算 F_face
        for(int i=0; i<faces.Count; ++i)
        {
            // TBD
            faces[i].updateF_face();
        }

        // 更新Node位置及速度
        vertices.Clear();
        forces.Clear();
        for (int i=0; i<nodes.Count; ++i)
        {
            nodes[i].updateVel();
            nodes[i].updatePosition();
            vertices.Add(nodes[i].position);
            forces.Add(nodes[i].F_total);
        }

        // 更新Beam长度
        for(int i=0; i<beams.Count; ++i)
        {
            beams[i].updateL();
        }

        // 更新Face
        // 有点问题 TBD
        // Draw();
        meshController.UpdateMesh(vertices, forces);
    }
    
    public void OnExportOBJ()
    {
        meshController.Export(parser.filename);
    }
}
