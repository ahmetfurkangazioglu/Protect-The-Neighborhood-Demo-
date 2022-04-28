using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M24Bomb : MonoBehaviour
{
    [SerializeField] public float Radius;
    [SerializeField] public float ExplosionForce;
    [SerializeField] public GameObject BombEffect;
   
  
    private void OnCollisionEnter(Collision collision)
    {
        Explosion();       
        Destroy(gameObject);
    }

    void Explosion()
    {
       
        Vector3 Position = transform.position;
        Collider[] collider = Physics.OverlapSphere(Position, Radius);
        GameObject bombEffect = Instantiate(BombEffect, Position, gameObject.transform.rotation);

      Destroy(bombEffect,4f);
              
        foreach (Collider hit in collider)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (hit!=null && rb)
            {
                rb.AddExplosionForce(ExplosionForce, Position, Radius,0f,ForceMode.Impulse);
            }
            if (hit.gameObject.CompareTag("Enemy"))
            {
              hit.transform.gameObject.GetComponent<Enemy>().Dead();
            }
        }
       
    }


}
