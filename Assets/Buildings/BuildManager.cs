using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public List<BuildingData> availableBuilds;

    private BuildingData selectedBuild;

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

    public void PlaceBuild(Vector3 position)
    {
        BuildingData buildingData = GetBuildSelected();

        GameObject newBuildingObject = Instantiate(buildingData.buildingModel, position, Quaternion.identity);

        Building buildingComponent = newBuildingObject.AddComponent<Building>();
        buildingComponent.InitializeBuilding(buildingData);
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
