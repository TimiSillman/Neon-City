using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonNetworkManager : Photon.MonoBehaviour
{
    [SerializeField]
    private Text connectText;
    [SerializeField]
    private GameObject player;

    public GameObject lobbyCamera;
    [SerializeField]
    private Transform[] spawnPoints;

    public GameObject electro;
    public GameObject slidingDoor;

    private int players;

    public GameObject panel;

    public Text Host;

    public Text error;

    bool GameStarted = false;

    public Text Join;

    // Use this for initialization
    void Start ()
    {
        string userId = Random.Range(1, 999).ToString();
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.UserId = userId;
        PhotonNetwork.ConnectUsingSettings("asd");
	}

    public virtual void OnJoinedRoom()
    {
        int help = Random.Range(0, spawnPoints.Length);
        PhotonNetwork.Instantiate(player.name, spawnPoints[help].position, spawnPoints[help].rotation, 0);
        Destroy(spawnPoints[help].gameObject);
        lobbyCamera.SetActive(false);
        panel.SetActive(false);
    }

	// Update is called once per frame
	void Update ()
    {
        OnTwoPlayerEnter();
        if (players >= 2)
        {
            lobbyCamera.SetActive(false);
            electro.GetComponent<ElectricFieldController>().enabled = true;
            slidingDoor.GetComponent<PlatformMove>().enabled = true;
            GameStarted = true;
        }

        if (GameStarted && players < 2)
        {
            Application.LoadLevel(Application.loadedLevel);
        }


        connectText.text = PhotonNetwork.connectionStateDetailed.ToString();
	}


    public void HostGame()
    {
        RoomOptions RO = new RoomOptions()
        {
            isVisible = true,
            isOpen = true,
            maxPlayers = 4
        };

        if(PhotonNetwork.CreateRoom(Host.text, RO, null))
        {
            StartCoroutine(waitHost());
        }
    }

    public void JoinGame()
    {
        if (PhotonNetwork.JoinRoom(Join.text))
        {
            StartCoroutine(waitJoin());
        }
    }

    IEnumerator waitJoin()
    {
        yield return new WaitForSeconds(2);
        error.text = "Cannot join room, room is full";
    }

    IEnumerator waitHost()
    {
        yield return new WaitForSeconds(2);
        error.text = "Cannot host room, room name already exists";
    }

    void OnTwoPlayerEnter()
    {
        players = FindObjectsOfType<PlayerController>().Length;
        Debug.Log(players);
    }
}

