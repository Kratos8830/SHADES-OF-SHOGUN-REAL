using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundComboState : meleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        //Attack
        attackindex = 2;
        duration = 0.10f;
        animator.SetTrigger("Attack" + 2);
        Debug.Log("Player Attack" + 2 + "Fired");

    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (fixedtime >= duration)
        {
            if (shouldCombo)
            {
                stateMachine.SetNextState(new GroundFinisherState());
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }
        }

    }
}
