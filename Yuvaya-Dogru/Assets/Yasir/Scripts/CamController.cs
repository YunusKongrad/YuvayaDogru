using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    public float speed, sensitivity,posSpeed;
    public Transform target;
    public Vector3 velocity,vel2,downPos,upPos,simplePos,lookinPos;
    public InputActionReference ýAction;
    GameControls _controls;
    public Char_Controller char_control;
    Vector2 _mouseDelta;
    bool isLookingUp, isLookingDow,isClimbing;
    private float _xRotation = 0f;

    private void Awake() => _controls = new GameControls();
   
    void Update()
    {
      
        _mouseDelta = ýAction.action.ReadValue<Vector2>();
       
    }
    float mouseX2;
    private void LateUpdate()
    {

        float mouseX = _mouseDelta.x * sensitivity;
        float mouseY = _mouseDelta.y * sensitivity;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -25, 35f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        float t = Mathf.InverseLerp(-45f, 45f, _xRotation);
        if (isLookingUp)
        {
            // Vector3.Lerp, t oranýna göre iki pozisyon arasýnda gidip gelir.
            lookinPos = Vector3.Lerp(upPos, downPos, t);

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, lookinPos, ref vel2, posSpeed*1.2f);
        }
        else
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, simplePos, ref vel2, posSpeed);
        }

        mouseX2 = _xRotation;
        target.Rotate(Vector3.up * mouseX);

        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, target.position,
            ref velocity, speed);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
   

    private void OnEnable()
    {
        _controls.Enable();
        ýAction.action.Enable();
        _controls.Player.LookUp.performed += LookUp_performed;
        _controls.Player.LookUp.canceled += LookUp_canceled;
        
    }

   

    private void LookUp_canceled(InputAction.CallbackContext obj)
    {
        isLookingUp = false;
    }

    private void LookUp_performed(InputAction.CallbackContext obj)
    {
        isLookingUp = true;
    }

    private void OnDisable()
    {
        ýAction.action.Disable();
    }
}
