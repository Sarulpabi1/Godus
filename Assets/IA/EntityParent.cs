using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityData", menuName = "EDtF/Entity Data")]
public class EntityParent : ScriptableObject
{
    public string entityName;
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
