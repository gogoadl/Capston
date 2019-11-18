using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Solgae.FindSolgae
{
    public class Player : MonoBehaviourPun
    {

        public Transform target; // 오브젝트의 위치정보를 저장하는 객체 선언

        public Animator animator; // 애니메이터 객체 선언

        public Vector3 lookDirection; // x,y,z 좌표를 가지는 객체 선언

        public Rigidbody rigidbody;

        private int jumpCount = 0;

        string playerList;

        private void Start() // 초기화 함수
        {

            for(int i =0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                playerList += PhotonNetwork.PlayerList[i] + " ";
            }

            Debug.Log(playerList);

            animator = GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져온다
                                                 // (애니메이터에 있는 변수들을 사용하기 위해) ex) isWalk = true 등등
            InitAnimatorVariable(); // 애니메이터 변수 초기화

            if (photonView.IsMine) // 플레이어가 내 것 일경우 
            {
                
                Camera.main.gameObject.AddComponent<PlayerCamera>(); 
                PlayerCamera p = Camera.main.GetComponent<PlayerCamera>();
                p.target = this.target;
                //Destroy(CharacterCamera);
            }
           
        }
        
        void Update()
        {
            if(animator.GetBool("isDie") == true) // 죽으면 못움직임
            {
                return;
            }

            if (photonView.IsMine == false)
            {   // 포톤뷰가 내 것이 아닐경우 
                return;
            }

            Rotation();
            

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {   // 플레이어 이동 부분
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetBool("isWalk", false);
                    animator.SetBool("isRun", true);
                    animator.SetFloat("Speed", 25.0f);
                    this.transform.Translate(Vector3.forward * animator.GetFloat("Speed") * Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Speed", 10.0f);
                    animator.SetBool("isRun", false);
                    animator.SetBool("isWalk", true);
                    this.transform.Translate(Vector3.forward * animator.GetFloat("Speed") * Time.deltaTime);
                }

            }
            else
            {
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", false);
            }

        }

        void Rotation() // 플레이어 인스턴스의 방향을 설정
        {
            float getX = Input.GetAxis("Horizontal");
            float getY = Input.GetAxis("Vertical");

            lookDirection.Set(getX, 0, getY);   

            if (lookDirection != Vector3.zero)
            {
                Quaternion q = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(lookDirection), 15f * Time.deltaTime);
                transform.rotation = q;
            }
        }
    
        void InitAnimatorVariable()
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
            animator.SetBool("isJump", false);
            animator.SetBool("isGrounded", true);
            animator.SetBool("isAttack", false);
            animator.SetBool("isDie", false);
        }
        private void OnTriggerEnter(Collider other)
        {
             
            if (other.gameObject.tag == "PlayerAttack")
            {
                photonView.RPC("rpc", RpcTarget.All);

            }
            
        }

        [PunRPC]
        void rpc()
        {

            InitAnimatorVariable();
            animator.SetBool("isDie", true);
        }
    }
}
