using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngineInternal;
using static UnityEngine.GraphicsBuffer;
public class Char_Controller : MonoBehaviour
{
    private CharTirmanma charTirmanmaCS;   
    GameControls _controls;
     Vector2 _moveInput;
    [SerializeField] Char_Animation animator;
    [SerializeField] CamController cam;
    CharacterController cc;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform _cameraTransform, _hangingPos;
    [SerializeField] GameObject _pushingObj,hangingobj,_groundChechObj;
    [SerializeField] Vector3 _pushingObjPos;
    [Header("Karakter �zellikleri")]
    [SerializeField] float moveSpeed;
    [SerializeField] float speed,jumpForce, RunSpeed,pushingSpeed, originalStepOffset, sphereRadius,sphereDistance;
    [Header("Kal�c� de�erler")]
    public Vector3 movement;
    [SerializeField] float gravity, velocityY, moveMulti, rayDistance, gravityLimit,stamina,maxStamina,staminaFactor;

    [Header("Kontroller")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool wasGrounded,isWaitingFall,jumpPressed,sopungJumped,isRunning, canJump, isHanging;
    public bool canClimb, isSticky;
    [Header("stamina")]
    [SerializeField] bool staminaAnim/*, startStaminanim*/;
    [SerializeField] Image staminaBar, staminaBar2;
    [SerializeField] float staminaAlpha;

    [Header("T�rmanma")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public float ikWeight, ClimpSpeed, snapDuration;
    public bool isClimbing;
    
    [Header("Asperator Ozellikleri")]
    public bool isVacuumed = false;
    public int vacuumStage = 0;
    [Header("Recel Ozellikleri")]
    [SerializeField] float jamSlowSpeed = 2f;
    [SerializeField] float jamNormalSpeed = 5f;
    [SerializeField] float jamDuration = 10f;

    Coroutine jamRoutine;
    
    bool isJammed = false;

    

    private void Awake()
    {
        // 2. Nesneyi haf�zada olu�turuyoruz
        _controls = new GameControls();
     
        cc = GetComponent<CharacterController>();
        charTirmanmaCS = GetComponent<CharTirmanma>();
        
       

    }

    private void OnEnable()
    {
        if (_controls == null)
            _controls = new GameControls();
        
        // 3. Kontrolleri aktif ediyoruz (Bu olmazsa tu�lar �al��maz!)
        _controls.Enable();
        _controls.Player.Jump.performed += DoJump;
        _controls.Player.Run.performed += Run_performed;
        _controls.Player.Run.canceled += Run_canceled;
        _controls.Player.Climb.performed += Climb_performed;
       
    }

    private void Climb_performed(InputAction.CallbackContext obj)
    {
        if (!isSticky)
        {
            if (canClimb)
            {
                EndHanging();



                climb = true;
            }
            else
            {
                /**
                RaycastHit hit;
                if (Physics.Raycast(_cameraTransform.transform.position, forward, out hit, rayDistance))
                {
                   
                    if (hit.collider.CompareTag("rope"))
                    {
                        if (hit.collider.gameObject.transform.position.y > transform.position.y + 1)
                        {
                            isHanging = true;
                            normal = hit.normal;
                            hangingobj = hit.collider.gameObject;

                            cc.enabled = false;
                            Collider coll = hangingobj.GetComponent<Collider>();
                            Vector3 pos = hangingobj.transform.position - ((-new Vector3(hit.normal.x * coll.bounds.size.x,
                             hit.normal.y - ((cc.height + coll.bounds.size.y) / 2), hit.normal.z * coll.bounds.size.z)));
                            _hangingPos.position = pos;
                            animator.Hold();
                            StartCoroutine(ClimbAnim(_hangingPos));
                        }

                    }
                }
                **/
            }
        }
       
        
    }
    
    public void anim2bos(Transform target)
    {
        // 1. Fizi�i ve kontrol� kapat ki titreme yapmas�n
        // Ba�lang�� de�erlerini al
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float timeElapsed = 0;
        Debug.Log("denemeasfdasdf");
        while (timeElapsed < snapDuration)
        {
            // Lerp ile yumu�ak ge�i� (0'dan 1'e giden bir oran hesapla)
            float t = timeElapsed / snapDuration;

            // Yumu�ak hareket (Ease-Out efekti i�in Mathf.SmoothStep kullan�labilir)
            t = t * t * (3f - 2f * t); // SmoothStep form�l�

            // Pozisyonu ve D�n��� (Rotasyonu) hedefle e�le
            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Lerp(startRot, target.rotation, t);

            timeElapsed += Time.deltaTime;
            //yield return null; // Bir sonraki kareyi bekle
        }

        // 2. Tam hedefe kilitle (K�s�rat hatalar�n� �nlemek i�in)
        transform.position = target.position;
        transform.rotation = target.rotation;

        // BURADA: Art�k karakter tutunuyor.
        // controller.enabled = true; // D�KKAT: T�rmanma bitene kadar bunu a�ma!
    }
    public void EndHanging()
    {
        isHanging = false;
        canClimb = true;
        animator.Climb();
        movement = Vector3.zero;
        movement = (forward * _moveInput.y) + (right * _moveInput.x);
        movement.y += velocityY;
       

    }
    Vector3 normal;
    bool climb;

    public void EndClimb()
    {
        canClimb = false;
        isHanging = false;
        
        climb = false;
        isClimbing = false;
        Vector3 pos = hangingobj.transform.position;
        pos.y += 1f;
       // _hangingPos.position = pos;
        transform.position = pos;
        cc.enabled = true;
        animator.EndHold();
        cam.isClimbing = false;

    }
    private void Run_canceled(InputAction.CallbackContext obj)
    {
        //startStaminanim = false;
        isRunning = false;
        staminaAnim = false;
        speed -= RunSpeed;
    }

    private void Run_performed(InputAction.CallbackContext obj)
    {
        //startStaminanim = true;
        isRunning = true;
        staminaAnim = true;
        speed += RunSpeed;
    }

  
    Vector3 forward;
    Vector3 right;

    
    private void Update()
    {
        if (isVacuumed)
        {
            _moveInput = Vector2.zero;
            movement = Vector3.zero;
            velocityY = 0f;
            return;
        }
      
        _moveInput = _controls.Player.Move.ReadValue<Vector2>();
        
      
      
        forward = _cameraTransform.forward;
      
        if (_moveInput!=Vector2.zero && isRunning)
        {
            stamina -= Time.deltaTime * staminaFactor;
          
        }

        
      

      
        right = _cameraTransform.right;
        MoveCharacter();
        staminaBar.fillAmount = stamina / maxStamina;
      

        #region ziplma
      
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
            speed /= moveMulti;
            jumpPressed = false;
        }
        isGrounded = true;
        sopungJumped = false;
    }



    void SopungJump()
    {
        velocityY += jumpForce*2;
        animator.JumpAnim();
    }
   
    private void DoJump(InputAction.CallbackContext context)
    {
        if (isGrounded && !isSticky)
        {
            speed *= moveMulti;
            velocityY = jumpForce;          
            isGrounded = false;         
            jumpPressed = true;
            canJump = false;
            animator.JumpAnim();
        }
    }

    private void MoveCharacter()
    {
        if (isVacuumed)
            return;
        
        right = _cameraTransform.right;
        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();
        if (!isSticky)

        {
            if (cc.isGrounded)
            {

                cc.stepOffset = originalStepOffset;
            }
            else
            {

                cc.stepOffset = 0f;
            }

            if (isHanging)
            {

                cc.Move(movement * speed * Time.deltaTime);
            }
            else if (!canClimb)
            {
                bool hitGround =Physics.SphereCast(transform.position, sphereRadius,
              Vector3.down, out RaycastHit hitInfo, sphereDistance, groundLayer);

                if (cc.isGrounded && !hitGround)
                {
                    
                    if (hitInfo.collider!=null)
                    {
                        Debug.Log("asdasdasd");
                    }
                    else
                    {
                        isGrounded = false;
                    }
                }
                
                if (hitGround && velocityY < 0)
                {
                    velocityY = -2f;
                }
                else
                {
                    if (velocityY > gravityLimit)
                    {
                        velocityY -= gravity * Time.deltaTime;
                    }
                }
                if (cc.isGrounded && !hitGround)
                {

                    movement.x += _contactNormal.x * speed;
                    movement.z += _contactNormal.z * speed;                
                    
                }
                else
                {
                    movement = ((forward * _moveInput.y) + (right * _moveInput.x)) * speed;
                }

                movement.y += velocityY;


            }
            float animspeed = 0;
            if (_moveInput.x + _moveInput.y == 0)
            {
                if (_moveInput.y > 0 && _moveInput.x < 0)
                {
                    transform.localRotation = Quaternion.Euler(0, -45, 0);
                    animspeed = 1;
                    Debug.Log("0");
                }
                if (_moveInput.x > 0 && _moveInput.y < 0)
                {
                    transform.localRotation = Quaternion.Euler(0, 135, 0);
                    animspeed = 1;
                    Debug.Log("1");
                }
            }
            else
            {
                if (_moveInput.x + _moveInput.y >= 1.35)
                {
                    transform.localRotation = Quaternion.Euler(0, 45, 0);
                    animspeed = 1;
                    Debug.Log("2");
                }
                else if (_moveInput.x + _moveInput.y <= -1.35)
                {
                    transform.localRotation = Quaternion.Euler(0, -135, 0);
                    animspeed = 1;
                    Debug.Log("3");
                }
                else 
                {
                    if (_moveInput.y != 0)
                    {
                        if (_moveInput.y < 0)
                        {
                            transform.localRotation = Quaternion.Euler(0, 180, 0);
                            animspeed = 1;
                            Debug.Log("4");
                        }
                        else
                        {
                            transform.localRotation = Quaternion.Euler(0, 0, 0);
                            animspeed = 1;
                            Debug.Log("5");
                        }
                    }
                    if (_moveInput.x != 0)
                    {
                        if (_moveInput.x > 0)
                        {
                            transform.localRotation = Quaternion.Euler(0, 90, 0);
                            animspeed = 1;
                            Debug.Log("6");
                        }
                        else
                        {
                            transform.localRotation = Quaternion.Euler(0, 270, 0);
                            animspeed = 1;
                            Debug.Log("7");
                        }
                    }
                }
              

            }
            //Debug.Log(_moveInput);
            animator.WalkAnim(MathF.Abs(animspeed* speed));
            cc.Move(movement * Time.deltaTime);
        }


    }
    private Vector3 _contactNormal;
    private void OnDisable()
    {
        _controls.Disable();
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
         _contactNormal = hit.normal;
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
        else if (hit.collider.gameObject.layer == 6 && hit.collider.tag != "Sopung" && cc.velocity.y<-1f )
        {
           
            if (hit.normal.y > 0.5f)
            {
                canJump = true;
            }
            else if(hit.normal.y>-.1f && hit.collider.tag != "Camasir")
            {

                SetHanging(hit);
            }
            if (hit.collider.transform.position.y >= transform.position.y)
            {
                if (hit.normal.y > 0.9f)
                {
                    canJump = true;
                }
                else if (Mathf.Abs(hit.normal.y) < 0.1f && hit.collider.tag != "Camasir")
                {
                    SetHanging(hit);
                    
                }

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
        if (hit.collider.CompareTag("JAM"))
        {
            ApplyJamEffect();
        }
        
        
    }
    void ApplyJamEffect()
    {
        if (isJammed)
            return;

        isJammed = true;

        // TEMAS ANINDA
        moveSpeed = jamSlowSpeed;
        speed = jamSlowSpeed;
        RunSpeed = jamSlowSpeed;

        jamRoutine = StartCoroutine(JamReset());
    }
    
    
    IEnumerator JamReset()
    {
        yield return new WaitForSeconds(jamDuration);

        moveSpeed = jamNormalSpeed;
        speed = jamNormalSpeed;
        RunSpeed = jamNormalSpeed;

        isJammed = false;
        jamRoutine = null;
    }

    void SetHanging(ControllerColliderHit hit)
    {
        if (isHanging == false)
        {
            if (hit.gameObject.transform.position.y > transform.position.y)
            {
                isHanging = true;
                normal = hit.normal;
                hangingobj = hit.collider.gameObject;

                cc.enabled = false;
                Collider coll = hangingobj.GetComponent<Collider>();
               
                
                Vector3 pos = hit.gameObject.transform.position + (hit.normal * 0.8f);
                _hangingPos.position = pos;
                cc.enabled = false;
                transform.position = pos;
                cam.isClimbing = true;
                isClimbing = true;

                rightHandTarget = hit.gameObject.transform.GetChild(0);
                leftHandTarget = hit.gameObject.transform.GetChild(1);
                Vector3 rot = Vector3.zero;
                if (MathF.Abs(hit.normal.z) > MathF.Abs(hit.normal.x))
                {
                    if (hit.normal.z < 0)
                    {
                        rot.y = 0;

                    }
                    if (hit.normal.z > 0)
                    {
                        rot.y = 180;

                    }

                }
                else
                {
                    if (hit.normal.x < 0)
                    {
                        rot.y = 90;

                    }
                    else if (hit.normal.x > 0)
                    {
                        rot.y = 270;

                    }

                }

                transform.GetChild(0).localRotation = Quaternion.Euler(rot);
                animator.Hold();

            }

        }


    }
    bool _isCurrentlyPushing;
    IEnumerator staminaWait()
    {
        yield return new WaitForSeconds(1);
        if (!staminaAnim)
        {
            //startStaminanim = false;

        }
       
    }

    private void LateUpdate()
    {
        if (isJammed)
            return;
        
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
    
    public void VacuumMove(Vector3 velocity)
    {
        cc.Move(velocity * Time.deltaTime);
    }
}