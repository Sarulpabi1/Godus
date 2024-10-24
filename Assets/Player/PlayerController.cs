using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;

    private void Awake()
    {
        InputManager.ControlMap = new ControlMap();
    }
    private void OnEnable()
    {
        InputManager.ControlMap.Player.PlaceBuilding.started += BuildInstatiation;
        InputManager.ControlMap.Player.Enable();
    }

    private void OnDisable()
    {
        InputManager.ControlMap.Player.PlaceBuilding.performed -= BuildInstatiation;
        InputManager.ControlMap.Player.Disable();
    }

    private void BuildInstatiation(InputAction.CallbackContext context)
    {
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.LogWarning("No building selected.");
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            BuildManager.instance.PlaceBuild(hit.point);
            Debug.Log("oooooooooo");
        }
    }
}
