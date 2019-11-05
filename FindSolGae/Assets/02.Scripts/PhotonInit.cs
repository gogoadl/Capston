using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Solgae.FindSolgae
{
    
    // 게임 메뉴 씬 내에 Reporter 오브젝트는 빌드하여 실행한 클라이언트의 로그 내용을 확인하기 위해 가져왔습니다.
    // 사용법 : 빌드하여 실행한 게임 안에서 클릭한 상태로 원을 그려주면 됨


    public class PhotonInit : MonoBehaviourPunCallbacks
    {

        public Button BtnConnectMaster;
        public Button BtnConnectRoom;

        public bool TriesToConnectToMaster;
        public bool TriesToConnectToRoom;

        public static Text StateText; // 게임 네트워크 상태를 보여주는 텍스트
       

        void Start()
        {
            StateText = GameObject.FindGameObjectWithTag("StateText").GetComponent<Text>();
            DontDestroyOnLoad(StateText);
            //DontDestroyOnLoad(this); // 씬이 바뀌어도 해당 게임오브젝트는 파괴되지 않고 남긴다. 
            //DontDestroyOnLoad를 주석처리 해도 PhotonMono라는 오브젝트가 자동으로 DontDestroyOnLoad에 생성되어 있었음
            TriesToConnectToMaster = false;
            TriesToConnectToRoom = false;

        }
        void Update()
        {
            if (BtnConnectMaster != null)
                BtnConnectMaster.gameObject.SetActive(!PhotonNetwork.IsConnected && !TriesToConnectToMaster);
            if (BtnConnectRoom != null)
                BtnConnectRoom.gameObject.SetActive(PhotonNetwork.IsConnected && !TriesToConnectToMaster && !TriesToConnectToRoom);
        }
        public void OnClickConnectToMaster()
        {
            //Settings (all optional and only for tutorial purpose)

            //PhotonNetwork.OfflineMode = false; // true would "fake" an online connection
            //PhotonNetwork.NickName = "PlayerName"; // to set a player name
            //PhotonNetwork.AutomaticallySyncScene = true; // to call photonnetwork.loadlevel()
            //PhotonNetwork.GameVersion = "v1"; // only people with the same game version can play together

            TriesToConnectToMaster = true;
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
            StateText.text = "마스터서버에 접속하고 있습니다.";
            Debug.Log("Connected to Master!");
        }
        public void OnClickConnectToRoom()
        {
            if (!PhotonNetwork.IsConnected)
                return;
            Debug.Log(PhotonNetwork.IsConnected);
            TriesToConnectToRoom = true;


            PhotonNetwork.JoinRandomRoom(); 
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            StateText.text = "방에 접속하고 있습니다.";
            TriesToConnectToRoom = false;
            PhotonNetwork.IsMessageQueueRunning = false;
            Debug.Log("Master : " + PhotonNetwork.IsMasterClient + " | Players In Room :" + PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Main Scene"); // Scene Manager 를 통해 불러 올 경우 같은 씬을 로드하지 않음. 
                                                   // (플레이어가 입장해도 보여지지 않음)
                                                   //SceneManager.LoadScene("Main Scene");

            Debug.Log("메인씬으로 이동");
        }
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log(otherPlayer.NickName + "님이 나갔습니다.");
        }
        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            Debug.Log("누가 방을 나갔습니다.");
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient) 
        {   // 마스터클라이언트가 바뀔 때 호출됨. 나중에 방장나가면 글자띄울때 쓸 예정
            base.OnMasterClientSwitched(newMasterClient);
            Debug.Log(newMasterClient);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {   // 방 입장에 실패할 경우 새로운 방을 만든다.
            base.OnJoinRandomFailed(returnCode, message);
            StateText.text = "방 입장에 실패했습니다.";
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
            Debug.Log("onJoinRandomFailed");
        }

        public override void OnCreatedRoom()
        {   // 새로운 방을 생성할 때 호출
            base.OnCreatedRoom();
            StateText.text = "새로운 방을 생성 중입니다.";
            Debug.Log("OnCreateRoom");
        }

      
    }
}
