using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding : MonoBehaviour
{
    public float detectionRadius = 15f;
    public float moveRadius = 10f; 
    public float waitTime = 2f; 

    private NavMeshAgent agent;
    private Vector3 initialPosition;
    private Building targetBuilding;
    private ResourceManager resourceManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        resourceManager = ResourceManager.instance;
        initialPosition = transform.position;
        StartCoroutine(Patrol());
    }

    IEnumerator Patrol()
    {
        while (true)
        {

            targetBuilding = FindNearestBuildingFullOfRessources();

            if (targetBuilding != null) 
            {
                agent.SetDestination(targetBuilding.transform.position);

                while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null;
                }

                int collectedResources = targetBuilding.CollectResources();
                resourceManager.currentResources += collectedResources;
                collectedResources = 0;
            }
            else 
            {
                Vector3 randomPosition = initialPosition + Random.insideUnitSphere * moveRadius;
                randomPosition.y = initialPosition.y;

                if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, moveRadius, NavMesh.AllAreas))
                    agent.SetDestination(hit.position);

                while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
                {
                    yield return null;
                }
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    Building FindNearestBuildingFullOfRessources()
    {
        Building[] buildings = FindObjectsOfType<Building>();
        Building nearestBuilding = null;
        float shortestDistance = detectionRadius;

        foreach (Building building in buildings)
        {
            if (building.currentResources >= building.maxResources)
            {
                float distance = Vector3.Distance(transform.position, building.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestBuilding = building;
                }
            }
        }

        return nearestBuilding;
    }
}
