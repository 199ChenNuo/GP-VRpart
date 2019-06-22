using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MeshController : MonoBehaviour
{
    public Material material;
    public Shader vertShader;
    public Shader normalShader;

    public enum Type{
        Material,
        Strain,
        Normal
    }

    public Type displayType;
    public Gradient gradient;

    // render mesh
    private MeshRenderer meshRenderer;
    private MeshFilter meshfilter;
    private Mesh mesh;

    private List<Color> colors;

    // export obj
    private StreamWriter file;
    private string path = "Assets/Resources/Obj/";

    private void Awake()
    {
        meshfilter = new MeshFilter();
        meshfilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = material;
        mesh = new Mesh();
    }
    void Start()
    {
       
    }

    public void CreateMesh(List<Vector3> verts, List<int> triangles)
    {
        colors = new List<Color>(verts.Count);

        mesh.SetVertices(verts);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        meshfilter.mesh = mesh;
    }

    public void UpdateMesh(List<Vector3> verts, List<float> forces)
    {
        if (displayType == Type.Normal)
        {
            NormalVisulization();
        }
        else if(displayType == Type.Strain)
        {
            ForceVisulization(forces);
        }

        mesh.SetVertices(verts);
        mesh.RecalculateNormals();
        meshfilter.mesh = mesh;        
    }

    private void ForceVisulization(List<float> forces)
    {
        colors.Clear();
        foreach(float f in forces){
        
            colors.Add(gradient.Evaluate(f));
        }
        mesh.SetColors(colors);
        material.shader = vertShader;
    }

    private void NormalVisulization()
    {
        material.shader = normalShader;           
        
    }

    public void Export(string filename)
    {
        string[] tmp = filename.Split('/');
        filename = path + tmp[tmp.Length - 1].Split('.')[0] + ".obj";
        file = new StreamWriter(filename);

        Debug.Log("export OBJ: " + filename);

        // material file
        file.WriteLine("mtllib model.mtl");

        // vertices
        foreach (Vector3 v in mesh.vertices)
        {
            file.WriteLine("v " + v.x.ToString() + " " + v.y.ToString() + " " + v.z.ToString());
        }

        // faces
        for (int i = 0; i < mesh.vertexCount; i++)
        {
            file.WriteLine("f " + mesh.triangles[i * 3].ToString() + " " + mesh.triangles[i * 3 + 1].ToString() + " " + mesh.triangles[i * 3 + 2].ToString());
        }

        file.Close();

        Debug.Log("export OBJ OVER! ");
    }

}