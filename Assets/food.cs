using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food : MonoBehaviour
{

    float randompositionx;
    float randompositionz;
    Vector3 position;

    void Start()
    {
        Gamemanager.instance.AddFood(this);


        Vector3 position = new Vector3(Random.Range(-Gamemanager.instance.width, Gamemanager.instance.width), 0, Random.Range(-Gamemanager.instance.height, Gamemanager.instance.height));
        transform.position = position;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            Vector3 position = new Vector3(Random.Range(-Gamemanager.instance.width, Gamemanager.instance.width), 0, Random.Range(-Gamemanager.instance.height, Gamemanager.instance.height));
            transform.position = position;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Vector3 position = new Vector3(Random.Range(-Gamemanager.instance.width, Gamemanager.instance.width), 0, Random.Range(-Gamemanager.instance.height, Gamemanager.instance.height));
    //        transform.position = position;
    //    }
    //}

}
