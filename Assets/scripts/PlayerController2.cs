using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{

    public float groundSpeed = 10;
    public float airSpeed = 10;
    public float gravityStrength = 40;
    public float jumpForce = 20;
    public float wallClimbCD = 2;

    bool canJump = false;
    float verticalVelocity;
    Vector3 velocity;
    public Vector3 groundedVelocity;
    Vector3 normal;
    Vector3 playerVector;
    bool onWall = false;
    bool climbedUp = false;

    CharacterController cc;

    // Use this for initialization
    void Start()
    {
        cc = this.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        playerVector = Vector3.zero;
        Vector3 input = Vector3.zero;

        //get input
        input.x = Input.GetAxis("Horizontal");
        input = Vector3.ClampMagnitude(input, 1f);

        if (cc.isGrounded)
        {
            //if grounded, get the basic input the player gives with the basic ground speed
            playerVector = input;
            playerVector *= groundSpeed;

        }
        else
        {
            //If in air, the speed is set by airSpeed variable * the Vector 3 input
            playerVector = groundedVelocity;
            playerVector += (input * 3) * airSpeed;
        } 

        //clamp the speed so it does not go over the ground speed level (no BHOP)
        playerVector = Vector3.ClampMagnitude(playerVector, airSpeed);
        playerVector *= Time.deltaTime;



        verticalVelocity -= (gravityStrength * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump"))
        {
            //If touching wall, ability to double jump into an arc


            if (canJump || cc.isGrounded)
            {
                //basic jump
                verticalVelocity = jumpForce;
            }
            if (onWall)
            {
                Vector3 reflection = Vector3.Reflect(velocity, normal);
                Vector3 projected = Vector3.ProjectOnPlane(reflection, Vector3.up);
                groundedVelocity = projected.normalized * airSpeed + normal * airSpeed;

            }
        }


        if (Input.GetButton("Climb") && !climbedUp && onWall)
        {

            verticalVelocity = jumpForce;
            StartCoroutine(wait());
        }
        
        if (cc.velocity.y < 0)
        {
            playerVector.y = verticalVelocity * Time.deltaTime * 1.8f;
        }
        else
        {
            playerVector.y = verticalVelocity * Time.deltaTime;
        }
        //EI VITTU MITÄÄ HAJUA
        CollisionFlags flags = cc.Move(playerVector);
        velocity = playerVector / Time.deltaTime;
        //use of flags to determine if character can jump or not
        //if on ground
        //set canjump to true
        if ((flags & CollisionFlags.Below) != 0)
        {
            groundedVelocity = Vector3.ProjectOnPlane(velocity, Vector3.up);
            canJump = true;

            verticalVelocity = -3f;

            onWall = false;
        }
        else if ((flags & CollisionFlags.Sides) != 0)
        {

            canJump = true;
            onWall = true;
        }else if((flags & CollisionFlags.Above) != 0){
            verticalVelocity = -1.5f;
        }
        else
        {
            canJump = false;
            onWall = false;
        }


    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Gets the Vector value for the hitpoint
        normal = hit.normal;
    }


    IEnumerator wait()
    {
        climbedUp = true;
        yield return new WaitForSeconds(wallClimbCD);
        climbedUp = false;
    }

}
