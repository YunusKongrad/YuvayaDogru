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
    
    GameControls _controls;
     Vector2 _moveInput;
    [SerializeField] Char_Animation animator;
    [SerializeField] CamController cam;
    CharacterController cc;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform _cameraTransform, _hangingPos;
    [SerializeField] GameObject _pushingObj,hangingobj,_groundChechObj;
    [SerializeField] Vector3 _pushingObjPos;
    [Header("Karakter özellikleri")]
    [SerializeField] float moveSpeed;
    [SerializeField] float speed,jumpForce, RunSpeed,pushingSpeed,ClimpSpeed, snapDuration, originalStepOffset, sphereRadius,sphereDistance;

    [Header("Kalýcý deðerler")]
    public Vector3 movement;
    [SerializeField] float gravity, velocityY, moveMulti, rayDistance, gravityLimit,stamina,maxStamina,staminaFactor;

    [Header("Kontroller")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool wasGrounded,isWaitingFall,jumpPressed,sopungJumped,isRunning,isClimbing, canJump, isHanging;
    public bool canClimb, isSticky;
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
        _controls.Player.Climb.performed += Climb_performed;
       
    }

    private void Climb_performed(InputAction.CallbackContext obj)
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
    
    public void anim2bos(Transform target)
    {
        // 1. Fiziði ve kontrolü kapat ki titreme yapmasýn
        // Baþlangýç deðerlerini al
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        float timeElapsed = 0;
        Debug.Log("denemeasfdasdf");
        while (timeElapsed < snapDuration)
        {
            // Lerp ile yumuþak geçiþ (0'dan 1'e giden bir oran hesapla)
            float t = timeElapsed / snapDuration;

            // Yumuþak hareket (Ease-Out efekti için Mathf.SmoothStep kullanýlabilir)
            t = t * t * (3f - 2f * t); // SmoothStep formülü

            // Pozisyonu ve Dönüþü (Rotasyonu) hedefle eþle
            transform.position = Vector3.Lerp(startPos, target.position, t);
            transform.rotation = Quaternion.Lerp(startRot, target.rotation, t);

            timeElapsed += Time.deltaTime;
            //yield return null; // Bir sonraki kareyi bekle
        }

        // 2. Tam hedefe kilitle (Küsürat hatalarýný önlemek için)
        transform.position = target.position;
        transform.rotation = target.rotation;

        // BURADA: Artýk karakter tutunuyor.
        // controller.enabled = true; // DÝKKAT: Týrmanma bitene kadar bunu açma!
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
      
        _moveInput = _controls.Player.Move.ReadValue<Vector2>();
        
      
      
        forward = _cameraTransform.forward;
      
        if (_moveInput!=Vector2.zero && isRunning)
        {
            stamina -= Time.deltaTime * staminaFactor;
          
        }

        
      

      
        right = _cameraTransform.right;
        MoveCharacter();
        staminaBar.fillAmount = stamina / maxStamina;
      

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
        if (isGrounded)
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
                bool hitGround = Physics.SphereCast(transform.position, sphereRadius,
              Vector3.down, out RaycastHit hitInfo, sphereDistance, groundLayer);
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
            if (_moveInput.y < 0)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
            animator.WalkAnim(MathF.Abs(_moveInput.y * speed));
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
        else if (hit.collider.gameObject.layer == 6 && hit.collider.tag != "Sopung" && cc.velocity.y<-1f)
        {
           
            if (hit.normal.y > 0.5f)
            {
                canJump = true;
            }
            else
            {

                SetHanging(hit);
            }
            if (hit.collider.transform.position.y >= transform.position.y + 1)
            {
                if (hit.normal.y > 0.9f)
                {
                    canJump = true;
                }
                else if (Mathf.Abs(hit.normal.y) < 0.1f)
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
        
        
    }

    void SetHanging(ControllerColliderHit hit)
    {
        if (isHanging==false)
        {
            if (hit.gameObject.transform.position.y> transform.position.y)
            {
                isHanging = true;
                normal = hit.normal;
                hangingobj = hit.collider.gameObject;

                cc.enabled = false;
                Collider coll = hangingobj.GetComponent<Collider>();
                Vector3 pos = hangingobj.transform.position - ((-new Vector3(hit.normal.x * coll.bounds.size.x,
                 hit.normal.y* coll.bounds.size.y, hit.normal.z * coll.bounds.size.z)));
                _hangingPos.position = pos;
                cc.enabled = false;
                transform.position = pos;
                cam.isClimbing = true;
                Debug.Log(hit.gameObject.name);
                 Vector3 rot = Vector3.zero;
                if (hit.normal.z < 0)
                {
                    rot.y = 0;
                }
                else if (hit.normal.z >0)
                {
                    rot.y = 180;
                }
                else if(hit.normal.x < 0)
                {
                    rot.y = 90;
                }
                else if (hit.normal.x > 0)
                {
                    rot.y = 270;
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