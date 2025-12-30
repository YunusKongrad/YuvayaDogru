using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class ackapa : MonoBehaviour
{
   
   
   [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 2f;
    public bool isOpen = false;

    [Header("Interaction")]
    public float interactDistance = 3f;
    public Camera playerCamera;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openCloseSound;

    [Header("UI")]
    public TextMeshProUGUI interactText;

    private Quaternion _closedRotation;
    private Quaternion _openRotation;
    private Coroutine _currentCoroutine;
    private bool _playerLooking;

    private void Start()
    {
        _closedRotation = transform.rotation;
        _openRotation = Quaternion.Euler(transform.eulerAngles + new Vector3(0, openAngle, 0));

        if (playerCamera == null)
            playerCamera = Camera.main;

        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    private void Update()
    {
        _playerLooking = IsLookingAtThis();

        if (interactText != null)
            interactText.gameObject.SetActive(_playerLooking);

        if (_playerLooking && interactText != null)
            interactText.text = isOpen ? "E - Kapat" : "E - Aç";

        if (_playerLooking && Input.GetKeyDown(KeyCode.E))
        {
            if (_currentCoroutine != null)
                StopCoroutine(_currentCoroutine);

            if (audioSource && openCloseSound)
                audioSource.PlayOneShot(openCloseSound);

            _currentCoroutine = StartCoroutine(ToggleDoor());
        }
    }

    bool IsLookingAtThis()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        Debug.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * interactDistance,
            Color.green
        );

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            return hit.transform.GetComponentInParent<ackapa>() == this;
        }
        return false;
    }

    IEnumerator ToggleDoor()
    {
        Quaternion targetRotation = isOpen ? _closedRotation : _openRotation;
        isOpen = !isOpen;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.01f)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * openSpeed
            );
            yield return null;
        }

        transform.rotation = targetRotation;
    }
}
