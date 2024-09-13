using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Spark.Gameplay.Items.Pickupable;
using UnityEngine;
using UnityEngine.AI;


public class LinePath : MonoBehaviour
{
    [SerializeField] private List<Locations> locations = new List<Locations>();
    private LineRenderer lineRenderer;
    private NavMeshAgent navMeshAgent;


    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        lineRenderer.positionCount = 0;
    }

    public void DrawLine()
    {

        //navMeshAgent.SetDestination(PointLine());



        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, PointLine(), navMeshAgent.areaMask, path); //Saves the path in the path variable.
        Vector3[] corners = path.corners;
        lineRenderer.positionCount = corners.Length;
        lineRenderer.SetPositions(corners);



        //Vector3[] positions = new Vector3[navMeshAgent.path.corners.Length];

        //for (int i = 0; i < navMeshAgent.path.corners.Length; i++)
        //{

        //    if (i == 0)
        //    {
        //        positions[i] = transform.position;
        //        continue;
        //    }

        //    positions[i] = new Vector3(navMeshAgent.path.corners[i].x, navMeshAgent.path.corners[i].y, navMeshAgent.path.corners[i].z);



        //}
        //lineRenderer.SetPositions(positions);

        //if (navMeshAgent.path.corners.Length < 2)
        //    return;



    }

    public Vector3 PointLine()
    {
        Vector3 toPath = new Vector3();
        int index = 0;

        for(int i =0; i < locations.Count; i++)
        {
            if (locations[i].complate)
            {
                var distance = float.MaxValue;

                for (int j = 0; j < locations[i].flachCards.Count; j++)
                {
                    if (locations[i].flachCards[j] == null) continue;

                    var calculateDistance = Vector3.Distance(locations[i].flachCards[j].transform.position, transform.position);

                    if (calculateDistance < distance)
                    {
                        index = j;
                        distance = calculateDistance;
                        toPath = locations[i].flachCards[index].transform.position;
                    }
                }

            }

            if (toPath == Vector3.zero)
                toPath = locations[i].exit.position;

        }

        return toPath;
    }
}

[Serializable]
public class Locations
{
    public List<FlashCardItemPickup> flachCards = new List<FlashCardItemPickup>();
    public Transform exit;
    public bool complate;
}