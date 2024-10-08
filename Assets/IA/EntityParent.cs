using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityParent : MonoBehaviour
{
    [SerializeField] private float speed;
    private EntityParent instance;

    private void Awake()
    {
        instance = this;
    }
}
