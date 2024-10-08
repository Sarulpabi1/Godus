using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceParent : MonoBehaviour
{
    public enum RessourceType
    {
        Wood,
        Stone,
        Faith,
        Food
    }

    public RessourceParent instance;
    [SerializeField] private int amount;
    [SerializeField] private int amountMax;
    private void Awake()
    {
        instance = this;
    }
}
