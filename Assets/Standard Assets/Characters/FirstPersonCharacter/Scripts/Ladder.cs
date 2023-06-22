using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    playerBehavior player;
    Transform thisTransform;

    // Start is called before the first frame update
    void Start()
    {
       player = FindObjectOfType<playerBehavior>();
        thisTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.GetComponent<PlayerHealth>() != null && player.crouching == false)
        {
            other.attachedRigidbody.useGravity = false;
            other.attachedRigidbody.velocity = new Vector3(0, 0, 0);
            player.notAbleToCrouch = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.GetComponent<PlayerHealth>() != null && player.crouching == false)
        {
            if (Input.GetAxis("Vertical") > 0 && other.attachedRigidbody.velocity.magnitude < 2f)
            {
                other.attachedRigidbody.AddForce(Vector3.up * 1000);
                other.attachedRigidbody.AddForce(-thisTransform.forward * 1000);
            }
            else if (Input.GetAxis("Vertical") < 0 && other.attachedRigidbody.velocity.magnitude < 2f)
            {
                other.attachedRigidbody.AddForce(Vector3.down * 1000);
                other.attachedRigidbody.AddForce(thisTransform.forward * 100);
            }
            else
            {
                if (Input.GetAxis("Vertical") == 0)
                {
                    other.attachedRigidbody.velocity = new Vector3(0, 0, 0);
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.GetComponent<PlayerHealth>() != null)
        {
            other.attachedRigidbody.useGravity = true;
            player.notAbleToCrouch = false;
        }
    }
}
