using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    [SerializeField] GameObject zombie_Horde;
    [SerializeField] GameObject zombie_Runner;
    [SerializeField] GameObject zombie_Tank;
    [SerializeField] int hordeAmount;
    [SerializeField] int runnerAmount;
    [SerializeField] int tankAmount;
    [SerializeField] float intervalTime = 0.3f;
    [SerializeField] bool provokedOnSpawn = true;
    GameObject enemies;
    [HideInInspector]public bool forceSpawn = false;
    bool triggered = false;
    Transform spawnPoint;

    void Start()
    {
        enemies = GameObject.Find("Enemies");
        spawnPoint = transform.Find("SpawnPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (forceSpawn == true && triggered == false)
        {
            StartCoroutine("SpawnZombies");
            triggered = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerHealth>() != null && triggered == false)
        {
            StartCoroutine("SpawnZombies");
            triggered = true;
        }

        
    }

    IEnumerator SpawnZombies()
    {
        while(hordeAmount > 0 || runnerAmount > 0 || tankAmount > 0)
        {
            if(hordeAmount > 0)
            {
                GameObject newHordeZombie = Instantiate(zombie_Horde, spawnPoint.transform.position, spawnPoint.transform.rotation);
                newHordeZombie.transform.parent = enemies.transform;
                newHordeZombie.GetComponent<EnemyAI>().isProvoked = provokedOnSpawn;
                hordeAmount--;
                yield return new WaitForSeconds(intervalTime);
            }

            if (runnerAmount > 0)
            {
                GameObject newRunnerZombie = Instantiate(zombie_Runner, spawnPoint.transform.position, spawnPoint.transform.rotation);
                newRunnerZombie.transform.parent = enemies.transform;
                newRunnerZombie.GetComponent<EnemyAI>().isProvoked = provokedOnSpawn;
                runnerAmount--;
                yield return new WaitForSeconds(intervalTime);
            }

            if (tankAmount > 0)
            {
                GameObject newTankZombie = Instantiate(zombie_Tank, spawnPoint.transform.position, spawnPoint.transform.rotation);
                newTankZombie.transform.parent = enemies.transform;
                newTankZombie.GetComponent<EnemyAI>().isProvoked = provokedOnSpawn;
                tankAmount--;
                yield return new WaitForSeconds(intervalTime);
            }
        }
        Destroy(gameObject);
    }
}
