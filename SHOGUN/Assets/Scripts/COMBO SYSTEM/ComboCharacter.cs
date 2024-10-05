using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboCharacter : MonoBehaviour
{
    private StateMachine meleeStateMachine;

    [SerializeField] public Collider2D hitbox;
    void Start()
    {
       meleeStateMachine = GetComponent<StateMachine>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && meleeStateMachine.CurrentState.GetType() == typeof(idleCombatState))
        {
            meleeStateMachine.SetNextState(new GroundEntryState());
        }
    }
}
