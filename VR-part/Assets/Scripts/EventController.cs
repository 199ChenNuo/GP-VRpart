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

    // draw faces
    public Material material;
    public List<Vector3> vertices = new List<Vector3>();
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;

     // Start is called before the first frame update
    void Start()
    {
        parser = new Parser();
        parser.Parse();
        parser.Print();
        nodes = parser.nodes;
        beams = parser.beams;
        faces = parser.faces;

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
        vertices.Clear();
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
            vertices.Add(nodes[i].position);
        }

        for(int i=0; i<beams.Count; ++i)
        {
            beams[i].updateL();
        }

        // 这部分是画面的，因为mesh是整体一起画的，所以不是 
        // 很好逐face画，就放到EventController里面整体画了
        Draw();
    }

    [ContextMenu("Draw")]
    public void Draw()
    {
        // updateVertives();
        Vector2[] vertices2D = new Vector2[vertices.Count];
        Vector3[] vertices3D = new Vector3[vertices.Count];
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertice = vertices[i] + transform.position;
            vertices2D[i] = new Vector2(vertice.x, vertice.y);
            vertices3D[i] = vertice;
        }

        Triangulator tr = new Triangulator(vertices2D);
        int[] triangles = tr.Triangulate();

        Mesh mesh = new Mesh();
        mesh.vertices = vertices3D;
        mesh.triangles = triangles;

        if (meshRenderer == null)
        {
            meshRenderer = gameObject.GetOrAddComponent<MeshRenderer>();
        }
        meshRenderer.material = material;
        if (meshFilter == null)
        {
            meshFilter = gameObject.GetOrAddComponent<MeshFilter>();
        }
        meshFilter.mesh = mesh;
    }
}
