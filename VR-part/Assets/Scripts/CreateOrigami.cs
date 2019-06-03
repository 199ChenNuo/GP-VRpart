using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrigami : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject origami;
    public GameObject line;
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        origami = (GameObject)Resources.Load("Prefabs/Plane");
        GameObject parentOrigami = GameObject.Find("Origami");
        origami = Instantiate(origami);
        origami.transform.parent = parentOrigami.transform;

        line = GameObject.Find("LineRender");
        lineRenderer = line.GetComponent<LineRenderer>();
        // lineRenderer.material = new Material(Shader.Find("Materials/LineRendererMaterial"));
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, new Vector3(-5, 0, 5));
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, transform.position);
        List<Vector3> positions = new List<Vector3>();
        
        Vector3 p1 = new Vector3(-5, 0, 5);
        Vector3 p2 = new Vector3(0, 0, 5);
        Vector3 p3 = new Vector3(5, 0, 5);
        Vector3 p4 = new Vector3(-5, 0, 0);
        Vector3 p5 = new Vector3(0, 0, 0);
        Vector3 p6 = new Vector3(5, 0, 0);
        Vector3 p7 = new Vector3(-5, 0, -5);
        Vector3 p8 = new Vector3(-5, 0, -5);
        Vector3 p9 = new Vector3(-5, 0, -5);
        positions.Add(p1);
        positions.Add(p2);
        positions.Add(p3);
        positions.Add(p4);
        positions.Add(p5);
        positions.Add(p6);
        positions.Add(p7);
        positions.Add(p8);
        positions.Add(p9);
        Vector3 mid = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height/2, 0));
        double d1 = Distence(mid, p1);
        double d2 = Distence(mid, p2);
        double d3 = Distence(mid, p3);
        double d4 = Distence(mid, p4);
        double d5 = Distence(mid, p5);
        double d6 = Distence(mid, p6);
        double d7 = Distence(mid, p7);
        double d8 = Distence(mid, p8);
        double d9 = Distence(mid, p9);
        double min = 10000;
        Vector3 pos2 = positions[0];
        for(int i=0; i<9; i++)
        {
            if(Distence(positions[i], mid) < min)
            {
                min = Distence(positions[i], mid);
                pos2 = positions[i];
            }
        }
        lineRenderer.SetPosition(1, pos2);
    }

    float Distence(Vector3 p1, Vector3 p2)
    {
        return (p1 - p2).sqrMagnitude;
    }

}
