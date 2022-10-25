using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        ConnectToServer();
    }

    private void Start()
    {
        // ConnectToServer();
    }

    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        print("Connecting... ");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom("Room_1", roomOptions, TypedLobby.Default);

        print("Connect To Room...");
    }
    
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        print("Joined...");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        print("New Player Joined...");
        // OnPlayerFinishedConnect?.Invoke();
        base.OnPlayerEnteredRoom(newPlayer);
        
    }
}
