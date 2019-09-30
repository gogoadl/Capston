using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 포톤 네트워크를 사용
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhotonInit : MonoBehaviourPunCallbacks
{

    public Button BtnConnectMaster;
    public Button BtnConnectRoom;

    public bool TriesToConnectToMaster;
    public bool TriesToConnectToRoom;

    //void Awake()
    //{
    //    PhotonNetwork.AutomaticallySyncScene = true; // 방의 모든 클라이언트가 마스터 클라이언트와 동일한 레벨을 로드해야하는지 여부를 정의
    //}

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
    }
    void Update()
    {
        if(BtnConnectMaster != null)
            BtnConnectMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !TriesToConnectToMaster);
        if(BtnConnectRoom != null)
            BtnConnectRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && !TriesToConnectToRoom);
    }
    public void OnClickConnectToMaster()
    {
        //Settings (all optional and only for tutorial purpose)

        PhotonNetwork.OfflineMode = false; // true would "fake" an online connection
        PhotonNetwork.NickName = "PlayerName"; // to set a player name
        PhotonNetwork.AutomaticallySyncScene = true; // to call photonnetwork.loadlevel()
        PhotonNetwork.GameVersion = "v1"; // only people with the same game version can play together

        TriesToConnectToMaster = true;
        //PhotonNetwork.ConnectToMaster(ip.port.appid) // manual connection
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        TriesToConnectToMaster = false;
        TriesToConnectToRoom = false;
        Debug.Log(cause);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        TriesToConnectToMaster = false;
        Debug.Log("Connected to Master!");
    }
    public void OnClickConnectToRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        Debug.Log(PhotonNetwork.IsConnected);
        TriesToConnectToRoom = true;
        //PhotonNetwork.CreateRoom("HW's Game 1"); // Create a specific Room - error : OnCreateRoomFailed
        //PhotonNetwork.JoinRoom("HW's Game 1"); // Join a specific Room - error : OnJoinRoomFailed
        //PhotonNetwork.JoinRandomRoom(); // Join a random Room - error : OnJoinRandomRoomFailed
        PhotonNetwork.JoinOrCreateRoom("HW's Game1", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        TriesToConnectToRoom = false;
        Debug.Log("Master : " + PhotonNetwork.IsMasterClient + " | Players In Room :" + PhotonNetwork.CurrentRoom.PlayerCount);
        SceneManager.LoadScene("Main Scene"); 
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        // no room available
        // create a room (null as a name means "does not matter")

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log(message);
        TriesToConnectToRoom = false;
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }


}
