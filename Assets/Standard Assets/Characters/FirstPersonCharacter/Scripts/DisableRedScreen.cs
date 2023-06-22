using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRedScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("DisableScreen", 0.15f);
    }

   void DisableScreen()
    {
        gameObject.SetActive(false);
    }
}
