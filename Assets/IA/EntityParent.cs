using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityParent : MonoBehaviour
{
    [SerializeField] private float speed;
    public EntityParent instance;

    public enum IAType
    {
        Ally,
        Enemy,
        Neutral
    }
    private void Awake()
    {
        instance = this;
    }
}
