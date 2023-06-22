using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAttack : MonoBehaviour
{
    PlayerHealth player;
    EnemyHealth enemyHealth;
    float enemyDamage;
    [SerializeField] bool isRunner = false;
   
    enum zombieType {horde,runner,tank};
    [SerializeField] zombieType thisZombie;
    int difficulty = 2;

    void Awake()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 2);


       switch (difficulty)
        {
            case 1: // easy mode
                enemyDamage = 2f;
                break;
            case 2: // normal mode
                enemyDamage = 5f;
                break;
            case 3: // hard
                enemyDamage = 10f;
                break;
            case 4: // masochist
                enemyDamage = 20f;
                break;
        }

        switch (thisZombie)
        {
            case zombieType.horde:
              //  enemyDamage = enemyDamage;
                break;
            case zombieType.runner:
                enemyDamage *= 2;
                break;
            case zombieType.tank:
                enemyDamage *= 4;
                break;
        }
        player = FindObjectOfType<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
       
    }

   void EnemyHitEvent()
    {
        if (player == null) return;
        if(player.isPlayerAlive == true)
       
        player.PlayerHit(enemyDamage,isRunner);
    }

    public void StayingOnZombies(float damage)
    {
        if(enemyHealth.alive == true)
        player.PlayerHit(damage, false);
       
    }
}
