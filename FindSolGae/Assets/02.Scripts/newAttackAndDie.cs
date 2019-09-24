using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newAttackAndDie : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(other);
            Animator AIAnimator = other.GetComponent<Animator>();
            AIMove aimove = other.GetComponent<AIMove>();
            aimove.isDie = true;
            AIAnimator.SetBool("isDie", true);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.collider);
            Animator AIAnimator = collision.collider.GetComponent<Animator>();
            AIMove aimove = collision.collider.GetComponent<AIMove>();
            aimove.isDie = true;
            AIAnimator.SetBool("isDie", true);
        }
    }
}