using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
   
    public Image healthFill;
    public float healthAmount = 100f;
    private GameController gm;
    public Image resolveFill;
    private float resolveAmount = 0;
   

    private void Start()
    {
        
        gm= GameObject.Find("Player").GetComponent<GameController>();
        healthAmount = 100f;
        resolveFill.fillAmount = 0;
    }



    // Update is called once per frame
    void Update()
    {
     

        if(healthAmount<1)
        {
            gm.ZeroHealth();
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            UseResolvehealth();
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

    public void FillResolve(float amount)
    {
        resolveAmount += amount;
        resolveFill.fillAmount = resolveAmount / 100f;
    }

    private void RemoveResolve(float amount)
    {
        resolveAmount -= amount;
        resolveFill.fillAmount = resolveAmount / 100f;
    }

    
    private void UseResolvehealth()
    {
        if(resolveAmount>=33.33f && healthAmount<=99)
        {
            Heal(15f);
            RemoveResolve(33.33f);
        }
    }



   
}


