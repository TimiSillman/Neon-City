using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public int damage;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<Rigidbody>().velocity = transform.forward * 100f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Bullet" || collision.gameObject.layer.Equals("Ground"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "player")
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals("Ground"))
        {
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "player")
        {
            other.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
