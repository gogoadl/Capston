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

        public Button btnStart;

        public static Text StateText; // 게임 네트워크 상태를 보여주는 텍스트

        public InputField userNickName; // 플레이어의 닉네임을 텍스트로 받는다

        public GameObject StartPanel; // 게임시작 했을 때 보여줄 화면

        public GameObject LobbyPanel; // 로비로 입장했을 때 화면

        public Text playerNickNameTxt;

        void Start()
        {
            Screen.SetResolution(1280, 800, false); // 게임 화면의 크기를 지정한다. (width, height, fullscreenMode)

            StateText = GameObject.FindGameObjectWithTag("StateText").GetComponent<Text>();

            PhotonNetwork.ConnectUsingSettings(); // OnConnectToMaster 호출한다.

            btnStart.interactable = false;

        }

        public void OnClickStart()
        {
            btnStart.interactable = false;

            PhotonNetwork.NickName = userNickName.text;

            // 마스터 서버에 접속중이라면
            if (PhotonNetwork.IsConnected)
            {
                // 룸 접속 실행
                StateText.text = "룸에 접속...";
                //PhotonNetwork.JoinRandomRoom();

                playerNickNameTxt.text = userNickName.text;
                StartPanel.SetActive(false);
                LobbyPanel.SetActive(true);
            }
            else
            {
                // 마스터 서버에 접속중이 아니라면, 마스터 서버에 접속 시도
                StateText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
                // 마스터 서버로의 재접속 시도
                PhotonNetwork.ConnectUsingSettings();
            }

            
        }

    

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            Debug.Log(cause);
        }

        public override void OnConnectedToMaster()
        {
            
            base.OnConnectedToMaster();
            btnStart.interactable = true;
            
            StateText.text = "마스터서버에 접속했습니다.";
            Debug.Log("Connected to Master!");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            StateText.text = "방에 접속하고 있습니다.";
            PhotonNetwork.IsMessageQueueRunning = false;
            Debug.Log("Master : " + PhotonNetwork.IsMasterClient + " | Players In Room :" + PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Main Scene"); // Scene Manager 를 통해 불러 올 경우 같은 씬을 로드하지 않음. 
                                                   // (플레이어가 입장해도 보여지지 않음)
                                                   //SceneManager.LoadScene("Main Scene");

            Debug.Log("메인씬으로 이동");
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {   // 방 입장에 실패할 경우 새로운 방을 만든다.
            base.OnJoinRandomFailed(returnCode, message);
            StateText.text = "새로운 방을 생성합니다.";
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
