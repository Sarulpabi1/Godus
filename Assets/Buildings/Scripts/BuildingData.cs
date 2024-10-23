using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "EDtF/Building Data")]
public class BuildingData : ScriptableObject
{
    public string buildName;
    public float width;
    public float height;
    public int level;
    public GameObject buildingModel;
    public RessourceData ressourceData;

    [Header ("   Ressource Generation")]
    public float ressourceGenerationInterval = 5f;
    public int ressourceAmountPerInterval = 1;
    public int maxCapacity = 100;
    public int currentStoredAmount = 0;
}
