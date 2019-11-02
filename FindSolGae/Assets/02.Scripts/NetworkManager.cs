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
            Debug.Log("싱글톤 생성");
            if (instance != this)
            {
                Destroy(gameObject);
                Debug.Log("싱글톤 파괴");
            }
        }
        private void Start() 
        {
            
            PhotonNetwork.Instantiate("player", new Vector3(0,5,0), Quaternion.identity);
            Debug.Log("플레이어 인스턴스 생성");
        }

        
    }
}