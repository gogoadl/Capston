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

        private void Start() // 초기화 함수
        {
            
            Physics.gravity = new Vector3(0, -15.5f, 0); // 중력가속도를 15.5로 적용

            animator = GetComponent<Animator>(); // 애니메이터 컴포넌트를 가져온다
                                                 // (애니메이터에 있는 변수들을 사용하기 위해) ex) isWalk = true 등등
            InitAnimatorVariable(); // 애니메이터 변수 초기화

            if (photonView.IsMine) // 플레이어가 내 것 일경우 
            {
                Camera.main.gameObject.AddComponent<PlayerCamera>(); 
                PlayerCamera p = Camera.main.GetComponent<PlayerCamera>();
                //p.playerTransform = this.target;
                //Destroy(CharacterCamera);
            }
           
        }

        void Update()
        {
           
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {   // 포톤뷰가 내 것이 아닐경우 
                return;
            }

            Rotation();
            Attack();

            if (animator.GetBool("isJump")) // 점프 중일때 공격 불가능 하게 함
            {                               // (점프공격 애니메이션이 없음..)
                animator.SetBool("isAttack", false);
            }

            if (animator.GetBool("isGrounded")) // 플레이어 인스턴스가 바닥에 닿아 있으면 점프 가능
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (jumpCount == 1)     // 점프가 중복되는 것을 방지하기 위해 점프 카운트를 둔다
                        animator.SetBool("isJump", true);
                    animator.SetBool("isGrounded", false);
                    jumpCount = 0;
                    rigidbody.AddForce(Vector3.up * 6.5f, ForceMode.Impulse); // Impulse 방식으로 위쪽을 향해 힘을 가해준다.

                }
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {   // 플레이어 이동 부분
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    animator.SetBool("isWalk", false);
                    animator.SetBool("isRun", true);
                    animator.SetFloat("Speed", 35.0f);
                    this.transform.Translate(Vector3.forward * animator.GetFloat("Speed") * Time.deltaTime);
                }
                else
                {
                    animator.SetFloat("Speed", 15.0f);
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

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.tag == "Ground")
            {
                jumpCount = 1;
                animator.SetBool("isGrounded", true);
                animator.SetBool("isJump", false);
            }
        }


        void Rotation() // 플레이어 인스턴스의 방향을 설정
        {
            float getX = Input.GetAxis("Horizontal");
            float getY = Input.GetAxis("Vertical");

            lookDirection.Set(getX, 0, getY);   

            if (lookDirection != Vector3.zero)
            {
                Quaternion q = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(lookDirection), 7 * Time.deltaTime);
                transform.rotation = q;
            }
        }
        
        void Attack()
        {
            GameObject child = GameObject.FindWithTag("PlayerAttack");
            Collider c = child.GetComponent<BoxCollider>(); // 주먹에 있는 Box Collider 를 Get 함
            if (!animator.GetBool("isRun"))
            {
                if (Input.GetMouseButton(0))
                {
                    animator.SetBool("isAttack", true);
                    c.enabled = true; // 공격 했을 때 Collider의 상태를 true로 바꿈(충돌 처리가 되도록)
                }
                else
                {
                    animator.SetBool("isAttack", false);
                    c.enabled = false;
                }
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
    }
}