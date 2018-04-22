
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    public float timeUntilExplode;
    public float damage;
    public int radius;
    public ParticleSystem PS;
    public float power = 10f; 


    // Use this for initialization
    void Start () {
        StartCoroutine(WaitForExplosion());
	}


    IEnumerator WaitForExplosion()
    {
        yield return new WaitForSeconds(timeUntilExplode);
        Instantiate(PS, transform.position, Quaternion.identity);
        Explode();
    }

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] objectsInRange = Physics.OverlapSphere(transform.position, radius);
        
        foreach (Collider col in objectsInRange)
        {
            PlayerController player = col.GetComponent<PlayerController>();
            if (player != null)
            {
                //linear falloff of effect
                float proximity = (this.gameObject.transform.position - player.transform.position).magnitude;
                float effect = 1 - (proximity / radius);
                int help = Mathf.RoundToInt((damage * effect) / 2);
                player.GetComponent<PlayerStats>().TakeDamage(help);
            }

            Rigidbody rb = col.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius * 2, 3.0F);
        }

        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
