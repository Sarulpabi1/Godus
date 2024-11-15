using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public int currentResources;
    public static ResourceManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    private void Start()
    {
        currentResources = 100;
    }
    private void Update()
    {
        if (currentResources < 0)
            currentResources = 0;
    }
}

