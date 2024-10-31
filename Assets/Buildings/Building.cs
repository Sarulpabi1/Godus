using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public string buildingName;
    public int maxResources;
    public int currentResources;
    public RessourceData ressourceType;
    public float productionInterval;
    public int resourcePerInterval;

    public void InitializeBuilding(BuildingData data)
    {
        buildingName = data.buildName;
        maxResources = data.maxCapacity;
        currentResources = 0;
        resourcePerInterval = data.ressourceAmountPerInterval;
        productionInterval = data.productionInterval;
        ressourceType = data.ressourceData;

        StartCoroutine(ProduceResourcesOverTime());
    }
    private IEnumerator ProduceResourcesOverTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            if (currentResources < maxResources)
            {
                int producedAmount = Mathf.Min(resourcePerInterval, maxResources - currentResources);
                currentResources += producedAmount;
            }
        }
    }

    public int CollectResources()
    {
        int collected = currentResources;
        currentResources = 0;
        return collected;
    }
}

