using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {


    public float pickupRadius = 1.5f;
    public Transform interactionTransform;
    public GameObject collidingPlayer;

    bool hasInteracted = false;

    public virtual void Interact()
    {
        Debug.Log("Interacting with: " + transform.name);
    }


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "player" && !hasInteracted)
        {
            collidingPlayer = col.gameObject;
            Interact();
            hasInteracted = true;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
        {
            interactionTransform = transform;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, pickupRadius);
    }
}
