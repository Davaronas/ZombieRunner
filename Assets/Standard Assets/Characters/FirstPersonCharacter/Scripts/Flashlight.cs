using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] Light flashlight;
    [SerializeField] float minIntensityLevel = 0.5f;
    [SerializeField] float decreaseAmount = 0.01f;
    [SerializeField] float decreaseIntervalTime = 0.5f;
    bool canDecrease = true;
    [SerializeField] float maxIntensity;


    void Update()
    {
       
        if(canDecrease == true && flashlight.intensity > minIntensityLevel)
        {
            StartCoroutine("DecreaseIntensity");
        }

    }

    private void OnEnable()
    {
        canDecrease = true;
    }

    IEnumerator DecreaseIntensity()
    {
        canDecrease = false;
        flashlight.intensity -= decreaseAmount;
        yield return new WaitForSeconds(decreaseIntervalTime);
        canDecrease = true;
    }

    public void PickUpBattery()
    {
        flashlight.intensity = maxIntensity;
    }
}
