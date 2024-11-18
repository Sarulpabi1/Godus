using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "EDtF/Building Data")]
public class BuildingData : ScriptableObject
{
    public string buildName;
    public int level;
    public GameObject buildingModel;
    public RessourceData ressourceData;
    public int amountOfResourcesNeeded;

    [Header("   Ressource Generation")]
    public float productionInterval = 2;
    public int ressourceAmountPerInterval = 1;
    public int maxCapacity = 100;
    public int currentStoredAmount = 0;
}
