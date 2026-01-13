using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CharTirmanmaMekanik : MonoBehaviour
{
    CharacterController charController;
    public Transform kameraTransform;
    public GameObject pressEUi;
    [SerializeField] float tirmanmaHizi = 3f, rayMesafesi = 1f,turningSpeed;
    public LayerMask tirmanmaLayeri;
    public bool tirmanmaAktif = false,isTurning;
    private Vector2 moveInput;
    private GameControls inputMapi;
    private Vector3 duvarinPos, duvarinYonu, tirmanmaOncesiYon;
    private Char_Controller _char;
    private GameControls _controls;
    private string raycastAtilmaNedeni = "Duvar", interaksiyonaGirmeNedeni = "Yok";
    public float tirmanmaOncesiZ;
    private RaycastHit tutunmaHiti;
    
    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        inputMapi = new GameControls();
        _char = GetComponent<Char_Controller>();
    }
    private void OnEnable()
    {
        inputMapi.Enable();
        inputMapi.Player.Enable();
        inputMapi.Player.Move.performed += moveBilgisi => moveInput = moveBilgisi.ReadValue<Vector2>();
        inputMapi.Player.Move.canceled += moveBilgisi => moveInput = Vector2.zero;
        inputMapi.Player.Climb.performed += ETusiInteraksiyonlari;
    }
    private void OnDisable()
    {
        inputMapi.Disable();
    }
    private void ETusiInteraksiyonlari(InputAction.CallbackContext moveBilgisi)
    {
        switch (interaksiyonaGirmeNedeni)
        {
            case "Yok":
                Debug.Log("E'ye basildi ama interaksiyon yok");
                break;
            case "Duvar":
               
                tirmanmaOncesiZ = transform.position.z;
                transform.position = duvarinPos;
                transform.forward = duvarinYonu;
                tirmanmaOncesiYon = kameraTransform.forward.normalized;
                tirmanmaAktif = true;
                _char.isSticky = true;
                _char.cam.isClimbing = true;
                raycastAtilmaNedeni = "DuvardanCikis";
                break;
            case "DuvardanÝnme":
                DuvardanCikis();
                break;
            case "DuvarinUstuneCikma":
                DuvarinUstuneCikma();
                DuvardanCikis();
                break;
        }
    }
    private void Update()
    {
        RaycastAtici();
        if(tirmanmaAktif && !isTurning)
        {
            Vector3 move = Vector3.up * moveInput.y + transform.right * moveInput.x;
            charController.Move(move * tirmanmaHizi * Time.deltaTime);
        }
    }
    RaycastHit hit;
    private void RaycastAtici()
    {
        
        if(raycastAtilmaNedeni == "Duvar")
        {
            Debug.DrawRay(transform.position + Vector3.up, kameraTransform.forward, Color.green);
            if(Physics.Raycast(transform.position + Vector3.up, kameraTransform.forward, out hit, rayMesafesi, tirmanmaLayeri) == true)
            {
                duvarinPos = hit.point;
                duvarinPos -= hit.normal;
                Debug.Log(hit.normal);
                    duvarinYonu = hit.normal;
                pressEUi.SetActive(true);
                interaksiyonaGirmeNedeni = "Duvar";
            }
            else
            {
                pressEUi.SetActive(false);
                interaksiyonaGirmeNedeni = "Yok";
            }
        }
        else if (raycastAtilmaNedeni == "DuvardanCikis")
        {
           
            Vector3 rayPozisyonu = new Vector3(transform.position.x, transform.position.y + 1f, tirmanmaOncesiZ);
            Debug.DrawRay(rayPozisyonu, tirmanmaOncesiYon, Color.yellow);
            Physics.Raycast(rayPozisyonu, tirmanmaOncesiYon, out hit, rayMesafesi);
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("TirmanmaAlaniTag"))
                    {
                        pressEUi.SetActive(false);
                        interaksiyonaGirmeNedeni = "Yok";
                    }
                    else if (hit.collider.CompareTag("TirmanmaEnUst"))
                    {
                        pressEUi.SetActive(true);
                        tutunmaHiti = hit;
                        interaksiyonaGirmeNedeni = "DuvarinUstuneCikma";
                    }
                    else if (hit.collider.CompareTag("TirmanmaEnAlt"))
                    {
                        pressEUi.SetActive(true);
                        interaksiyonaGirmeNedeni = "DuvardanÝnme";
                    }
                    else
                    {
                        DuvardanCikis();
                    }

                }
             
            }
          
        }
        if (hit.collider==null)
        {
            DuvardanCikis();
        }
    }
    private void DuvardanCikis()
    {
        if (tirmanmaAktif)
        {
            _char.OnLanded();
             _char.isSticky = false;
            _char.cam.isClimbing = false;
        }
        tirmanmaAktif = false;       
        raycastAtilmaNedeni = "Duvar";
        pressEUi.SetActive(false);
        interaksiyonaGirmeNedeni = "Yok";
       
       
    }
    private void DuvarinUstuneCikma()
    {
        Vector3 cikisPozisyonu = tutunmaHiti.point + Vector3.up * (charController.height * 0.5f) - tutunmaHiti.normal * (charController.radius + 0.05f);
        charController.enabled = false;
        transform.position = cikisPozisyonu;
        charController.enabled = true;
        transform.forward = -tutunmaHiti.normal;
    }

    public void JumpOther()
    {
        isTurning = true;
        targetAngle += 180;
        toward *= -1;
        StartCoroutine(Jump());
        
    }
   [SerializeField] int targetAngle,toward=1;
    IEnumerator Jump()
    {
        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        // Açý farký 0.1 dereceden büyük olduðu sürece devam et
        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            // Dönüþ
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                turningSpeed * Time.deltaTime
            );

            // Ýleri hareket (Kendi baktýðý yöne doðru)
            Vector3 move = new Vector3(0, 0, toward * 1*Time.deltaTime);
            charController.Move(move);

            yield return null;
        }
        if (Physics.Raycast(transform.position + Vector3.up, kameraTransform.forward, out hit,
            rayMesafesi, tirmanmaLayeri))
        {
          
        }
        else
        {
            _char.isSticky = false;
        }
            // Tam hizalama
            transform.rotation = targetRotation;
        isTurning = false;
    }
}
