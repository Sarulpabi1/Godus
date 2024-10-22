using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildManager : MonoBehaviour
{
    public BuildingData buildingData;
    public BuildManager instance;
    public List<BuildingData> activeBuilds = new List<BuildingData>();

    private Dictionary<RessourceData, int> globalRessourceStock = new Dictionary<RessourceData, int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
            Destroy(gameObject);
            
    }

    public void PlaceBuild(BuildingData build)
    {
        Instantiate(build);
        activeBuilds.Add(build);

        if (!globalRessourceStock.ContainsKey(build.ressourceData))
        {
            globalRessourceStock.Add(build.ressourceData, 0);
        }

        StartCoroutine(GenerateResources(build));
    }

    private IEnumerator GenerateResources(BuildingData build)
    {
        while (true)
        {
            yield return new WaitForSeconds(build.ressourceGenerationInterval);

            if (build.currentStoredAmount < build.maxCapacity)
            {
                int amountToGenerate = Mathf.Min(build.ressourceAmountPerInterval, build.maxCapacity - build.currentStoredAmount);
                build.currentStoredAmount += amountToGenerate;
            }
        }
    }

    public int CollectResourcesFromBuild(BuildingData build)
    {
        int collectedAmount = build.currentStoredAmount;
        build.currentStoredAmount = 0;

        globalRessourceStock[build.ressourceData] += collectedAmount;

        return collectedAmount;
    }

    public int GetGlobalResourceAmount(RessourceData resourceType)
    {
        if (globalRessourceStock.ContainsKey(resourceType))
        {
            return globalRessourceStock[resourceType];
        }
        return 0;
    }
}
