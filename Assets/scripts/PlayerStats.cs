using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float heath = 100f;


    PlayerController PC;

	// Use this for initialization
	void Start () {
        if (this.GetComponent<PlayerController>())
        {
            PC = this.GetComponent<PlayerController>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
