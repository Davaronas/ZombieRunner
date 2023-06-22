using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollapseBridge : MonoBehaviour
{

    GameObject wood1;
    GameObject wood2;
    bool alreadyBroken = false;
    AudioSource audioSource;
    NavMeshObstacle parentObstacle;

    void Start()
    {
        wood1 = transform.Find("Wood1").gameObject;
        wood2 = transform.Find("Wood2").gameObject;
        audioSource = GetComponent<AudioSource>();
        parentObstacle = transform.parent.GetComponent<NavMeshObstacle>();
    }

   
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>() != null && alreadyBroken == false)
        {
            Rigidbody wood1RB = wood1.AddComponent<Rigidbody>();
            Rigidbody wood2RB = wood2.AddComponent<Rigidbody>();
            wood1RB.drag = -50;
            wood2RB.drag = -50;
            Destroy(wood1, 5);
            Destroy(wood2, 5);
            alreadyBroken = true;
            parentObstacle.enabled = true;
            audioSource.Play();
        }
    }
}
