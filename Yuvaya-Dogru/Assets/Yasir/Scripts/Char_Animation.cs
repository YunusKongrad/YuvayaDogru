using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Char_Animation : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Char_Controller controller;
    float currentSpeed;
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

    }

    public void Hold()
    {
        animator.SetBool("Hold", true);
        controller.canClimb = true;
    }

    public void Climb()
    {
      
        animator.SetTrigger("Climb");
       
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
        transform.GetChild(0).localRotation = Quaternion.Euler(-90, 90, 90);

    }
    public void ClimbWait()
    {
        animator.SetBool("Climb2", false);
        animator.SetBool("Climb3", true);
        transform.GetChild(0).localRotation = Quaternion.Euler(-90, 90, 90);
    }

    public void ClimbEnd()
    {
        animator.SetBool("Climb2", false);
        animator.SetBool("Climb3", false);
       
        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);

    }
    void Update()
    {

    }
}
