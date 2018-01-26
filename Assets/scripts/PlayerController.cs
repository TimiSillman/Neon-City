using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 20;
    public float jumpForce = 15;
    public float wallJumpSpeed = -0.7f;


    //Private Variables
    Vector3 moveDirection = Vector3.zero;
    CharacterController controller;
    float gravity = 30f;

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

            //Tässä siis yritän tehdä että voi liikkua ilmassa, jos tossa ei ole tota GetButton(Horizontal) niin sitten se ei launchaa seinästä niinkuin pitäisi

            //Ongelma: Jos painaa A/D ja sitte Jump niin se kiipee pelkästää ylöspäin, mut jos ei paina noita niin launchaa hyvin, pitää siis saada joku esto
            //          Että se ei pysty kiipee ylöspäin, että aina launchaa
            if (Input.GetButton("Horizontal"))
            {
                moveDirection.x = Input.GetAxis("Horizontal");
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection.x *= speed;
            }
        }


        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.isGrounded && hit.normal.y < 0.01f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                Debug.Log("HIT");
                moveDirection.y = jumpForce;
                moveDirection.x = moveDirection.x * wallJumpSpeed;
            }
            
        }
    }

}
