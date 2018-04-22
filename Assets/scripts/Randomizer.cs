using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour {

    public Weapon[] Weapons;

    PhotonView photonView;

	// Use this for initialization
	void Start () {

        photonView = GetComponent<PhotonView>();

        Weapon wep = Weapons[Random.Range(0, Weapons.Length)];

        instantiate(wep);
        
	}

    [PunRPC]
    public void instantiate(Weapon wep)
    {
        this.gameObject.GetComponent<WeaponPickup>().weapon = wep;

        string help = wep.name;

        Instantiate(Resources.Load(help), this.gameObject.transform);

        if (this.photonView.isMine)
        {
            this.photonView.RPC("instantiate", PhotonTargets.OthersBuffered, wep);
        }
    }

}
