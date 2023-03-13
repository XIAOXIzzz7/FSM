using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEditor.Rendering.InspectorCurveEditor;

[Serializable]
public class BlackBoard
{

}
public enum StateType
{
    Idle,
    Attack,
    Die,
    Jump,
    Move
}

public interface Istate
{
    void OnEnter();
    void OnExit();

    void OnUpdate();

}

public class FSM
{
    public Istate curState;
    public Dictionary<StateType, Istate> State;
    public BlackBoard blackBoard;
    public FSM(BlackBoard black)
    {
        State = new Dictionary<StateType, Istate>();
        this.blackBoard = black;
    }
    public void AddState(StateType stateType,Istate istate)
    {
        if (State.ContainsKey(stateType))
        {
            Debug.Log("当前状态已存在"+stateType);
            return;
        }
        else
        {
            State.Add(stateType, istate);
        }
        
    }
    public void SwitchState(StateType stateType)
    {
        if (!State.ContainsKey(stateType))
        {
            Debug.Log("该状态不存在"+stateType);
        }
        else
        {
            if (curState != null)
            {
                curState.OnExit();
            }
            curState = State[stateType];
            curState.OnEnter();
        }
    }
    public void OnUpdate()
    {
        
  
        

        curState.OnUpdate();
    }

    
}

