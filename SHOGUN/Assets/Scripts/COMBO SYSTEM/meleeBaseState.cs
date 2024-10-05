using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class meleeBaseState : State
{
    public float duration;
    protected Animator animator;
    protected bool shouldCombo;
    protected int attackindex;
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator = GetComponent<Animator>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(Input.GetMouseButtonDown(0) == true)
        {
            shouldCombo = true;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
