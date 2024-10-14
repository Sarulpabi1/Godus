using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public static ControlMap ControlMap;
    public static InputAction InputAction;

    public bool InputEnabled = true;

    public Vector2 MoveValue;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else DestroyImmediate(gameObject);
    }

}
