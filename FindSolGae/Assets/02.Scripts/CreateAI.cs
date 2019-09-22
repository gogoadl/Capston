using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for(int i =0; i<20; i++)
        { 

        GameObject a = Instantiate(GameObject.Find("AI"),new Vector3(Random.Range(-100,100), 2, Random.Range(-100,100)), Random.rotation);
            a.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
            
        }



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
