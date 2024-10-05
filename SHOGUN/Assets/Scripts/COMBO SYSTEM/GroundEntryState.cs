using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEntryState : meleeBaseState
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        //Attack
        attackindex = 1;
        duration = 0.5f;
        animator.SetTrigger("Attack" + 1);
        Debug.Log("Player Attack" + 1 + "Fired");

    }

    public override void OnUpdate()
    {
        base.OnUpdate();
       
        if(fixedtime >= duration)
        {
            if(shouldCombo) 
            { 
              stateMachine.SetNextState(new GroundComboState());
            }
            else
            {
                stateMachine.SetNextStateToMain();
            }
        }
        
    }
}
