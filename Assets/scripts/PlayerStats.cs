using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float health = 100f;
    public GameObject Ragdoll;

    bool isImmune = false;
    bool ragdolled = false;

    public GameObject lobbyCam;

    PlayerController PC;

	// Use this for initialization
	void Start () {
        if (this.GetComponent<PlayerController>())
        {
            PC = this.GetComponent<PlayerController>();
        }

        FindObjectOfType<AudioListener>().enabled = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (lobbyCam == null)
        {
            lobbyCam = FindObjectOfType<PhotonNetworkManager>().lobbyCamera;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ElectricField" && !isImmune)
        {
            StartCoroutine(immune(3));
            TakeDamage(20);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "ElectricField" && !isImmune)
        {
            StartCoroutine(immune(3));
            TakeDamage(20);
        }
    }

    public void TakeDamage(int amount)
    {


        health -= amount;
        if (health < 0)
        {
            Die();
        }
       
    }

    public void Die()
    {
        if (!ragdolled)
        {
            Instantiate(Ragdoll, this.transform.position, this.transform.rotation);
            ragdolled = true;
        }

        FindObjectOfType<AudioListener>().enabled = true;
        this.GetComponentInChildren<Camera>().enabled = false;
        lobbyCam.SetActive(true);

        PhotonNetwork.Destroy(this.gameObject);
    }

    IEnumerator immune(float time)
    {
        isImmune = true;
        yield return new WaitForSeconds(time);
        isImmune = false;
    }
    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(health);

        } else if (stream.isReading)
        {
            health = (float)stream.ReceiveNext();
        }
    }

}
