using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Solgae.FindSolgae
{
    public class Solgae : MonoBehaviourPun
    {

        public Animator animator; // 애니메이터 객체 선언

        public int MovementFlag = 0; // 플래그 값에 의해 이동시킴

        public Quaternion q; // 방향 쿼터니언

        PhotonView pv;

        public float invokeRunTime = 1f;

        public float invokeWaitTime = 3f;

        public float getX = 0f;

        public float getZ = 0f;

        public Vector3 lookDirection; // x,y,z 좌표를 가지는 객체 선언

        void Start()
        {
            pv = GetComponent<PhotonView>();

            animator = GetComponentInParent<Animator>();
            // 애니메이터 컴포넌트를 얻는다
            InitAnimatorVariable();
            // 애니메이터 변수 초기화
            InvokeRepeating("RandomMovement", invokeRunTime, invokeWaitTime);
            // RandomMovement 함수를 invokeWaitTime 초마다 invokeRunTime 초만큼 실행
        }

        void Update()
        {
            Rotation();
            if (MovementFlag == 0 || animator.GetBool("isDie"))
                return;

            switch (MovementFlag) // 플래그 값에 의해 움직임이 변경됨
            {

                case 1: // 걷기
                    animator.SetBool("isWalk", true);
                    animator.SetBool("isRun", false);
                    this.transform.rotation = q;
                    this.transform.Translate(Vector3.forward * 10.0f * Time.deltaTime);

                    break;
                case 2: // 뛰기
                    animator.SetBool("isRun", true);
                    animator.SetBool("isWalk", false);
                    this.transform.rotation = q;
                    this.transform.Translate(Vector3.forward * 25.0f * Time.deltaTime);

                    break;
                case 3: // 가만히 서있기
                    animator.SetBool("isWalk", false);
                    animator.SetBool("isRun", false);

                    break;
            }

        }

        void InitAnimatorVariable() // 애니메이터 변수 초기화 
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
            animator.SetBool("isJump", false);
            animator.SetBool("isGrounded", true);
            animator.SetBool("isAttack", false);
            animator.SetBool("isDie", false);
        }

        void Rotation() // 플레이어 인스턴스의 방향을 설정
        {

            lookDirection = getX * Vector3.forward + getZ * Vector3.right;

            if (lookDirection != Vector3.zero)
            {
                q = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(lookDirection), 20f * Time.deltaTime);
            }
        }

        void RandomMovement()
        {

            MovementFlag = Random.Range(1, 4);

            invokeRunTime = Random.Range(0.5f, 1.5f);

            invokeWaitTime = Random.Range(1.0f, 5.0f);

            getX = Random.Range(-1.0f, 1.0f);
            getZ = Random.Range(-1.0f, 1.0f);
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.tag == "PlayerAttack")
            {
                pv.RPC("rpc", RpcTarget.All);

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