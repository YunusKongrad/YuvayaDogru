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

    private void Awake() => _controls = new GameControls();
    private void Start()
    {


    }

    void Update()
    {

        _mouseDelta = iAction.action.ReadValue<Vector2>();

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
        Vector3 origin = camTarget.position;
        Vector3 direction = (transform.position - camTarget.position).normalized;

        // Hedef mesafemiz varsayılan olarak maksimumdur
        float targetDistance = maxCameraDistance;

        RaycastHit hit;

        // 2. KONTROL: Karakterden kameraya doğru bir küre fırlatıyoruz
        // Önemli: direction kullanıyoruz, position değil!
        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxCameraDistance, collisionLayers))
        {
            // Çarpışma varsa mesafeyi, çarpışma noktasına göre ayarla
            // Hit distance bize ne kadar uzakta çarptığını verir
            targetDistance = hit.distance - wallOffset;
            isBlocked = true;

        }
        else
        {
            isBlocked = false;
        }
        float safeZ = -(targetDistance - 0.2f); // Eksi işareti koyduk çünkü kamera arkada!

        // Güvenlik: Asla 0'a çok yaklaşmasın (Karakterin içine girmesin)
        if (safeZ > -0.5f) safeZ = -0.5f;

        Vector3 targetLocalPos = new Vector3(transform.localPosition.x, transform.localPosition.y, safeZ);

        // Lerp ile git
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, Time.deltaTime * posSpeed*15);

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
                lookinPos = Vector3.Lerp(lookinPos, targetPos, Time.deltaTime * posSpeed*15);
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                    lookinPos, ref vel2, posSpeed);
            }
            else
            {

                lookinPos = Vector3.Lerp(lookinPos, simplePos, Time.deltaTime * posSpeed * 15);

                transform.localPosition = Vector3.SmoothDamp(transform.localPosition,
                    lookinPos, ref vel2, posSpeed);
            }
        }


        dummy.Rotate(Vector3.up * mouseX);
        if (!isClimbing)
        {

            target.rotation = dummy.rotation;
        }
        else
        {

        }



        transform.parent.position = Vector3.SmoothDamp(transform.parent.position, target.position,
            ref velocity, speed);
        transform.parent.Rotate(Vector3.up * mouseX);
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
