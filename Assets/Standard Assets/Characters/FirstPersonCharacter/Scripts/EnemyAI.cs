using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

[SelectionBase]





public class EnemyAI : MonoBehaviour
{
    Transform target;
    [SerializeField] GameObject viewPoint;
    [SerializeField] float chaseDistance = 2f;
    [SerializeField] float chaseDistanceFacing = 10f;
    [SerializeField] float multiplierNotFacing = 3f;
    [SerializeField] float multiplierFacing = 1.2f;
    [SerializeField] float facingAngle = 45f;
    [SerializeField] float alertRange = 3f;
    [SerializeField] float chaseSpeed = 4f;
    [SerializeField] float idleWalkSpeed = 1.5f;
    [SerializeField] float idleTurnSpeed = 50f;
    [SerializeField] float chaseTurnSpeed = 300f;
    [SerializeField] float minTimeBetweenIdleWalk = 20f;
    [SerializeField] float maxTimeBetweenIdleWalkPlusOne = 40f;
    [SerializeField] float walkingRangePlusOne = 21f;
    [SerializeField] float spottingFrequency = 0.1f;
    EnemyAI[] allEnemy = new EnemyAI[500];
    float currentChaseDistance;
    float currentChaseDistanceFacing;
    bool canSpot = true;
    bool stoppedIdleAudio = false;


    public bool isProvoked = false;
    Animator enemyAnimator;
    [SerializeField] float turnSpeedWhenAttacking = 5f;
    AudioSource audioSource;
    [SerializeField] bool zombie_Sounds = true;
    bool endedIdleWalk = true;
    playerBehavior player_behaviour;

    [SerializeField] AudioClip attack1;
    [SerializeField] AudioClip attack2;
    [SerializeField] AudioClip attack3;
    [SerializeField] AudioClip attack4;
    [SerializeField] AudioClip attack5;
    [SerializeField] AudioClip attack6;
    [SerializeField] AudioClip attack7;
    [SerializeField] AudioClip attack8;
    [SerializeField] AudioClip attack9;
    [SerializeField] AudioClip attack10;
    [SerializeField] AudioClip attack11;
    [SerializeField] AudioClip attack12;
    [SerializeField] AudioClip attack13;
    [SerializeField] AudioClip attack14;
    [SerializeField] AudioClip attack15;



    NavMeshAgent navMeshAgent;
    float distanceToTarget = Mathf.Infinity;

    void Awake()
    {
        target = FindObjectOfType<PlayerHealth>().transform;
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        navMeshAgent.speed = idleWalkSpeed;
        navMeshAgent.angularSpeed = idleTurnSpeed;
        player_behaviour = FindObjectOfType<playerBehavior>();
        float currentChaseDistance = chaseDistance;
        float currentChaseDistanceFacing = chaseDistanceFacing;

    }


    void Update()
    {
        if (player_behaviour.returnRunningState() == true)
        {
            currentChaseDistance = chaseDistance * multiplierNotFacing;
            currentChaseDistanceFacing = chaseDistanceFacing * multiplierFacing;
        }
        else
        {
            currentChaseDistance = chaseDistance;
            currentChaseDistanceFacing = chaseDistanceFacing;
        }

        if (isProvoked == false && canSpot == true)
        {
            StartCoroutine("PlayerSpotting");
        }
        else if (isProvoked == false && distanceToTarget <= currentChaseDistance)
        {
            Alert();
        }

        if (isProvoked)
        {
            EngageTarget();
            if (stoppedIdleAudio == false)
            {
                audioSource.Stop();
                stoppedIdleAudio = true;
            }
        }

        else if (isProvoked == false && endedIdleWalk == true)
        {
            IdleWalk();
        }

        if (isProvoked == false)
        {
            if (navMeshAgent.velocity == new Vector3(0, 0, 0))
            {
                enemyAnimator.SetBool("IdleWalk", false);
            }
            else enemyAnimator.SetBool("IdleWalk", true);
        }

    }

