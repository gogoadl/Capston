using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviourPun
{
    public Animator animator;

    public BoxCollider boxCollider;

    public PhotonView pv;

    float timer = 0.0f;

    float waitingTime = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();

        boxCollider = GetComponent<BoxCollider>();

        pv = GetComponentInParent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine)
            return;
        
        if(animator.GetBool("isDie") == false)
        { 

            if(Input.GetMouseButton(0) && timer < waitingTime)
            {
                timer += Time.deltaTime;
                animator.SetBool("isAttack", true);
                boxCollider.enabled = true;
            }
            else
            {
                timer = 0;
                animator.SetBool("isAttack", false);
                boxCollider.enabled = false;
            }
        }
    }
}
