using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildManager : MonoBehaviour
{    
    public static BuildManager instance;

    public List<BuildingData> availableBuilds;

    public BuildingData selectedBuild;
    private List<BuildingData> activeBuilds = new List<BuildingData>();
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

        GetBuildSelected();
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

    public void BuildSelection(int index)
    {
        selectedBuild = availableBuilds[index];
    }

    private BuildingData GetBuildSelected()
    {
        if (selectedBuild == null && availableBuilds.Count > 0)
        {
            selectedBuild = availableBuilds[0];
        }

        return selectedBuild;
    }
}
