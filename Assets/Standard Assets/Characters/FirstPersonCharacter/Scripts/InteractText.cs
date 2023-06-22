using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractText : MonoBehaviour
{
    public bool pointingAtObject = false;
    public float timer = 0;
    [SerializeField] float disableTextTime =0.05f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

       if(timer > disableTextTime)
        {
            StopPointingOutOfRange();
        }
    }

    void StopPointingOutOfRange()
    {
        pointingAtObject = false;
        gameObject.SetActive(false);
    }
}
