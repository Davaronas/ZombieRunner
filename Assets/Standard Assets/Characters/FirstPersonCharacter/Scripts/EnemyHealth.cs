using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] public float hitPoints = 100f;
    [SerializeField] float dissapearCorpseTime = 10f;
   
    Transform mainCamera;
    EnemyAI enemyAI;
    Animator animator;
    NavMeshAgent navMeshAgent;
   [HideInInspector] public bool alive = true;
    [HideInInspector] public bool dismembered = false;
   

    public Rigidbody[] rigidbodies = new Rigidbody[20];

    void Awake()
    {
        mainCamera = Camera.main.transform;
        enemyAI = GetComponent<EnemyAI>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        
    }

   
       


    public void TakeDamage(float damage)
    {
        hitPoints -= damage;
        if (hitPoints <= 0 && alive == true)
        {
            Invoke("BroadcastEnemyDeath", 0.3f);
            animator.enabled = false;
            enemyAI.enabled = false;

            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
               
                Invoke("StopRigidbodies", 15f);
            }
           
           navMeshAgent.enabled = false;
            GetComponent<AudioSource>().Stop();
          // Destroy(gameObject, dissapearCorpseTime);
            alive = false;
        }
        else
        {
            enemyAI.Alert();
            //enemyAI.isProvoked = true;
        } 
    }

    public void TakeDamage(BroadcastHit.EvaluateHit currentHit)
    {
        if(enemyAI.isProvoked == false && currentHit.stealthHit == true)
            hitPoints -= currentHit.damage*4;
        else
            hitPoints -= currentHit.damage;
        if (hitPoints <= 0 && alive == true)
        {
            animator.enabled = false;
            enemyAI.enabled = false;

            foreach (Rigidbody rigidbody in rigidbodies)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
              
                Invoke("StopRigidbodies", 15f);
            }

            navMeshAgent.enabled = false;
            GetComponent<AudioSource>().Stop();
            //Destroy(gameObject, dissapearCorpseTime);
            alive = false;
        }
        else
        {
            enemyAI.Alert();
            //enemyAI.isProvoked = true;
        }
    }

    void BroadcastEnemyDeath()
    {
        BroadcastMessage("StopDismemberingAfterDeath");
    }

    void StopRigidbodies()
    {
        foreach (Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
    }


}
