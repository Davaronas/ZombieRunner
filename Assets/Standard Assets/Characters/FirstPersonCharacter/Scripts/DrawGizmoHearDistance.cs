using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]

public class DrawGizmoHearDistance : MonoBehaviour
{
    Weapon[] weapons = new Weapon[9];
    int i;
    float[] zombieHearDistances = new float[9];


    // Start is called before the first frame update
    void Start()
    {
        weapons = Resources.FindObjectsOfTypeAll<Weapon>();
        
       
    }

    // Update is called once per frame
    void Update()
    {
        for (i = 0; i <= 8; i++)
        {
            zombieHearDistances[i] = weapons[i].zombieHearDistance;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(255, 0, 0);
        Gizmos.DrawWireSphere(transform.position,zombieHearDistances[0]);
        Gizmos.DrawWireSphere(transform.position, zombieHearDistances[1]);
        Gizmos.DrawWireSphere(transform.position, zombieHearDistances[2]);
        Gizmos.DrawWireSphere(transform.position, zombieHearDistances[3]);
        Gizmos.DrawWireSphere(transform.position, zombieHearDistances[4]);
        Gizmos.DrawWireSphere(transform.position, zombieHearDistances[5]);
        Gizmos.DrawWireSphere(transform.position, zombieHearDistances[6]);
        Gizmos.DrawWireSphere(transform.position, zombieHearDistances[7]);
        Gizmos.DrawWireSphere(transform.position, zombieHearDistances[8]);

    }
}
