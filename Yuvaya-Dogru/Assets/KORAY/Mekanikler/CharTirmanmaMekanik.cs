using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharTirmanmaMekanik : MonoBehaviour
{
    CharacterController charController;
    public Transform kameraTransform;
    public GameObject pressEUi;
    private float tirmanmaHizi = 3f, rayMesafesi = 1f;
    public LayerMask tirmanmaLayeri;
    private bool tirmanmaAktif = false;
    private Vector2 moveInput;
    private GameControls inputMapi;
    private Vector3 duvarinPos, duvarinYonu, tirmanmaOncesiYon;
    private Char_Controller _char;
    private string raycastAtilmaNedeni = "Duvar", interaksiyonaGirmeNedeni = "Yok";
    private float tirmanmaOncesiZ;
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
                transform.position = duvarinPos;
                transform.forward = duvarinYonu;
                tirmanmaOncesiZ = transform.position.z;
                tirmanmaOncesiYon = kameraTransform.forward.normalized;
                tirmanmaAktif = true;
                _char.isSticky = true;
                raycastAtilmaNedeni = "DuvardanCikis";
                break;
            case "Duvardan›nme":
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
        if(tirmanmaAktif == true)
        {
            Vector3 move = Vector3.up * moveInput.y + transform.right * moveInput.x;
            charController.Move(move * tirmanmaHizi * Time.deltaTime);
        }
    }
    private void RaycastAtici()
    {
        RaycastHit hit;
        if(raycastAtilmaNedeni == "Duvar")
        {
            if(Physics.Raycast(transform.position + Vector3.up, kameraTransform.forward, out hit, rayMesafesi, tirmanmaLayeri) == true)
            {
                duvarinPos = hit.point - hit.normal * (charController.radius + 0.05f);
                duvarinYonu = -hit.normal;
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
            Physics.Raycast(rayPozisyonu, tirmanmaOncesiYon, out hit, rayMesafesi);
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
                interaksiyonaGirmeNedeni = "Duvardan›nme";
            }
            else
            {
                DuvardanCikis();
            }
        }
    }
    private void DuvardanCikis()
    {
        tirmanmaAktif = false;
        _char.isSticky = false;
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

}
