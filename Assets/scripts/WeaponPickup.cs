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
        foreach (PlayerAttack player in FindObjectsOfType<PlayerAttack>())
        {
            if (player.gameObject == collidingPlayer)
            {
                player.weapon = weapon;
            }
        }
        Destroy(gameObject);
    }
}
