using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSettings : MonoBehaviour
{
    Rigidbody rb;
    GameObject GameControl;

    void Start()
    {
        GameControl = GameObject.FindWithTag("MainCamera");
      rb =GetComponent<Rigidbody>();
        rb.velocity = GameControl.transform.forward *200f;
        Destroy(gameObject, 2f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);    
    }

}
