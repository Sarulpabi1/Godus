using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private enum GAMESTATE
    {
        Menu,
        Pause,
        InGame
    }

    private GameState instance;
    private void Awake()
    {
        instance = this;
    }
}
