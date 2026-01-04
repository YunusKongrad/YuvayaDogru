using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharTirmanma : MonoBehaviour
{
    CharacterController cc;
    public Transform kameraTransform;
    public GameObject pressE;
    private float tirmanmaHizi = 3f, mesefa = 1f, duvarYoksuresi = 0f, maxDuvarYokSuresi = 0.15f;
    public LayerMask tirmanmaLayer;
    public bool tirmanmaAktif = false;
    private Vector2 moveInput;
    private GameControls kontroller;
    private Vector3 duvarinPos, duvarinYonu;
    private Char_Controller _char;


    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        kontroller = new GameControls();
        _char = GetComponent<Char_Controller>();
    }
    private void OnEnable()
    {
        kontroller.Enable();
        kontroller.Player.Enable();
        kontroller.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        kontroller.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        kontroller.Player.Climb.performed += TryStartClimb;
    }
    private void OnDisable()
    {
        kontroller.Disable();
    }
    private void Update()
    {
        Debug.Log("Ray sonucu: " + IsClimbableAhead());

        CheckClimbUi();
        if(tirmanmaAktif == true)
        {
            ClimbMovement();
            CheckExitConditions();
        }
    }
    private bool IsClimbableAhead()
    {
        Vector3 rayYonu;
        if (tirmanmaAktif == true)
        {
            rayYonu = transform.forward;
        }
        else
        {
            rayYonu = kameraTransform.forward;
        }
        Debug.DrawRay(transform.position + Vector3.up, rayYonu * mesefa, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, rayYonu, out hit, mesefa, tirmanmaLayer) == true)
        {
            duvarinPos = hit.point - hit.normal * 0.46f;
            duvarinYonu = -hit.normal;
            return true;
        }
        else
        {
            return false;
        }
    }
    private void CheckClimbUi()
    {
        if(tirmanmaAktif == false && IsClimbableAhead() == true)
        {
            pressE.SetActive(true);
        }
        else
        {
            pressE.SetActive(false);
        }
    }
    private void TryStartClimb(InputAction.CallbackContext ctx)
    {
        if(IsClimbableAhead() == true)
        {
            _char.isSticky = true;
            transform.position = duvarinPos;
            transform.forward = -duvarinYonu;
            tirmanmaAktif = true;
        }
        else
        {
            Debug.Log("e basýldý ama duvar yok");
        }
    }
    private void ClimbMovement()
    {
        Vector3 move = Vector3.up * moveInput.y + transform.right * moveInput.x;
        cc.Move(move * tirmanmaHizi * Time.deltaTime);
    }
    private void CheckExitConditions()
    {
        if(IsClimbableAhead() == false)
        {
            duvarYoksuresi += Time.deltaTime;
            if (duvarYoksuresi > maxDuvarYokSuresi)
            {
                StopClimb();
            }
        }
        else if (IsClimbableAhead() == true)
        {
            duvarYoksuresi = 0f;
        }
    }
    private void StopClimb()
    {
        tirmanmaAktif = false;
        _char.isSticky = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TirmanmaEnUst"))
        {
            StopClimb();
            cc.Move(Vector3.up * 0.5f);
        }
    }
}
