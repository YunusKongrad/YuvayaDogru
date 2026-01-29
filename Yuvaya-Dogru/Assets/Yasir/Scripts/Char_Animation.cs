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
    public AudioClip yurumeSesi, tirmanmaSesi,zýplamaSesi, eatSound;
    private float timer = 0f;
    [SerializeField] private float stepInterval = 0.5f;
    void PlaySound(AudioClip ses)
    {
        if(ses == null) { return; }
        audioSource.PlayOneShot(ses);
    }
 
    public void JumpAnim()
    {
        animator.SetTrigger("Jump");
        PlaySound(zýplamaSesi);
    }
    public void Eat()
    {
        PlaySound(eatSound);
    }
    public void onland()
    {
        PlaySound(zýplamaSesi);
    }
    public void WalkAnim(float speed)
    {
        currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * 5f);
        animator.SetFloat("Speed", currentSpeed);

        Debug.Log(speed);
        if (speed > 0)
        {
            timer += Time.deltaTime;

            if (timer >= stepInterval)
            {
                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.PlayOneShot(yurumeSesi);
                timer = 0f;
            }
        }
        else
        {
        
            audioSource.Stop();
            timer = stepInterval;
        }

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
    public void ClimbStart()
    {
        animator.SetBool("Climb2", true);
        animator.SetBool("Climb3", false);
        Debug.Log("clim1");
    }
    public void ClimbWait()
    {
        animator.SetBool("Climb2", false);
        animator.SetBool("Climb3", true);
        //transform.GetChild(0).localRotation = Quaternion.Euler(-90, 90, -90);
        Debug.Log("clim2");
    }

    public void ClimbEnd()
    {
        animator.SetBool("Climb2", false);
        animator.SetBool("Climb3", false);
       
        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        Debug.Log("clim3");

    }
    void Update()
    {

    }
}
