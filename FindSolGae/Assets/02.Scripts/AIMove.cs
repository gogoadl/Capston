using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMove : MonoBehaviour
{
    Transform AITransform;

    Animator AIanimator;

    Vector3 lookDirection;

    int state;
    // Start is called before the first frame update
    void Start()
    {
        AIanimator = GetComponent<Animator>();

        AITransform = GetComponent<Transform>();

        AIanimator.SetBool("isWalk", false);
        AIanimator.SetBool("isRun", false);
        AIanimator.SetBool("isJump", false);
        AIanimator.SetBool("isGrounded", true);

        state = (int)Random.Range(1, 4);


        InvokeRepeating("RandomState", state, 1);
        

    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case 1:
                {
                    
                    AIanimator.SetFloat("Speed", 15.0f);
                    AIanimator.SetBool("isRun", false);
                    AIanimator.SetBool("isWalk", true);
                    this.transform.Translate(Vector3.forward * AIanimator.GetFloat("Speed") * Time.deltaTime);
                    break;
                }
            case 2:
                {
                    AIanimator.SetBool("isRun", false);
                    AIanimator.SetBool("isWalk", false);
                    
                    break;
                }
            case 3:
                {
                    
                    AIanimator.SetFloat("Speed", 25.0f);
                    AIanimator.SetBool("isRun", true);
                    this.transform.Translate(Vector3.forward * AIanimator.GetFloat("Speed") * Time.deltaTime);
                    
                    break;
                }
        }

        

    }
    void RandomState()
    {
        state = (int)Random.Range(1, 4);
        Rotation();
        
    }


    void Rotation()
    {
        lookDirection.Set(Random.Range(-360, 360),0 , Random.Range(-360, 360));   // 벡터 셋팅.
        //Quaternion q = Quaternion.LookRotation(target.TransformDirection(lookDirection));  // 회전
        Quaternion q = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(lookDirection), 15f * Time.deltaTime);
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(lookDirection), 300 * Time.deltaTime);


        if (lookDirection != Vector3.zero)
            transform.rotation = q;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            
            AIanimator.SetBool("isGrounded", true);
            AIanimator.SetBool("isJump", false);
        }
    }
}
