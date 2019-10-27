using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
namespace Solgae.FindSolgae
{
    public class NetworkManager : MonoBehaviourPunCallbacks // 싱글톤 패턴으로 하나의 네트워크 매니저를 가지도록 함
    {
        public static NetworkManager instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<NetworkManager>();
                }
                return m_instance;
            }
        }
        private static NetworkManager m_instance;

        public GameObject playerPrefab;


        private void Awake()
        {
            if (instance != this)
            {
                Destroy(gameObject);
                Debug.Log("destroy singleton Object");
            }
        }
        private void Start() // 
        {
            
            PhotonNetwork.Instantiate("player", Vector3.zero, Quaternion.identity);
            Debug.Log("Instantiate Player");
        }
    }
}