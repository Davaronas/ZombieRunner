using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger_Dead_Skeleton_Animaton1 : MonoBehaviour
{
    [SerializeField]GameObject animation_Trigger_Object;
    Animator animator;
    bool animation_Done = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animation_Trigger_Object == null && animation_Done == false)
        {
            animator.SetTrigger("Sword_Drawn");
            animation_Done = true;
        }
    }

   
}
