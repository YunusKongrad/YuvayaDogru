using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharTirmanma : MonoBehaviour
{
    CharacterController cc;
    public Transform kameraTransform;
    public GameObject pressE;
    private float tirmanmaHizi = 3f, mesefa = 3f;
    public LayerMask tirmanmaLayer;
    public bool tirmanmaAktif = false;
    private Vector2 moveInput;
    private GameControls kontroller;
    private Vector3 duvarinPos;
    Char_Controller _char;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        _char = GetComponent<Char_Controller>();
        kontroller = new GameControls();
    }
    private void OnEnable()
    {
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
        CheckClimbUi();
        if(tirmanmaAktif == true)
        {
            ClimbMovement();
            CheckExitConditions();
        }
    }
    private bool IsClimbableAhead()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, mesefa, tirmanmaLayer) == true)
        {
            duvarinPos = hit.point - hit.normal * 0.46f;
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
        Debug.Log("E'ye basildi!");
        if(IsClimbableAhead() == true)
        {
            Debug.Log("Duvar var, týrmanmaya giriyor");
            transform.position = duvarinPos;
            tirmanmaAktif = true;
            _char.isSticky = true;
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
            StopClimb();
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
