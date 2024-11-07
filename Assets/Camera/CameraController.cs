using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;  

public class CameraMovement : MonoBehaviour
{

    private Transform cameraTransform;


    //Movement (Horizontal)
    [Header("   Horizontal Movement")]
    [SerializeField] private float maxSpeed = 5f;
    private float speed;
    [SerializeField] private float damping = 15f;
    private Vector3 horizontalVelocity;


    //Zoom Variables
    [Header("   Zoom")]
    [SerializeField] private float step = 2f;
    [SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float zoomSpeed = 2f;
    private float zoomHeight;


    //Rotation
    [Header("   Rotation")]
    [SerializeField] private float maxRotationSpeed = 1f;

    //EdgeOfScreen
    [Header("   EdgeOfScreen")]
    [SerializeField, Range(0f, 1f)] private float edgeTolerance;
    [SerializeField] private bool useScreenEdge = true;
    

    private Vector3 targetPosition;
    private Vector3 lastPosition;

    private void Awake()
    {
        InputManager.ControlMap = new ControlMap();
        cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }

    private void OnEnable()
    {
        zoomHeight = cameraTransform.localPosition.y;
        cameraTransform.LookAt(this.transform);

        lastPosition = this.transform.position;
        InputManager.InputAction = InputManager.ControlMap.Camera.Movement;
        InputManager.ControlMap.Camera.Rotation.performed += RotateCamera;
        InputManager.ControlMap.Camera.Zoom.performed += ZoomCamera;
        InputManager.ControlMap.Camera.Enable();
    }

    private void OnDisable()
    {
        InputManager.ControlMap.Camera.Rotation.performed -= RotateCamera;
        InputManager.ControlMap.Camera.Zoom.performed -= ZoomCamera;
        InputManager.ControlMap.Camera.Disable();
    }

    private void Update()
    {
        GetKeyboardMovement();
        if (useScreenEdge)
            CheckMouseAtScreenEdge();
        
        UpdateVelocity();
        UpdateCameraPosition();
        UpdateBasePosition();  
    }
    private void UpdateVelocity()
    {
        horizontalVelocity = (this.transform.position - lastPosition)/Time.deltaTime;
        horizontalVelocity.y = 0;
        lastPosition = this.transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = InputManager.InputAction.ReadValue<Vector2>().x * GetCameraRight() + InputManager.InputAction.ReadValue<Vector2>().y * GetCameraForward();
        inputValue = inputValue.normalized;

        if (inputValue.sqrMagnitude > 0.1f)
            targetPosition += inputValue;
    }

    private Vector3 GetCameraForward()
    {
        Vector3 forward = cameraTransform.forward;
        forward.y = 0;
        return forward;
    }

    private Vector3 GetCameraRight()
    {
        Vector3 right = cameraTransform.right;
        right.y = 0;
        return right;
    }

    private void UpdateBasePosition()
    {
        if (targetPosition.sqrMagnitude > 0.1f)
        {
            speed = Mathf.Lerp(speed, maxSpeed, Time.deltaTime);
            transform.position += targetPosition * speed * Time.deltaTime;
        }
        else 
        { 
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }
        targetPosition = Vector3.zero;
    }

    private void RotateCamera(InputAction.CallbackContext inputValue)
    {
        if (!Mouse.current.middleButton.isPressed)
            return;

        float value = inputValue.ReadValue<Vector2>().x;
        transform.rotation = Quaternion.Euler(0f, value * maxRotationSpeed + transform.rotation.eulerAngles.y, 0f);
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(value) > 0.1f)
        {
            zoomHeight = cameraTransform.localPosition.y + value * step;
            if (zoomHeight < minHeight)
                zoomHeight = minHeight;
            else if (zoomHeight > maxHeight)
                zoomHeight = maxHeight;
        }
    }

    private void UpdateCameraPosition()
    {
        Vector3 zoomTarget = new Vector3(cameraTransform.localPosition.x, zoomHeight, cameraTransform.localPosition.z);
        zoomTarget -= zoomSpeed * (zoomHeight - cameraTransform.localPosition.y) * Vector3.forward;
        
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
        cameraTransform.LookAt(this.transform);
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector3 moveDirection = Vector3.zero;

        if (mousePosition.x < edgeTolerance * Screen.width)
            moveDirection += -GetCameraRight();
        else if (mousePosition.x > (1f - edgeTolerance) * Screen.width)
            moveDirection += GetCameraRight();
        
        if (mousePosition.y < edgeTolerance * Screen.height)
            moveDirection += -GetCameraForward();
        else if (mousePosition.y > (1f - edgeTolerance) * Screen.height)
            moveDirection += GetCameraForward();

        targetPosition += moveDirection;
    }
}
