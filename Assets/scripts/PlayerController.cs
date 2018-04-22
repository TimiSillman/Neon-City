using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Player Movement Public Variables
    float speed = 15;
    float jumpSpeed = 20;
    float gravity = 30;
    float walljumpCD = 0.35f;
    Vector3 moveDirection;
    Vector3 lastDirection;
    bool wallJumped;
    CharacterController cc;

    //Player Actions Public Variables
    public Transform firePoint;
    public Weapon weapon;
    public float weaponAmmo;
    public Transform weaponLocation;

    //Player Actions Private Variables
    public bool fireRateCooldown = false;

    private PhotonView photonView;

    // Use this for initialization
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        cc = this.GetComponent<CharacterController>();
    }

    void Update()
    {

        Movement();

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);

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
        cc.Move(moveDirection * Time.deltaTime);


        if ((cc.collisionFlags & CollisionFlags.Below) != 0)
        {
            wallJumped = false;

            moveDirection = new Vector3(Input.GetAxis("Horizontal") * speed, 0.0f);
            moveDirection.y -= gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpSpeed;

            }

        }
        else
        {
            if (wallJumped == false)
            {

                if (Input.GetAxisRaw("Horizontal") == 0)
                {
                    moveDirection.x = lastDirection.x;
                }
                else
                {
                    moveDirection.x = Input.GetAxis("Horizontal") * speed;
                }
            }

            if (cc.velocity.y <= 0)
            {
                moveDirection.y -= gravity * Time.deltaTime * 1.8f;
            }
            else
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }


        }

        lastDirection = cc.velocity;

    }

    [PunRPC]
    public void Fire()
    {
        if (weaponAmmo > 0)
        {
           if (this.GetComponentInChildren<WeaponDestroy>().gameObject.transform.Find("muzzle"))
            {
                this.GetComponentInChildren<WeaponDestroy>().gameObject.transform.Find("muzzle").transform.gameObject.SetActive(true);
            }

            string help = weapon.bullet.name;
            Instantiate(Resources.Load(help), firePoint.position, firePoint.rotation);
            
            if (this.photonView.isMine)
            {
                this.photonView.RPC("Fire", PhotonTargets.OthersBuffered);
            }

            weaponAmmo -= 1;
            if (this.GetComponentInChildren<WeaponDestroy>().tag != "Grenade")
            {
                this.GetComponentInChildren<AudioSource>().Play();
            }
            StartCoroutine(fireRateCD(weapon.fireRate));
            StartCoroutine(Muzzle());


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
            if (Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Horizontal") == -1)
                moveDirection.y *= 0.5f;
            if (Input.GetButtonDown("Jump"))
            {
                wallJumped = true;
                StartCoroutine(wait());
                moveDirection.y = jumpSpeed;
                moveDirection.x = speed * hit.normal.x;
            }
        }
    }


    IEnumerator fireRateCD(float amount)
    {

        fireRateCooldown = true;
        yield return new WaitForSeconds(amount);
        fireRateCooldown = false;
    }

    IEnumerator Muzzle()
    {
        yield return new WaitForSeconds(0.05f);
        if (this.GetComponentInChildren<WeaponDestroy>().gameObject.transform.Find("muzzle"))
        {
            this.GetComponentInChildren<WeaponDestroy>().gameObject.transform.Find("muzzle").transform.gameObject.SetActive(false);
        }
    }

    IEnumerator wait()
    {
        
        yield return new WaitForSeconds(walljumpCD);
        wallJumped = false;
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
        if (wep.tag != "Grenade")
        {
            wep.GetComponent<Rigidbody>().isKinematic = false;
            wep.GetComponent<Rigidbody>().useGravity = true;
            wep.GetComponent<BoxCollider>().enabled = true;
            wep.transform.parent = null;
        } else
        {
            Destroy(wep);
        }
    }

}
