using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent Npc;
    GameObject Target;
    GameObject GameControl;

    public int Health;
    public float Damage;

    Animator animator;
    bool DeadControl = true;
    void Start()
    {
        GameControl = GameObject.FindWithTag("GameMainControl");
        Npc = GetComponent<NavMeshAgent>();
    }

  
    void Update()
    {
        animator = gameObject.GetComponent<Animator>();
        Npc.SetDestination(Target.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("EnemyTarget"))
        {
            if (DeadControl)
            {
                Dead();
                GameControl.GetComponent<GameControl>().HealthControl(Damage);
            }    
        }
        
    }
    public void SetTarget(GameObject Obje)
    {
        Target = Obje;
    }
    public void DamageControl(int Damage)
    {
        Health -= Damage;
        if (Health<=0 && DeadControl)
        {
            Dead();
        }
    }
   
   public void Dead()
    {
        DeadControl = false;
        GameControl.GetComponent<GameControl>().DeadEnemy();
        animator.SetTrigger("Dead");
        Destroy(gameObject,10f);
    }
}
