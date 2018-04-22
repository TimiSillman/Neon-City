﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour
{
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private MonoBehaviour[] disableScripts;
    private PhotonView photonView;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        Initialize();

    }

    private void Initialize()
    {
        if (photonView.isMine)
        {

        }
        else
        {
            playerCamera.SetActive(false);
            foreach (MonoBehaviour m in disableScripts)
            {
                m.enabled = false;
            }

            
        }
    }

}
