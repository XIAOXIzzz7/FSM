using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerBoard:BlackBoard
{

}
public class MoveState : IState
{
    public FSM fsm;
    public PlayerBoard playerBoard;
    public MoveState(FSM fsm)
    {
        this.fsm = fsm;
        fsm.blackBoard = playerBoard as BlackBoard;
    }
    public void OnEnter()
    {
       
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        
    }
}
public class JumpState : IState
{
    public FSM fsm;
    public PlayerBoard playerBoard;
    public JumpState(FSM fsm)
    {
        this.fsm = fsm;
        fsm.blackBoard = playerBoard as BlackBoard;
    }
    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}

public class PlayController : MonoBehaviour
{
    private FSM fsm;
    private PlayerBoard playerBoard;
    // Start is called before the first frame update
    void Start()
    {
        init();
    }
    private void init()
    {
        fsm = new FSM(playerBoard);
        fsm.AddState(StateType.Move, new MoveState(fsm));
        fsm.AddState(StateType.Jump, new JumpState(fsm));
        fsm.SwitchState(StateType.Move);
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnUpdate();
    }
}
