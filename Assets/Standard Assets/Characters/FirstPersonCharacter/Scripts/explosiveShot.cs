using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveShot : MonoBehaviour
{
    Weapon usedWeapon;
    float dealDamage;



    private void Awake()
    {
        
        usedWeapon = FindObjectOfType<Weapon>();
        dealDamage = usedWeapon.currentWeaponDamage;
    }

    void Start()
    {
        transform.parent = GameObject.FindGameObjectWithTag("Explosions").transform;
        Destroy(gameObject, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
       

        if (other.CompareTag("Enemy") || other.CompareTag("EnemyHead"))
        {
            other.GetComponent<BroadcastHit>().CallTakeDamage(dealDamage,50,true);
        }

       

        if(other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            other.gameObject.GetComponent<PlayerHealth>().PlayerHit(dealDamage/10,false);
        }
    }

}
