﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    public string filename = "Assets/Resources/FOLD/flappingBird.fold";
    public List<Node> nodes = new List<Node>();
    public List<Beam> beams = new List<Beam>();
    public List<Face> faces = new List<Face>();

    private FOLD fold;
    private Debuger debuger = new Debuger();

    // Start is called before the first frame update
     public void Parse()
    {
        // Step 1: .fold -> FOLD
        fold = new FOLD();
        fold.Parse(filename);

        // Step 2: FOLD -> Node/Beam/Face

        // parse Nodes
        List<Vector3> verts = fold.vertices_coords;
        for (int i = 0; i < verts.Count; i++)
        {
            Vector3 coord = verts[i];
            Node node = new Node();
            node.SetIndex(i);
            node.SetPosition(coord);

            nodes.Add(node);
        }

        // parse Beams
        List<Vector2Int> edges_verts = fold.edges_verts;
        List<string> edges_types = fold.edges_types;
        List<float> edges_foldAngles = fold.edges_foldAngles;
        Dictionary<string, int> dics = fold.edges_neighbor_verts;
        for(int i = 0; i < edges_verts.Count; i++)
        {
            Beam beam = new Beam();
            beam.SetIndex(i);
            


            int n1 = edges_verts[i].x;
            int n2 = edges_verts[i].y;
            beam.SetNode1(nodes[n1]);
            beam.SetNode2(nodes[n2]);
            // for visualization
            //beam.SetL(Vector3.Distance(nodes[n1].position, nodes[n2].position) * 1.1f);
            beam.SetL(Vector3.Distance(nodes[n1].position, nodes[n2].position));
            beam.SetL_0(beam.l);
            // set type should be after set l
            // because in set type, we will use l
            beam.SetType(edges_types[i]);
            beam.SetThetaTarget(edges_foldAngles[i]);
            beam.SetTheta(0);

            nodes[n1].AddBeam(beam);
            nodes[n2].AddBeam(beam);

            // face和face交界处的beam的同面相邻点
            if (beam.type == Beam.Type.Mountain || beam.type == Beam.Type.Valley)
            {
                int p1 = dics[n1.ToString() + "," + n2.ToString()];
                int p2 = dics[n2.ToString() + "," + n1.ToString()];
                
                beam.neigh_p1 = nodes[p1];
                beam.neigh_p2 = nodes[p2];

                // Debug.Log("beam.p1:" + beam.neigh_p1.position.ToString());
                // Debug.Log("beam.p2:" + beam.neigh_p2.position.ToString());

            }         

            beams.Add(beam);
        }

        // parse Faces
        List<Vector3Int> faces_verts = fold.faces_verts;
        for(int i = 0; i < faces_verts.Count; i++)
        {
            Face face = new Face();
            face.SetIndex(i);
            Vector3Int ids = faces_verts[i];
            face.AddNode(nodes[ids.x]);
            face.AddNode(nodes[ids.y]);
            face.AddNode(nodes[ids.z]);

            face.SetAlpha_0();

            faces.Add(face);
        }

        Debug.Log("parse Success!");
    }

    public void Print()
    {
        debuger.PrintNodes(nodes);
        debuger.PrintBeams(beams);
        debuger.PrintFaces(faces);
    }

}
