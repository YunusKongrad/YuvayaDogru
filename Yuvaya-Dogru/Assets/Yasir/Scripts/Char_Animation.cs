using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Char_Animation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Char_Controller controller;
    float currentSpeed;
    public AudioSource audioSource;
    public AudioClip yurumeSesi, tirmanmaSesi;
    void PlaySound(AudioClip ses)
    {
        if(ses == null) { return; }
        audioSource.PlayOneShot(ses);
    }
    void Start()
    {

    }
    public void JumpAnim()
    {
        animator.SetTrigger("Jump");
    }

    public void WalkAnim(float speed)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 5f);
        animator.SetFloat("Speed", currentSpeed);
        //PlaySound(yurumeSesi);
    }

    public void Hold()
    {
        animator.SetBool("Hold", true);
        controller.canClimb = true;
    }

    public void Climb()
    {
      
        animator.SetTrigger("Climb");
        PlaySound(tirmanmaSesi);
    }
    public void EndHold()
    {
        animator.SetBool("Hold", false);
       
    }

    public void Falling(bool fall)
    {
        animator.SetBool("Falling", fall);

    }
    void Update()
    {

    }
}
