
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    public string customName;

    private State mainStateType;

    public State CurrentState { get; private set; }
    private State nextState;

    void Update()
    {
      if(nextState != null)
        {
            SetState(nextState);
           
        } 
      if(CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }

    public void SetState(State _newstate) 
    {
        nextState = null;
        if (CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentState = _newstate;
        CurrentState.OnEnter(this);
    }

 public void SetNextState(State _newstate)
    {
        if(_newstate != null)
        {
            
            nextState = _newstate;
            
        }
    }
    public void LateUpdate()
    {
        if(CurrentState != null) 
        {
            CurrentState.OnLateUpdate();

        }

    }
    public void FixedUpdate()
    {
        if (CurrentState != null)
        {
            CurrentState.OnFixedUpdate();

        }

    }

    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }

    private void Awake()
    {
        SetNextStateToMain();

    }


    private void OnValidate()
    {
        if (mainStateType == null)
        {
            if (customName == "Combat")
            {
                mainStateType = new idleCombatState();
            }
        }
    }
}
