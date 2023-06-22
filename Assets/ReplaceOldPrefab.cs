using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaceOldPrefab : MonoBehaviour
{
    [SerializeField] GameObject newPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        Instantiate(newPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
