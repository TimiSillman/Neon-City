using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class PhotonNetworkManager : Photon.MonoBehaviour {

    [SerializeField] private Text connectText;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lobbyCamera;
    [SerializeField] private Transform spawnPoint;
    private string userId;
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    void RandomString()
    {
        int charAmount = Random.Range(5, 10); //set those to the minimum and maximum length of your string
        for (int i = 0; i < charAmount; i++)
        {
            userId += glyphs[Random.Range(0, glyphs.Length)];
        }
    }
    // Use this for initialization
    void Start () {
        RandomString();
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.UserId = userId;
        PhotonNetwork.ConnectUsingSettings("test");
	}


    public virtual void OnJoinedLobby()
    {
        Debug.Log("CONNECTED TO LOBBY");
        PhotonNetwork.JoinOrCreateRoom("yes", null, null);
    }


    public virtual void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation, 0);
        lobbyCamera.SetActive(false);
    }


	// Update is called once per frame
	void Update () {
        //TESTING ONLY
        Debug.Log(PhotonNetwork.GetPing());
        connectText.text = PhotonNetwork.connectionStateDetailed.ToString();
	}
}
