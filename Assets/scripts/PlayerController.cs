using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player Movement Public Variables
    public float groundSpeed = 10;
    public float airSpeed = 10;
    public float gravityStrength = 40;
    public float jumpForce = 20;
    public float wallClimbCD = 2;

    //Player movement private variables
    bool canJump = false;
    float verticalVelocity;
    public Vector3 velocity;
    public Vector3 playerVector;
    bool onWall = false;
    bool climbedUp = false;
    CharacterController cc;
    public float speed = 10.0f;
    public float jumpSpeed = 20.0f;
    public float gravity = 20.0f;
    public float jumpCount = 0.0f;
    float maxJump = 2.0f;
    Vector3 lastMove;

    bool sliding = false;
    bool wallJumped = false;
    public Vector3 moveDirection = Vector3.zero;

    //Player Actions Public Variables
    public Transform firePoint;
    public Weapon weapon;
    public float wallJumpCD = 0.5f;

    //Player Actions Private Variables
    public bool fireRateCooldown = false;


    // Use this for initialization
    void Start()
    {
        cc = this.GetComponent<CharacterController>();
    }

    void Update()
    {
        if (weapon != null && !fireRateCooldown)
        {
            if (weapon.isRapidFire)
            {
                if (Input.GetButton("Fire1"))
                {
                    Fire();
                }
            }
            else if (!weapon.isRapidFire)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Fire();
                }
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Movement1();
        LookAtMouse();
    }

    void LookAtMouse()
    {
        var mouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        var playerScreenPoint = Camera.main.WorldToScreenPoint(this.transform.position);
        if (mouse.x < playerScreenPoint.x)
        {
            //mouse is on the left side of the player
            this.gameObject.transform.localScale = new Vector3(-1,1,1);
        }
        else if (mouse.x > playerScreenPoint.x)
        {
            //mouse is on the right side of the player
            this.gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    void Movement1()
    {
        cc.Move(moveDirection * Time.deltaTime);


        if ((cc.collisionFlags & CollisionFlags.Below) != 0)
        {
            wallJumped = false;
            moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, 0.0f);
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;

            }
          moveDirection.y -= gravity * Time.deltaTime * 10f;
        }

        else
        {
            if (wallJumped == false)
            {
                moveDirection.x = Input.GetAxis("Horizontal") * speed;
            }
            if (cc.velocity.y < 0)
            {
                moveDirection.y -= gravity * Time.deltaTime * 1.8f;
            }
            else
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }


        }




    }
    void Movement()
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
            playerVector = input;
            playerVector *= airSpeed;
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
            playerVector = Vector3.ProjectOnPlane(velocity, Vector3.up);
            canJump = true;

            verticalVelocity = -3f;

            onWall = false;
        }
        else if ((flags & CollisionFlags.Sides) != 0)
        {

            canJump = true;
            onWall = true;
        }
        else if ((flags & CollisionFlags.Above) != 0)
        {
            verticalVelocity = -1.5f;
        }
        else
        {
            canJump = false;
            onWall = false;
        }
    }


    void Fire()
    {
        if (weapon.ammo > 0)
        {
            var bullet = Instantiate(weapon.bullet, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weapon.projectileSpeed;

            weapon.ammo -= 1;
            fireRateCD(weapon.fireRate);

        } else
        {
            //throw the weapon to hell
            weapon = null;
        }

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!cc.isGrounded && hit.normal.y < 0.1f)
        {
            if(Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
            moveDirection.y *= 0.5f;
            if (Input.GetButtonDown("Jump"))
            {
                wallJumped = true;
                StartCoroutine(wait());
                moveDirection.y = jumpSpeed;
                moveDirection.x = speed  * hit.normal.x;
            }
        }
    }


    IEnumerator fireRateCD(float amount)
    {
        fireRateCooldown = true;
        yield return new WaitForSeconds(amount);
        fireRateCooldown = false;
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(wallJumpCD);
        wallJumped = false;
    }

}
