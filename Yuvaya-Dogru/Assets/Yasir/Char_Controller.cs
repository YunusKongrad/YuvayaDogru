using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Char_Controller : MonoBehaviour
{
    
    GameControls _controls;
     Vector2 _moveInput;
    CharacterController cc;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform _cameraTransform;
    [Header("Karakter özellikleri")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce,RunSpeed;

    [Header("Kalýcý deðerler")]
    public Vector3 movement;
    [SerializeField] float gravity, velocityY, moveMulti, rayDistance, gravityLimit,stamina,maxStamina,staminaFactor;
    [Header("Kontroller")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool canJump,wasGrounded,isWaitingFall,jumpPressed,sopungJumped,isRunning;
    [Header("stamina")]
    [SerializeField] bool staminaAnim, startStaminanim;
    [SerializeField] Image staminaBar, staminaBar2;
    [SerializeField] float staminaAlpha;

    private void Awake()
    {
        // 2. Nesneyi hafýzada oluþturuyoruz
        _controls = new GameControls();
     
        cc = GetComponent<CharacterController>();
        

    }

    private void OnEnable()
    {
        // 3. Kontrolleri aktif ediyoruz (Bu olmazsa tuþlar çalýþmaz!)
        _controls.Enable();
        _controls.Player.Jump.performed += DoJump;
        _controls.Player.Run.performed += Run_performed;
        _controls.Player.Run.canceled += Run_canceled;

    }

    private void Run_canceled(InputAction.CallbackContext obj)
    {
        startStaminanim = false;
        isRunning = false;
        staminaAnim = false;
    }

    private void Run_performed(InputAction.CallbackContext obj)
    {
        startStaminanim = true;
        isRunning = true;
        staminaAnim = true;
    }

    private void Update()
    {
     
        _moveInput = _controls.Player.Move.ReadValue<Vector2>();
        if (_moveInput!=Vector2.zero && isRunning)
        {
            stamina -= Time.deltaTime * staminaFactor;
            Debug.Log("azalýyor");
        }
        MoveCharacter();
        staminaBar.fillAmount = stamina / maxStamina;
        #region stamina
        if (startStaminanim && staminaAlpha<255)
        {
            staminaAlpha += Time.deltaTime * 160f;
            if (staminaAlpha>255)
            {
                staminaAlpha = 255;
            }
            if (staminaAlpha<=150)
            {
                staminaBar2.color = new Color32(0, 0, 0, (byte)staminaAlpha);
            }
            staminaBar.color = new Color32(0, 50, 250, (byte)staminaAlpha);
            
        }
        else if(!startStaminanim && staminaAlpha >0)
        {
            staminaAlpha -= Time.deltaTime * 150F;
            if (staminaAlpha < 0)
            {
                staminaAlpha = 0;
            }
            staminaBar.color = new Color32(0, 50, 250, (byte)staminaAlpha);
            staminaBar2.color = new Color32(0, 0, 0, (byte)staminaAlpha);
        }

        #endregion


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


    private void OnLanded()
    {
        if (jumpPressed)
        {
            moveSpeed /= moveMulti;
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
            moveSpeed *= moveMulti;
            velocityY = jumpForce;          
            isGrounded = false;         
            jumpPressed = true;
            canJump = false;
        }
    }

    private void MoveCharacter()
    {
        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        if (!isGrounded)
        {
            if (velocityY > gravityLimit)
            {
                velocityY -= gravity * Time.deltaTime;
            }
        }
        movement = (forward * _moveInput.y) + (right * _moveInput.x);
        movement.y += velocityY;
        if (isRunning)
        {
            if (stamina > 0)
            {
                cc.Move(movement * RunSpeed * Time.deltaTime);
            }
            else
            {
                cc.Move(movement * moveSpeed * Time.deltaTime);
            }

        }
        else
        {
            cc.Move(movement * moveSpeed * Time.deltaTime);
        }
        
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
        if (hit.collider.CompareTag("Ground"))
        {
            if (hit.normal.y > 0.9f)
            {
                canJump = true;
            }


        }
    }

    IEnumerator staminaWait()
    {
        yield return new WaitForSeconds(1);
        if (!staminaAnim)
        {
            startStaminanim = false;

        }
       
    }

}