using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public List<BuildingData> availableBuilds;

    private BuildingData selectedBuild;
    public BuildingGrid buildingGrid;

    private void Awake()
    {
        int resolution = buildingGrid.gridResolution;
        float cellSize = buildingGrid.cellSize;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        GetBuildSelected();
    }

    public bool PlaceBuild(Vector3 position)
    {
        
        BuildingData buildingData = GetBuildSelected();

        if (buildingData == null)
        {
            return false;
        }

        if (buildingGrid != null)
        {
            if (buildingGrid.CanPlaceBuilding(position, buildingData))
            {
                GameObject newBuildingObject = Instantiate(buildingData.buildingModel, position, Quaternion.identity);

                Building buildingComponent = newBuildingObject.AddComponent<Building>();
                buildingComponent.InitializeBuilding(buildingData);
                return true;
            }
        }
        return false;
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
