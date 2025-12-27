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
    bool isLookingUp, isLookingDown;
    private float _xRotation = 0f;

    private void Awake()
    {
        _controls = new GameControls();
    }
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
        _xRotation = Mathf.Clamp(_xRotation, -45, 45f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        float value = Time.deltaTime * 7 * sensitivity;
        if (_xRotation<mouseX2)
        {
            //YUKARI
            if (lookinPos.z<upPos.z)
            {
                lookinPos.z +=value ;
               
            }
            if (lookinPos.y>upPos.y)
            {
                lookinPos.y -= value;
            }
           


        }
        if (_xRotation > mouseX2)
        {
            //AÞAÐI
            if (lookinPos.z>downPos.z)
            {
                lookinPos.z -= value;
            }
            if (lookinPos.y < downPos.y)
            {
                lookinPos.y += value;
            }
            
        }
        if (isLookingUp)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, lookinPos,
         ref vel2, posSpeed);
        }
        else
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, simplePos,
         ref vel2, posSpeed);
        }

            mouseX2 = _xRotation;
        target.Rotate(Vector3.up * mouseX);
       
        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, target.position,
            ref velocity, speed);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
    private void FixedUpdate()
    {

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
