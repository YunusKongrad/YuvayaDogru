using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngineInternal;
public class Char_Controller : MonoBehaviour
{
    private CharTirmanma charTirmanmaCS;   
    GameControls _controls;
     Vector2 _moveInput;
    CharacterController cc;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform _cameraTransform;
    [SerializeField] GameObject _pushingObj;
    Vector3 _pushingObjPos;
    [Header("Karakter özellikleri")]
    [SerializeField] float moveSpeed;
    [SerializeField] float speed,jumpForce, RunSpeed,pushingSpeed;

    [Header("Kalýcý deðerler")]
    public Vector3 movement;
    [SerializeField] float gravity, velocityY, moveMulti, rayDistance, gravityLimit,stamina,maxStamina,staminaFactor;
    [Header("Kontroller")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool canJump,wasGrounded,isWaitingFall,jumpPressed,sopungJumped,isRunning,isClimbing,canClimb;
    [Header("stamina")]
    [SerializeField] bool staminaAnim, startStaminanim;
    [SerializeField] Image staminaBar, staminaBar2;
    [SerializeField] float staminaAlpha;

    private void Awake()
    {
        // 2. Nesneyi hafýzada oluþturuyoruz
        _controls = new GameControls();
     
        cc = GetComponent<CharacterController>();
        charTirmanmaCS = GetComponent<CharTirmanma>();

    }

    private void OnEnable()
    {
        // 3. Kontrolleri aktif ediyoruz (Bu olmazsa tuþlar çalýþmaz!)
        _controls.Enable();
        _controls.Player.Jump.performed += DoJump;
        _controls.Player.Run.performed += Run_performed;
        _controls.Player.Run.canceled += Run_canceled;
        //_controls.Player.Climb.performed += Climb_performed;
        //_controls.Player.Climb.canceled += Climb_canceled;
    }

    //private void Climb_canceled(InputAction.CallbackContext obj)=> canClimb = false;
   
    //private void Climb_performed(InputAction.CallbackContext obj) => canClimb = false;
   

    private void Run_canceled(InputAction.CallbackContext obj)
    {
        startStaminanim = false;
        isRunning = false;
        staminaAnim = false;
        speed -= RunSpeed;
    }

    private void Run_performed(InputAction.CallbackContext obj)
    {
        startStaminanim = true;
        isRunning = true;
        staminaAnim = true;
        speed += RunSpeed;
    }
    Vector3 forward;
    Vector3 right;
    private void Update()
    {
        if (charTirmanmaCS.tirmanmaAktif == false)
        {
            _moveInput = _controls.Player.Move.ReadValue<Vector2>();
            if (_moveInput != Vector2.zero && isRunning)
            {
                stamina -= Time.deltaTime * staminaFactor;
                Debug.Log("azalýyor");
            }
            forward = _cameraTransform.forward;
            right = _cameraTransform.right;
            MoveCharacter();
            staminaBar.fillAmount = stamina / maxStamina;
            /*if (canClimb)
            {
                RaycastHit ray;
                if (Physics.Raycast(transform.position, forward, out ray, 3))
                {

                    if (ray.collider.CompareTag("rope"))
                    {
                        isClimbing = true;
                    }
                }                      
            }*/

            #region zýplma
            bool isGrounded2 = cc.isGrounded;
            if (isGrounded2)
            {
                sopungJumped = false;
            }
            if (isGrounded2 && !wasGrounded && canJump)
            {
                OnLanded();
            }
            #endregion
        }
        else if(charTirmanmaCS.tirmanmaAktif == true)
        {

        }
    }


    private void OnLanded()
    {
        if (jumpPressed)
        {
            speed /= moveMulti;
            jumpPressed = false;
        }
        isGrounded = true;
        sopungJumped = false;
    }



    void SopungJump()
    {
        velocityY += jumpForce*2;
    }
   
    private void DoJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            speed *= moveMulti;
            velocityY = jumpForce;          
            isGrounded = false;         
            jumpPressed = true;
            canJump = false;
        }
    }

    private void MoveCharacter()
    {

        right = _cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        /*if (isClimbing)
        {
            movement.y += _moveInput.y;
        }
        else
        {
            if (!isGrounded)
            {
                if (velocityY > gravityLimit)
                {
                    velocityY -= gravity * Time.deltaTime;
                }
            }
            movement = (forward * _moveInput.y) + (right * _moveInput.x);
            movement.y += velocityY;

          
        }*/
        if (!isGrounded)
        {
            if (velocityY > gravityLimit)
            {
                velocityY -= gravity * Time.deltaTime;
            }
        }
        movement = (forward * _moveInput.y) + (right * _moveInput.x);
        movement.y += velocityY;
        cc.Move(movement * speed * Time.deltaTime);
    }

    private void OnDisable()
    {
        _controls.Disable();
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Crump"))
        {
            stamina += 3;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            Destroy(hit.gameObject);

            StartCoroutine(staminaWait());
        }
        if (hit.collider.CompareTag("Sopung"))
        {
            if (hit.normal.y > 0.9f)
            {
                if (!sopungJumped)
                {
                    SopungJump();
                    sopungJumped = true;
                }
            }


        }
       
        if (hit.collider.gameObject.layer==6 && hit.collider.tag!= "Sopung")
        {
            if (hit.normal.y > 0.9f)
            {
                canJump = true;
            }


        }
        if (hit.collider.CompareTag("Pushable"))
        {
            Vector3 extents = hit.collider.bounds.extents;

            
            float objectRadius = Mathf.Abs(Vector3.Dot(extents, hit.normal));

          
            float dynamicRayDistance = objectRadius + 0.1f;
            RaycastHit ray;
           

            if (Physics.Raycast(hit.transform.position, -hit.normal, out ray, dynamicRayDistance))
            {

                Debug.Log("çarpýyor");

            }
            else
            {
                _isCurrentlyPushing = true;
                speed = pushingSpeed;
                _pushingObjPos = hit.gameObject.transform.position;
                _pushingObjPos -= hit.normal * Time.deltaTime * speed;
                hit.gameObject.transform.position = _pushingObjPos;
            }
            Debug.DrawRay(hit.transform.position, -hit.normal*dynamicRayDistance,Color.red);
        }
        if (hit.collider.CompareTag("rope"))
        {
            
        }
        
    }
    bool _isCurrentlyPushing;
    IEnumerator staminaWait()
    {
        yield return new WaitForSeconds(1);
        if (!staminaAnim)
        {
            startStaminanim = false;

        }
       
    }

    private void LateUpdate()
    {
        if (_isCurrentlyPushing)
        {
            _isCurrentlyPushing = false;
        }
        else
        {
            if (isRunning)
            {
                speed = RunSpeed;
            }
            else
            {
                speed = moveSpeed;
            }
           
        }
    }
}