using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 포톤 네트워크를 사용
using Photon.Realtime;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // 방의 모든 클라이언트가 마스터 클라이언트와 동일한 레벨을 로드해야하는지 여부를 정의
    }

    private void Start()
    {
        PhotonNetwork.GameVersion = "1.0";
        PhotonNetwork.ConnectUsingSettings();
    }

}
