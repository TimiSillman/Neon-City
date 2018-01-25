using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0F;
    public float jumpForce = 8.0F;
    public float verticalVelocity;


    //Private Variables
    Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    float gravity = 20.0F;
    Vector3 moveVector;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpForce;

        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.isGrounded && hit.normal.y < 0.1f)
        {

            if (Input.GetButton("Jump"))
            {
                Debug.Log("HIT");
                verticalVelocity = jumpForce;
                moveVector = hit.normal * speed;
            }
        }
    }


}
