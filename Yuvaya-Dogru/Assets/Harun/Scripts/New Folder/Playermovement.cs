using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermovement : MonoBehaviour
{
    /*
    public float moveSpeed = 6f;
    public LayerMask groundLayer;

    Rigidbody rb;
    bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        CheckGround();

        if (!isGrounded)
        {
            // Havada kontrol yok
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(x, 0f, z) * moveSpeed;
        rb.velocity = new Vector3(move.x, rb.velocity.y, move.z);
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(
            transform.position + Vector3.up * 0.1f,
            Vector3.down,
            1.2f,
            groundLayer
        );
    }
    */
    
    //aa
    
    public float moveSpeed = 6f;

    public float normalGravity = -20f;
    public float lowGravity = -6f;   // Fan çıkışı sonrası
    float currentGravity;

    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentGravity = normalGravity;
    }

    void Update()
    {
        bool grounded = controller.isGrounded;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move =
            transform.right * x +
            transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        //  ZEMİNE DEĞİNCE
        if (grounded)
        {
            velocity.y = 0f;          // DUR
            currentGravity = 0f;      // Gravity KAPALI
        }
        else
        {
            // Gravity uygula
            velocity.y += currentGravity * Time.deltaTime;
        }

        controller.Move(velocity * Time.deltaTime);
    }

    //  FAN ALANINDAN ÇIKINCA ÇAĞRILACAK
    public void SetLowGravity()
    {
        currentGravity = lowGravity;
    }

    //  NORMAL GRAVITY (GEREKİRSE)
    public void SetNormalGravity()
    {
        currentGravity = normalGravity;
    }
}
