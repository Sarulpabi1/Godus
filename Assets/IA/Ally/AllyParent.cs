using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyParent : MonoBehaviour
{
    public enum AllyType
    {
        Believer,
        Lumberjack,
        Miner,
        Hunter
    }

    public AllyParent instance;

    private void Awake()
    {
        instance = this;
    }
}
