using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
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
    [SerializeField] GameObject _pushingObj, hangingobj, _groundChechObj;
    [SerializeField] Vector3 _pushingObjPos;
    bool hitGround;
    Vector2 slideMove;
    Coroutine _slindingC;
    [Header("Karakter ozellikleri")]
    [SerializeField] float moveSpeed;
    [SerializeField] float speed, jumpForce, RunSpeed, pushingSpeed, originalStepOffset, sphereRadius, sphereDistance;
    [Header("Kal�c� de�erler")]
    public Vector3 movement;
    [SerializeField] float gravity, velocityY, moveMulti, rayDistance, gravityLimit, stamina, maxStamina, staminaFactor;

    [Header("Kontroller")]
    [SerializeField] bool isGrounded;
    [SerializeField] bool wasGrounded, isWaitingFall, jumpPressed, sopungJumped, isRunning, canJump, isHanging;
    public bool canClimb, isSticky;
    [Header("stamina")]
    [SerializeField] bool staminaAnim, startStaminanim;
    [SerializeField] Image staminaBar, staminaBar2;
    [SerializeField] float staminaAlpha, crumpAlpha;
    [SerializeField] int crumpCount;
    
    [SerializeField] Image crumpImage;
    [SerializeField] TextMeshProUGUI crumpT;

    [Header("Tirmanma")]
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public float ikWeight, ClimpSpeed, snapDuration;
    public bool isClimbing, isBlocking;

    private void Awake()
    {
        // 2. Nesneyi haf�zada olu�turuyoruz
        _controls = new GameControls();

        cc = GetComponent<CharacterController>();
        charTirmanmaCS = GetComponent<CharTirmanma>();

    }

    private void OnEnable()
    {
        // 3. Kontrolleri aktif ediyoruz (Bu olmazsa tu�lar �al��maz!)
        _controls.Enable();
        _controls.Player.Jump.performed += DoJump;
        _controls.Player.Run.performed += Run_performed;
        _controls.Player.Run.canceled += Run_canceled;
        _controls.Player.Climb.performed += Climb_performed;
        _controls.Player.Eat.performed += Eat_performed;

    }

    private void Eat_performed(InputAction.CallbackContext obj)
    {
        if (crumpCount > 0 && stamina < maxStamina)
        {
            crumpCount--;
            stamina += 15;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }

            
        }
        crumpT.text = crumpCount.ToString();
        crumpAlpha = 255;
        crumpImage.gameObject.SetActive(true);
        StartCoroutine(CrumpAnim());
    }
    IEnumerator CrumpAnim()
    {


        while(crumpImage.color.a>0.1f)
        {
            crumpAlpha -= Time.deltaTime * 150;
            crumpImage.color = new Color32(255, 255, 255, (byte)crumpAlpha);
            crumpT.color= new Color32(0, 0, 0, (byte)crumpAlpha);
            yield return null;
        }

        crumpImage.gameObject.SetActive(false);
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

        _moveInput = _controls.Player.Move.ReadValue<Vector2>();

        if (staminaAnim && staminaAlpha < 1)
        {
            staminaAlpha += Time.deltaTime;
            staminaBar.color = new Color(staminaBar.color.r, staminaBar.color.g, staminaBar.color.b, staminaAlpha);
            if (staminaAlpha < .5f)
            {
                staminaBar2.color = new Color(0, 0, 0, staminaAlpha);
            }

        }
        else if (staminaAlpha > 0)
        {
            staminaAlpha -= Time.deltaTime;
            staminaBar.color = new Color(staminaBar.color.r, staminaBar.color.g, staminaBar.color.b, staminaAlpha);
            if (staminaAlpha < .5f)
            {
                staminaBar2.color = new Color(0, 0, 0, staminaAlpha);
            }

        }


        forward = _cameraTransform.forward;

        if (_moveInput != Vector2.zero && isRunning)
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

        #region camCheck

        RaycastHit hit;
        isBlocking = Physics.Raycast(transform.position, -forward, out hit, 3, groundLayer);


        #endregion
    }


    public void OnLanded()
    {
        if (jumpPressed)
        {
            speed /= moveMulti;
            jumpPressed = false;
        }
        isGrounded = true;
        sopungJumped = false;
        animator.Falling(false);
    }



    void SopungJump()
    {
        velocityY += jumpForce * 2;
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
    
    IEnumerator SlindingMove()
    {

        while (isSliding)
        {


            slideMove.x = Mathf.MoveTowards(slideMove.x, _moveInput.x, Time.deltaTime);
            slideMove.y = Mathf.MoveTowards(slideMove.y, _moveInput.y, Time.deltaTime);


            yield return null;
        }
        _slindingC = null;
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
                hitGround = Physics.SphereCast(transform.position, sphereRadius,
              Vector3.down, out RaycastHit hitInfo, sphereDistance, groundLayer);

                if (cc.isGrounded && !hitGround)
                {

                    if (hitInfo.collider != null)
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
                    if (isSliding)
                    {
                        movement = ((forward * slideMove.y) + (right * slideMove.x)) * speed;
                    }
                    else
                    {
                        movement = ((forward * _moveInput.y) + (right * _moveInput.x)) * speed;
                    }
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
                }
                if (_moveInput.x > 0 && _moveInput.y < 0)
                {
                    transform.localRotation = Quaternion.Euler(0, 135, 0);
                    animspeed = 1;
                }
            }
            else
            {
                if (_moveInput.x + _moveInput.y >= 1.35)
                {
                    transform.localRotation = Quaternion.Euler(0, 45, 0);
                    animspeed = 1;
                }
                else if (_moveInput.x + _moveInput.y <= -1.35)
                {
                    transform.localRotation = Quaternion.Euler(0, -135, 0);
                    animspeed = 1;
                }
                else
                {
                    if (_moveInput.y != 0)
                    {
                        if (_moveInput.y < 0)
                        {
                            transform.localRotation = Quaternion.Euler(0, 180, 0);
                            animspeed = 1;
                        }
                        else
                        {
                            transform.localRotation = Quaternion.Euler(0, 0, 0);
                            animspeed = 1;
                        }
                    }
                    if (_moveInput.x != 0)
                    {
                        if (_moveInput.x > 0)
                        {
                            transform.localRotation = Quaternion.Euler(0, 90, 0);
                            animspeed = 1;
                        }
                        else
                        {
                            transform.localRotation = Quaternion.Euler(0, 270, 0);
                            animspeed = 1;
                        }
                    }
                }
            }
            //Debug.Log(_moveInput);
            if (velocityY<-1f && !hitGround)
            {
                animator.Falling(true);
            }
            else
            {
                animator.Falling(false);
            }
                animator.WalkAnim(MathF.Abs(animspeed * speed));
            cc.Move(movement * Time.deltaTime);
        }


    }
    private Vector3 _contactNormal;
    private void OnDisable()
    {
        _controls.Disable();
    }

    IEnumerator StaminaWait()
    {
        yield return new WaitForSeconds(1);
        if (!isRunning && !_isCurrentlyPushing)
        {
            staminaAnim = false;
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        _contactNormal = hit.normal;
        if (hit.collider.CompareTag("SliderArea"))
        {
            isSliding = true;
            // Eğer hali hazırda bir coroutine çalışmıyorsa başlat
            if (_slindingC == null)
            {
                _slindingC = StartCoroutine(SlindingMove());
            }

        }
        else
        {
            isSliding = false;
            _slindingC = null;
        }
        if (hit.collider.CompareTag("Crump"))
        {
            stamina += 1;
            crumpCount++;
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }
            crumpT.text = crumpCount.ToString();
            staminaAnim = true;
            StartCoroutine(StaminaWait());
            Destroy(hit.gameObject);
        }
        if (hit.collider.CompareTag("Sopung"))
        {
            if (hit.normal.y > 0.9f)
            {
                if (!sopungJumped)
                {
                    SopungJump();
                    sopungJumped = true;
                    animator.Falling(false);
                }
            }


        }
        else if (hit.collider.gameObject.layer == 6 && hit.collider.tag != "Sopung" && cc.velocity.y < -1f)
        {

            if (hit.normal.y > 0.5f)
            {
                canJump = true;
            }
            else if (hit.normal.y > -.1f && hit.collider.tag == "rope")
            {

                SetHanging(hit);
            }
            if (hit.collider.transform.position.y >= transform.position.y)
            {
                if (hit.normal.y > 0.9f)
                {
                    canJump = true;
                }
                else if (Mathf.Abs(hit.normal.y) < 0.1f && hit.collider.tag == "rope")
                {
                    SetHanging(hit);

                }

            }


        }
        if (hit.collider.CompareTag("Pushable") && stamina>0)
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
                stamina -= Time.deltaTime*0.2f;
                staminaAnim = true;                              
                _pushingObjPos = hit.gameObject.transform.position;
                _pushingObjPos -= hit.normal * Time.deltaTime * speed;
                hit.gameObject.transform.position = _pushingObjPos;
            }
            //Debug.DrawRay(hit.transform.position, -hit.normal * dynamicRayDistance, Color.red);
        }


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
    private bool isSliding;

    private void LateUpdate()
    {
        if (_isCurrentlyPushing)
        {
            _isCurrentlyPushing = false;
        }
        else
        {
            staminaAnim = false;
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