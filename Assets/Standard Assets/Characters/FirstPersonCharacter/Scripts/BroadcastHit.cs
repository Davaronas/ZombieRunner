using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroadcastHit : MonoBehaviour
{
    Rigidbody bodyPartRigidbody;
    Transform mainCamera;
    [SerializeField] EnemyHealth parentEnemy;
    float parentHealth;
    [SerializeField] bool bodyPartCanBeDismembered = false;
    [SerializeField] bool head = false;
    [SerializeField] GameObject bodyPart;
    [SerializeField] Quaternion additionalRotation;
    Quaternion currentRotation;
    [SerializeField] GameObject[] bodyPartsToDisable = new GameObject[6];
    [Space]
    [Space]
    [SerializeField] GameObject dismemberBloodVFX;
    

    public struct EvaluateHit
    {
        public float damage;
        public bool stealthHit;
    }


    private void Awake()
    {
        mainCamera = Camera.main.transform;
        bodyPartRigidbody = GetComponent<Rigidbody>();
    }

    public void CallTakeDamage(float damage)
    {
       SendMessageUpwards("TakeDamage", damage);
    }

    public void CallTakeDamage(float damage,float force)
    {
        parentHealth = parentEnemy.hitPoints;
        SendMessageUpwards("TakeDamage", damage);
        StartCoroutine(AfterDeadEffects(parentHealth,damage, force,false,false));
    }

    public void CallTakeDamage(float damage, float force, bool canDismember)
    {
        parentHealth = parentEnemy.hitPoints;
        SendMessageUpwards("TakeDamage", damage);
        StartCoroutine(AfterDeadEffects(parentHealth,damage, force,canDismember,false));
    }

        public void CallTakeDamage(float damage, float force, bool canDismember, bool isSword)
        {
        parentHealth = parentEnemy.hitPoints;
        EvaluateHit currentHit;
            currentHit.damage = damage;
            currentHit.stealthHit = isSword;
            SendMessageUpwards("TakeDamage", currentHit);
            StartCoroutine(AfterDeadEffects(parentHealth,damage,force,canDismember,true));

        }

    public void StopDismemberingAfterDeath()
    {
        bodyPartCanBeDismembered = false;
    }

    IEnumerator AfterDeadEffects(float parentHealth,float damage, float force,bool canDismember,bool isSword)
    {
        bodyPartRigidbody.AddForce(mainCamera.forward * force);
       currentRotation =  Quaternion.Euler(transform.localRotation.x + additionalRotation.x, transform.localRotation.y + additionalRotation.y, transform.localRotation.z + additionalRotation.z);
        if (parentEnemy.dismembered == false && parentEnemy.alive == false)
        {
            if (canDismember && bodyPartCanBeDismembered)
            {
                if(head != true)
                Instantiate(bodyPart, transform.position, currentRotation);
                else
                {
                    if(head == true && isSword == true)
                        Instantiate(bodyPart, transform.position, currentRotation);
                }
                Instantiate(dismemberBloodVFX, transform.position, transform.rotation);
                parentEnemy.dismembered = true;
                foreach (GameObject bodyPartToDisable in bodyPartsToDisable)
                {
                    if (bodyPartToDisable != null)
                        bodyPartToDisable.SetActive(false);
                }
            }
        }
       
            
        yield return new WaitForEndOfFrame();
    }

        public void ZombieHeadDamage(float damage)
        {
            SendMessageUpwards("StayingOnZombies", damage);
        }
    
}
