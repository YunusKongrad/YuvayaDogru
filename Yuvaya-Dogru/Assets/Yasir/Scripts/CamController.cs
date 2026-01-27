using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamController : MonoBehaviour
{
    public float speed, sensitivity, posSpeed;
    public LayerMask collisionLayers;
    public Transform target, camTarget, dummy;
    public Vector3 velocity, vel2, downPos, upPos, simplePos, lookinPos, maxSimplePos;
    public InputActionReference iAction;
    GameControls _controls;
    public Char_Controller char_control;
    Vector2 _mouseDelta;
    public bool isLookingUp, isClimbing, isBlocked, isCollision;
    private float _xRotation = 0f;
    public float maxCameraDistance, sphereRadius;
    public float wallOffset = 0.2f;
    public UiOyunİciManager oyunİciUiManager;

    private void Awake() => _controls = new GameControls();
    private void Start()
    {
        GameObject obj = GameObject.Find("UiOyunIciManager");
        oyunİciUiManager = obj.GetComponent<UiOyunİciManager>();
    }

    void Update()
    {
        if (oyunİciUiManager.pauseAktif == false)
        {
            _mouseDelta = iAction.action.ReadValue<Vector2>();
        }
        else if (oyunİciUiManager.pauseAktif == true)
        {

        }
    }
    private void OnDrawGizmos()
    {
        if (camTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(camTarget.position, transform.position);
            Gizmos.DrawWireSphere(camTarget.position, sphereRadius);
        }
    }


    private void LateUpdate()
    {
        if (oyunİciUiManager.pauseAktif == false)
        {

            Vector3 origin = camTarget.position;
            Vector3 direction = (transform.position - camTarget.position).normalized;

          
            float targetDistance = maxCameraDistance;

            RaycastHit hit;

          
            if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxCameraDistance, collisionLayers))
            {
                targetDistance = hit.distance - wallOffset;
                isBlocked = true;


            }
            else
            {
                isBlocked = false;
            }

            // Güvenlik: Mesafe asla 0.1'den küçük olmasın (karakterin içine girmesin)
            float safeZ = -(targetDistance - 0.2f);

            // Güvenlik: Asla 0'a çok yaklaşmasın (Karakterin içine girmesin)
            if (safeZ > -0.5f) safeZ = -0.5f;

            Vector3 targetLocalPos = new Vector3(transform.localPosition.x, transform.localPosition.y, safeZ);

            // Lerp ile git
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, Time.deltaTime * posSpeed * 15);

            float mouseX = _mouseDelta.x * sensitivity;
            float mouseY = _mouseDelta.y * sensitivity;
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -25, 35f);
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            float t = Mathf.InverseLerp(-45f, 45f, _xRotation);

            if (!isBlocked)
            {
                if (isLookingUp || isClimbing)
                {
                    // Vector3.Lerp, t oran�na g�re iki pozisyon aras�nda gidip gelir.

                    Vector3 targetPos = Vector3.Lerp(upPos, downPos, t);
                    lookinPos = Vector3.Lerp(lookinPos, targetPos, Time.deltaTime * posSpeed);
                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                        lookinPos, ref vel2, posSpeed);
                }
                else
                {

                    lookinPos = Vector3.Lerp(lookinPos, simplePos, Time.deltaTime * posSpeed);

                    transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                        lookinPos, ref vel2, posSpeed);
                }
            }


            dummy.Rotate(Vector3.up * mouseX);
            if (!isClimbing)
            {

                target.rotation = dummy.rotation;

                lookinPos = Vector3.Lerp(lookinPos, simplePos, Time.deltaTime * posSpeed * 15);

                transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                    lookinPos, ref vel2, posSpeed);

            }



            transform.parent.position = Vector3.SmoothDamp(transform.parent.position, target.position,
                ref velocity, speed);
            transform.parent.Rotate(Vector3.up * mouseX);
        }
        else if(oyunİciUiManager.pauseAktif == true)
        {

        }
    }


    private void OnEnable()
    {
        _controls.Enable();
        iAction.action.Enable();
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
        iAction.action.Disable();
    }
}
