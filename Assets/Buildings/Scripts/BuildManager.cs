using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public List<BuildingData> availableBuilds;

    private BuildingData selectedBuild;
    public ResourceManager resourceManager;
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
        Vector2Int cellCoords = BuildingGrid.instance.GetGridCoordinates(position);

        BuildingData buildingData = GetBuildSelected();

        if (BuildingGrid.instance.IsCellOccupied(cellCoords))
        {
            print("Cell is occupied");
            return;
        }

        if (resourceManager.currentResources < selectedBuild.amountOfResourcesNeeded)
        {
            print("Not enough resources");
            return;
        }

        Vector3 cellPosition = BuildingGrid.instance.GetWorldPosition(cellCoords);

        GameObject newBuildingObject = Instantiate(buildingData.buildingModel, cellPosition, Quaternion.identity);
        EntityManager.instance.VillagerInstatiation(cellPosition);
        BuildingGrid.instance.PlaceBuildingInCell(cellCoords, buildingData);

        Building buildingComponent = newBuildingObject.AddComponent<Building>();
        buildingComponent.InitializeBuilding(buildingData);
        resourceManager.currentResources -= selectedBuild.amountOfResourcesNeeded;
        
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
