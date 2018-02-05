using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : Interactable {

    public Weapon weapon;


    public override void Interact()
    {
        base.Interact();
        PickUp();
    }

    void PickUp()
    {
        Debug.Log("Picking up: " + weapon.name);
        //Add item to players "hands"
        foreach (PlayerController player in FindObjectsOfType<PlayerController>())
        {
            if (player.gameObject == collidingPlayer)
            {
                if (player.weapon != null)
                {
                    player.RemoveWeapon();
                }
                player.weapon = weapon;
                player.weaponAmmo = weapon.ammo;
                player.addWeaponToHand();
            }
        }
        Destroy(gameObject);
    }
}