    private void IdleWalk()
    {
        endedIdleWalk = false;
        if(audioSource.isPlaying == false)
        audioSource.Play();
        Vector3 idleWalkDestination = new Vector3(transform.position.x + Random.Range(-walkingRangePlusOne, walkingRangePlusOne), transform.position.y, transform.position.z + Random.Range(-walkingRangePlusOne, walkingRangePlusOne));
        navMeshAgent.SetDestination(idleWalkDestination);
        Invoke("StartIdleWalkAgain", Random.Range(minTimeBetweenIdleWalk, maxTimeBetweenIdleWalkPlusOne));
    }

    IEnumerator PlayerSpotting()
    {
        canSpot = false;
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (Vector3.Angle(transform.forward, target.transform.position - transform.position) < facingAngle && distanceToTarget <= currentChaseDistanceFacing)
        {
           RaycastHit canSeePlayer;
            if (Physics.Raycast(viewPoint.transform.position, target.transform.position - viewPoint.transform.position, out canSeePlayer,chaseDistanceFacing))
            {
                if (canSeePlayer.transform.GetComponent<playerBehavior>() != null)
                {
                    Alert();
                }
            }
        }
        yield return new WaitForSeconds(spottingFrequency);
        canSpot = true;
    }

    public void Alert()
    {
        if (isProvoked == false)
        {
            allEnemy = FindObjectsOfType<EnemyAI>();
            foreach (EnemyAI enemy in allEnemy)
            {
                if (Vector3.Distance(transform.position, enemy.transform.position) <= alertRange)
                {
                    enemy.isProvoked = true;
                }
            }
            if (audioSource != null) 
            isProvoked = true;
        }
    }

        void StartIdleWalkAgain()
        {
            endedIdleWalk = true;
        }

        private void EngageTarget()
        {
        if (zombie_Sounds && target.GetComponent<PlayerHealth>().isGamePaused == false)
                PlayZombieSounds();

        navMeshAgent.speed = chaseSpeed;
        navMeshAgent.angularSpeed = chaseTurnSpeed;

        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if (distanceToTarget > navMeshAgent.stoppingDistance)
            {
                ChaseTarget();
            }
            else
            {
                AttackTarget();
            }
        }

        private void PlayZombieSounds()
        {
            if (audioSource == null) return;
            if (audioSource.isPlaying == false)
            {
                switch (Random.Range(1, 16))
                {
                    case 1:
                        audioSource.PlayOneShot(attack1);
                        break;
                    case 2:
                        audioSource.PlayOneShot(attack2);
                        break;
                    case 3:
                        audioSource.PlayOneShot(attack3);
                        break;
                    case 4:
                        audioSource.PlayOneShot(attack4);
                        break;
                    case 5:
                        audioSource.PlayOneShot(attack5);
                        break;
                    case 6:
                        audioSource.PlayOneShot(attack6);
                        break;
                    case 7:
                        audioSource.PlayOneShot(attack7);
                        break;
                    case 8:
                        audioSource.PlayOneShot(attack8);
                        break;
                    case 9:
                        audioSource.PlayOneShot(attack9);
                        break;
                    case 10:
                        audioSource.PlayOneShot(attack10);
                        break;
                    case 11:
                        audioSource.PlayOneShot(attack11);
                        break;
                    case 12:
                        audioSource.PlayOneShot(attack12);
                        break;
                    case 13:
                        audioSource.PlayOneShot(attack13);
                        break;
                    case 14:
                        audioSource.PlayOneShot(attack14);
                        break;
                    case 15:
                        audioSource.PlayOneShot(attack15);
                        break;
                }
            }
        }
    

    void ChaseTarget()
    {
        navMeshAgent.SetDestination(target.position);
        enemyAnimator.SetTrigger("Move");
        enemyAnimator.SetBool("Attack", false);
    }

    private void AttackTarget()
    {
        FaceTarget();
        enemyAnimator.SetBool("Attack", true);
    }

    private void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookDirection = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, turnSpeedWhenAttacking);
    }

    public void ifHeardShot(float hearDistance)
    {
        if (distanceToTarget <= hearDistance)
        {
            isProvoked = true;
        }
    }

    public void DecreaseVolumeAfterPlayerDeath()
    {
        audioSource.volume = 0.015f;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
       // Gizmos.DrawWireSphere(transform.position, hearDistance);
    }
}
