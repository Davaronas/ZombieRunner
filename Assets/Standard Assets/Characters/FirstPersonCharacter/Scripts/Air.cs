using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Air : MonoBehaviour
{

    RectTransform rectTransform;
   [SerializeField] float canStayUnderWaterSeconds = 60f;
    float currentCanStayUnderWaterSeconds;
    PlayerHealth playerHealth;
    bool canDealDamage = true;
    [SerializeField] float damageIntervalTime = 1f;
    [SerializeField] float damage = 5f;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        playerHealth = FindObjectOfType<PlayerHealth>();
        currentCanStayUnderWaterSeconds = canStayUnderWaterSeconds;
    }

    private void OnEnable()
    {
        currentCanStayUnderWaterSeconds = canStayUnderWaterSeconds;
        rectTransform.localScale = new Vector3(1f, (canStayUnderWaterSeconds / currentCanStayUnderWaterSeconds) / 100f, 1f);
        bool canDealDamage = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentCanStayUnderWaterSeconds = Mathf.Clamp(currentCanStayUnderWaterSeconds - Time.deltaTime,0f,100f);
        rectTransform.localScale = new Vector3(1f, (currentCanStayUnderWaterSeconds / canStayUnderWaterSeconds), 1f);
        if(currentCanStayUnderWaterSeconds <= 0 && canDealDamage == true)
        {
            canDealDamage = false;
            StartCoroutine("Drowning");
        }
    }

    IEnumerator Drowning()
    {
        if (playerHealth.isPlayerAlive == true)
        {
            playerHealth.PlayerHit(damage, false);
            yield return new WaitForSeconds(damageIntervalTime);
            canDealDamage = true;
        }
    }
}
