﻿using System.Collections;
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

    //Player Actions Public Variables
    public Transform firePoint;
    public Weapon weapon;
    public float weaponAmmo;
    public Transform weaponLocation;

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            RemoveWeapon();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Movement();
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
        if (weaponAmmo > 0)
        {
            var bullet = Instantiate(weapon.bullet, firePoint.position, firePoint.rotation);
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weapon.projectileSpeed;

            weaponAmmo -= 1;
            StartCoroutine(fireRateCD(weapon.fireRate));
        } else
        {
            RemoveWeapon();
        }
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Gets the Vector value for the hitpoint

        if (!cc.isGrounded && hit.normal.y < 0.1f)
        {
            if (Input.GetButtonDown("Jump"))
            {

                //LISÄÄ TÄHÄN VITUNMOINEN FORCE
                Debug.Log("HIT");
                playerVector.y = jumpForce;
                playerVector = Vector3.forward * airSpeed * hit.normal.x;
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
        climbedUp = true;
        yield return new WaitForSeconds(wallClimbCD);
        climbedUp = false;
    }


    public void addWeaponToHand()
    {
        var newWeapon = Instantiate(weapon.mesh, weaponLocation.position, weaponLocation.rotation);

        if (this.gameObject.transform.localScale.x <= 0f)
        {
            newWeapon.transform.localScale = new Vector3(newWeapon.transform.localScale.x * -1, newWeapon.transform.localScale.y, newWeapon.transform.localScale.z);
        }

        newWeapon.transform.parent = weaponLocation;
    }

    public void RemoveWeapon()
    {
        weapon = null;
        GameObject wep = GetComponentInChildren<WeaponDestroy>().gameObject;
        wep.GetComponent<Rigidbody>().isKinematic = false;
        wep.GetComponent<Rigidbody>().useGravity = true;
        wep.GetComponent<BoxCollider>().enabled = true;
        wep.transform.parent = null;
    }
}
