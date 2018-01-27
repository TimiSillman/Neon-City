using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20;
    public float jumpForce = 15;
    public float wallJumpSpeed = -1f;
    public float speedSmooth;

    //Private Variables
    public Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    float gravity = 30f;
    Vector3 currentSpeed = Vector3.zero;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {

        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection.x *= speed;
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpForce;
            }
            
        } else if (!controller.isGrounded)
        {
            if (Input.GetButton("Horizontal"))
            {
                moveDirection.x = Input.GetAxis("Horizontal");
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection.x *= speed;
            }
        }


        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
        currentSpeed = moveDirection;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
 

        if (!controller.isGrounded && hit.normal.y < 0.1f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("HIT");
                moveDirection.y = jumpForce;
                moveDirection.x *= wallJumpSpeed;
            }
            
        }
    }

}
