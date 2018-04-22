using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour {

    public Weapon[] Weapons;

	// Use this for initialization
	void Start () {
        Weapon wep = Weapons[Random.Range(0, Weapons.Length)];

        this.gameObject.GetComponent<WeaponPickup>().weapon = wep;

        var weaponSpawn = Instantiate(wep.mesh, this.gameObject.transform);

       
        
        
	}

}
