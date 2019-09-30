using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    public Player PlayerPrefabs;

    public Player LocalPlayer;

    void Awake()
    {
        if(!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene("Menu Scene");
            return;
        }
    }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefabs);

    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefabs);
    }
}
