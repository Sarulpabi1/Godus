using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingParent : MonoBehaviour
{
    public BuildingParent instance;
    [SerializeField] private float height;
    [SerializeField] private float width;
    private bool isFull;

    public enum BuildingType
    {
        House,
        Sawmill,
        Mine
    }

    private void Awake()
    {
        instance = this;
    }
}
