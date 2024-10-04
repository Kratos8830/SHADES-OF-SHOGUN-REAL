using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
   
    public Image healthFill;
    private float healthAmount = 100f;
    public TMP_Text currenthealth;

    private void Start()
    {
        currenthealth.text = "100";
    }



    // Update is called once per frame
    void Update()
    {
        currenthealth.text = ""+ healthAmount; 
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(5);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(5);
        }
    }

    public void TakeDamage(float Damage)
    {
        healthAmount -= Damage;
        healthFill.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100f);
        healthFill.fillAmount = healthAmount / 100f;
    }
}


