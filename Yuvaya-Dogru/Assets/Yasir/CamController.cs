using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    public float speed, sensitivity;
    public Transform target;
    public Vector3 offSet;
    public InputActionReference ýAction;
    public Char_Controller char_control;
    Vector2 _mouseDelta;

    private float _xRotation = 0f;
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

        _mouseDelta = ýAction.action.ReadValue<Vector2>();
        float mouseX = _mouseDelta.x * sensitivity;
        float mouseY = _mouseDelta.y * sensitivity;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        //transform.position = Vector3.Lerp(transform.position, target.position - offSet, speed * Time.deltaTime);
        //target.rotation = new Quaternion(target.rotation.x, mouseY*sensitivity, target.rotation.z, target.rotation.w);
        target.Rotate(Vector3.up * mouseX);
        transform.parent.Rotate(Vector3.up * mouseX);
    }
    private void FixedUpdate()
    {

    }

    private void OnEnable()
    {
        ýAction.action.Enable();
    }

    private void OnDisable()
    {
        ýAction.action.Disable();
    }
}
