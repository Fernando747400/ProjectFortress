using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    private PhotonView _photonView;
    void Start()
    {
        _photonView = GetComponent<PhotonView>();

        if (_photonView.IsMine)
        {
            Debug.Log(transform.name + "Photon");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
