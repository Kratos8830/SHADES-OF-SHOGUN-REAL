using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFinisherState : meleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        //Attack
        attackindex = 1;
        duration = 0.5f;
        animator.SetTrigger("Attack" + 3);
        Debug.Log("Player Attack" + 1 + "Fired");

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
            
          
                stateMachine.SetNextStateToMain();
           
        }

    }
}
