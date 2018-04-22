using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricFieldController : MonoBehaviour {

    //Public variables
    public float startUpTime = 2f;
    public int[] StopPoints;


    //private variables
    bool startRaising = false;
    bool isWaiting = false;
    int currentStopPoint = 0;



    // Use this for initialization
    void Start () {
        StartCoroutine(Wait());
    }
	
	// Update is called once per frame
	void FixedUpdate () {

		if (startRaising)
        {
            Vector3 moveUp = new Vector3(0, 0.01f, 0);
            this.gameObject.transform.position += moveUp;

            if (currentStopPoint < StopPoints.Length)
            {
                //When the electric field reaches a specific Y position, defined the inspector, for each stopping point
                //The field stops for a specific time and then continues to rise
                if (gameObject.transform.position.y >= StopPoints[currentStopPoint])
                {
                    StartCoroutine(Wait());
                    currentStopPoint++;
                }
            }
        }

    }

    IEnumerator Wait()
    {
        startRaising = false;
        yield return new WaitForSeconds(startUpTime);
        startRaising = true;
    }
}
