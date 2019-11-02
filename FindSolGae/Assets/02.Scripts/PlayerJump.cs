using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviourPunCallbacks
{
    Animator animator;

    Rigidbody rigidbody;

    PhotonView photonView;

    int jumpCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponentInParent<PhotonView>(); // 상위 오브젝트에서 PhotonView 클래스를 얻어온다


        Physics.gravity = new Vector3(0, -20.5f, 0); // 중력가속도를 15.5로 적용

        animator = GetComponentInParent<Animator>();
        if(animator)
        {
            Debug.Log("애니메이터 얻어오기 성공!");
            animator.SetBool("isGrounded", true); // 초기화 안해주니 에러가 뜨는듯>??
            animator.SetBool("isJump", false);
        }

        rigidbody = GetComponentInParent<Rigidbody>();
        if(rigidbody)
        {
            Debug.Log("리짇바디 얻어오기 성공!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        { 
            if (animator.GetBool("isGrounded")) // 플레이어 인스턴스가 바닥에 닿아 있으면 점프 가능
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (jumpCount == 1)
                    {// 점프가 중복되는 것을 방지하기 위해 점프 카운트를 둔다
                        jumpCount = 0;
                        animator.SetBool("isJump", true);
                        animator.SetBool("isGrounded", false);

                        Debug.Log("jumpcount = " + jumpCount);
                        rigidbody.AddForce(Vector3.up * 10.5f, ForceMode.Impulse); // Impulse 방식으로 위쪽을 향해 힘을 가해준다.
                    }
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Ground")
        {
            Debug.Log("바닥과 닿아있음");
            jumpCount = 1;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJump", false);
        }
    }


}
