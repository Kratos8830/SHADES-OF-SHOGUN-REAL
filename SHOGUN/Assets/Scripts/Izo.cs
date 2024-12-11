using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Izo: MonoBehaviour
{
    public bool canDash = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UnlockDashAbility();
        }
    }

    private void UnlockDashAbility()
    {
        GameController.Instance.DashUnlocked = true;
    }
}
