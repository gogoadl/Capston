﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] // Inspector 창에서 보이게 함
    Camera CharacterCamera; // 카메라 객체 선언
   
    public Transform target; // 오브젝트의 위치정보를 저장하는 객체 선언

    public Animator animator; // 애니메이터 객체 선언

    public Vector3 lookDirection; // x,y,z 좌표를 가지는 객체 선언

    public Rigidbody rigidbody; // 오브젝트의 물리엔진 정보를 가지는 객체 선언

    int jumpCount; // 공중에서 연속해서 점프를 할 수 없도록 점프카운트 사용

    bool isGround; // 현재 플레이어의 위치가 바닥에 닿아 있는지 확인
    
    public float dist = 5f; // 카메라와 오브젝트 사이의 거리

    public float xSpeed = 220.0f; // 카메라가 x축으로 이동하는 속도
    public float ySpeed = 100.0f; // 카메라가 y축으로 이동하는 속도

    private float x = 0.0f;
    private float y = 0.0f;

    public float yMinLimit = 0f;  // -20
    public float yMaxLimit = 0f;  // 80



    float clampAngle(float angle, float min, float max)
    {
        if(angle < -360)
        {
            angle += 360;
        }

        if(angle > 360)
        {
            angle -= 360;
        }
        return Mathf.Clamp(angle, min, max);  // Angle 의 최소값과 최대값을 리턴
    }

    private void Start() // 초기화 함수
    {

        Physics.gravity = new Vector3(0, -15.5f, 0); // 중력가속도를 16.5로 적용

        rigidbody = GetComponent<Rigidbody>();

        animator = GetComponent<Animator>();
        
        jumpCount = 1;

        isGround = true;

        Vector3 angles = CharacterCamera.transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        animator.SetBool("isWalk", false);
        animator.SetBool("isRun", false);
        animator.SetBool("isJump", false);
        animator.SetBool("isGrounded", true);
        animator.SetBool("isAttack", false);
        animator.SetBool("isDie", false);
    }

    private void LateUpdate()
    {
        if(target)
        {
            if(Input.GetKey(KeyCode.LeftAlt))
            {

            
            dist -= 5f * Input.mouseScrollDelta.y;

            if(dist < 0.5)
            {
                dist = 1;
            }
            if(dist >= 100)
            {
                dist = 100;
            }

            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

            y = clampAngle(y, yMinLimit, yMaxLimit);  // 카메라 앵글이 적정하도록 조정
                

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            
            Vector3 position = rotation * new Vector3(0, 0.0f, -dist) + target.position + new Vector3(0.0f, 10f, 0.0f);

            CharacterCamera.transform.rotation = rotation;
            CharacterCamera.transform.position = position;
            }
        }
    }

    void Update()
    {
        Rotation();
        Attack();

        if(animator.GetBool("isJump"))
        {
            animator.SetBool("isAttack", false);
        }

        if (animator.GetBool("isGrounded"))
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpCount == 1)
                    animator.SetBool("isJump", true);
                animator.SetBool("isGrounded", false);
                jumpCount = 0;
                rigidbody.AddForce(Vector3.up * 6.5f, ForceMode.Impulse);

            }
        }
        
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isWalk", false);
                animator.SetBool("isRun", true);
                animator.SetFloat("Speed", 25.0f);
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
    void Rotation()
    {
        // 마우스를 이용해 조종
        float getX = Input.GetAxis("Horizontal");
        float getY = Input.GetAxis("Vertical");


        lookDirection.Set(getX, 0, getY);   // 벡터 셋팅.

        if (lookDirection != Vector3.zero)
        {
            Quaternion q = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(lookDirection), 7 * Time.deltaTime);
            transform.rotation = q;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            jumpCount = 1;
            animator.SetBool("isGrounded", true);
            animator.SetBool("isJump", false);
        }

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.collider);
        }
    }

    void Attack()
    {
        if(Input.GetMouseButton(0))
        {
            animator.SetBool("isAttack", true);
        }
        else
        {
            animator.SetBool("isAttack", false);
        }
    }
}
