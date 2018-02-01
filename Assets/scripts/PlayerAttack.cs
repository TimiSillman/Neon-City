using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    public Transform firePoint;

    public Weapon weapon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (weapon != null)
        {
            if (weapon.isRapidFire)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Fire();
                }
            } else if (!weapon.isRapidFire)
            {
                if (Input.GetButton("Fire1"))
                {
                    Fire();
                }
            }

        }
	}

    void Fire()
    {
        var bullet = Instantiate(weapon.bullet, firePoint.position, firePoint.rotation);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * weapon.projectileSpeed;


    }
}
