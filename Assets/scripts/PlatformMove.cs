using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMove : MonoBehaviour {

    public int waitTime = 30;

    Rigidbody rb;
    bool move = false;

	// Use this for initialization
	void Start () {
        StartCoroutine(Wait());
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (move && this.transform.position.x <= 19)
        {
            rb.MovePosition(transform.position + transform.forward * Time.deltaTime * 2);
        }
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
        move = true;
    }
}
