using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField] private InventoryController _inventoryController;
    
    private PhotonView _photonView;
    private bool _isMinePhoton;
    

    public bool IsMinePhoton { get => _isMinePhoton; }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();

        if (_photonView.IsMine)
        {
            _isMinePhoton = true;
        }
        else
        {
            transform.gameObject.SetActive(false);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
