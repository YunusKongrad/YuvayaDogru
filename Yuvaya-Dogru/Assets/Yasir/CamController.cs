using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    public float speed, sensitivity,zAxismin, zAxisMax, yAxismin, yAxisMax;
    public Transform target;
    public Vector3 offSet,velocity,vel2;
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
       
    }
    float mouseX2;
    private void LateUpdate()
    {

        float mouseX = _mouseDelta.x * sensitivity;
        float mouseY = _mouseDelta.y * sensitivity;
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -45, 45f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        Debug.Log(mouseY>0);
        if(_xRotation<mouseX2)
        {
            //YUKARI
            Debug.Log("yukarý");
            if (zAxisMax > offSet.z)
            {
                offSet.z += Time.deltaTime *2* sensitivity;
            }
           
            if (yAxismin < offSet.y)
            {
                offSet.y -= Time.deltaTime *2* sensitivity;

            }
            

        }
        if(_xRotation > mouseX2)
        {
            //AÞAÐI
            Debug.Log("aþaðý");
            if (zAxismin < offSet.z)
            {
                offSet.z -= Time.deltaTime*2* sensitivity;

            }
            
            if (yAxisMax > offSet.y)
            {
                offSet.y += Time.deltaTime*2 * sensitivity;
            }
           
        }
        
        mouseX2 = _xRotation;
        target.Rotate(Vector3.up * mouseX);
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition,  offSet,
           ref vel2, speed);
        transform.parent.localPosition = Vector3.SmoothDamp(transform.parent.localPosition, target.position,
            ref velocity, speed);
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
