using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class MeshController : MonoBehaviour
{
    public Material material;
    public float maxForce = 1.0f;
    public Shader vertShader;
    public Shader normalShader;


    private MeshRenderer meshRenderer;
    private MeshFilter meshfilter;
    private Mesh mesh;

    private List<Color> colors;

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

    public void UpdateMesh(List<Vector3> verts)
    {
        mesh.SetVertices(verts);
        mesh.RecalculateNormals();
        meshfilter.mesh = mesh;
        
    }

    public void ForceVisulization(List<float> forces)
    {
        colors.Clear();
        foreach(float f in forces){
            /*if (f < 0.33 * maxForce)
            {
                colors.Add(Color.Lerp(Color.blue, Color.green, f*0.33f));
            }
            else if (f < 0.66 * maxForce)
            {
                colors.Add(Color.Lerp(Color.green, Color.yellow, f*0.66f));
            }*/

            colors.Add(Color.Lerp(Color.yellow, Color.red, f));
            

        }
        mesh.SetColors(colors);
        material.shader = vertShader;
    }

    public void NormalVisulization()
    {
        material.shader = normalShader;           
        
    }


}