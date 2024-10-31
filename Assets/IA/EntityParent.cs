using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityParent : ScriptableObject
{
    public float HP;
    public float speed;
    public GameObject Model;
    public enum Team
    {
        Ally,
        Enemy,
        Neutral
    }
}
