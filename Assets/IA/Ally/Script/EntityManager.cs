using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public EntityParent entityParent;
    public static EntityManager instance;
    public List<GameObject> Units;

    private void Awake()
    {
        if (instance == null) 
            instance = this;
    }
    
    public void VillagerInstatiation(Vector3 position)
    {

        GameObject newVillager = Instantiate(entityParent.Model, position, Quaternion.identity);
        Units.Add(newVillager);
    }

}
