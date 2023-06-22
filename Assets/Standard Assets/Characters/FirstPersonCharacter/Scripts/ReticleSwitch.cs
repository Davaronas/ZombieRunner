using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleSwitch : MonoBehaviour
{

    MapType mapType;
    [SerializeField] GameObject reticle;
   [SerializeField] Sprite notDarkReticle;
   [SerializeField] Sprite darkReticle;
   Image reticleSprite;

    void Start()
    {
        reticleSprite = reticle.GetComponent<Image>();
        mapType = FindObjectOfType<MapType>();
        if (mapType.dark == true)
        {
            reticleSprite.sprite = darkReticle;
        }
        else
        {
            reticleSprite.sprite = notDarkReticle;
        }
    }

   
}
