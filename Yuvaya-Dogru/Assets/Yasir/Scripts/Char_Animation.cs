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


    void Update()
    {

    }
}
