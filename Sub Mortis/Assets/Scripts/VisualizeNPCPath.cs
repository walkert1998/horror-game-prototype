using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class VisualizeNPCPath : MonoBehaviour
{
    public NavMeshAgent agent;
    LineRenderer renderer;
    public List<Vector3> points;
    NavMeshPath path;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        DrawPath();
    }

    public void DrawPath ()
    {
        if (agent.path.corners.Length < 2)
            return;

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            renderer.positionCount = agent.path.corners.Length;
            points = agent.path.corners.ToList();
            for (int j = 0; j < points.Count; j++)
            {
                renderer.SetPosition(j, points[j]);
            }
        }
    }
}
