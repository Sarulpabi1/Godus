using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    BuildingData selectedBuild;
    private void OnEnable()
    {
        InputManager.ControlMap.Player.PlaceBuilding.performed += BuildInstatiation;
    }

    private void OnDisable()
    {
        InputManager.ControlMap.Player.PlaceBuilding.performed -= BuildInstatiation;
    }

    private void BuildInstatiation(InputAction.CallbackContext context)
    {
        BuildManager.instance.PlaceBuild(selectedBuild);
    }

    

    
}
