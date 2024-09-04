using System;
using System.Collections;
using System.Collections.Generic;
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
    }

    public void DrawLine()
    {
        navMeshAgent.SetDestination(PointLine());
        lineRenderer.positionCount = navMeshAgent.path.corners.Length;
        lineRenderer.SetPosition(0, transform.position);

        for(int i =1; i<navMeshAgent.path.corners.Length; i++)
        {
            var pointLine = new Vector3(navMeshAgent.path.corners[i].x, navMeshAgent.path.corners[i].y, navMeshAgent.path.corners[i].z);
            lineRenderer.SetPosition(i, pointLine);
        }
    }

    public Vector3 PointLine()
    {
        Vector3 toPath = new Vector3();

        for(int i =0; i < locations.Count; i++)
        {
            if (locations[i].complate)
            {
                int index = 0;

                for (int j = 0; j < locations[0].flachCards.Count; j++)
                {
                    var calculateDistance = Vector3.Distance(locations[i].flachCards[j].transform.position, transform.position);
                    var distance = float.MaxValue;

                    if (calculateDistance < distance) index = j;
                }

                toPath = locations[i].flachCards[index].transform.position;
            }
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