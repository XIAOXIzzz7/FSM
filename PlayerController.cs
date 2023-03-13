using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;

[Serializable]
public class PlayerBoard : BlackBoard
{
    internal float velocity;
    public Transform transform;
    public Animator animator;
    public CharacterController characterController;
}
public class JumpState : Istate
{
    public PlayerBoard Board;
    public FSM fsm;

    public JumpState(FSM fsm)
    {
        this.fsm = fsm;
        this.Board = fsm.blackBoard as PlayerBoard;
    }
    public void OnEnter()
    {
       
        Board.animator.SetBool("Jump", true);
        
    }

    public void OnExit()
    {
        Board.animator.SetBool("Jump", false);
    }

    public void OnUpdate()
    {

       
        if (!Board.characterController.isGrounded)
        {

            fsm.SwitchState(StateType.Move);
           

        }

    }
}
public class MoveState :Istate
{
    public PlayerBoard Board;
    public FSM fsm;
    
    public MoveState(FSM fsm)
    {
        this.fsm = fsm;
        this.Board = fsm.blackBoard as PlayerBoard;
    }
    public void OnEnter()
    {

        
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
      
        Board.animator.SetFloat("MoveVelocity",Board.velocity,0.3f,Time.deltaTime);
    }
}
public class PlayerController : MonoBehaviour
{
   
    public FSM fsm;
    public PlayerBoard playerBoard;
    private Transform Ptransform;
    private Animator animator;
    private PlayerInput _input;
    private float WalkingVelocity = 1.522f;
    private float RunningVelocity = 5.13f;
    public Transform myCamera;
    float smooth = 0.2f;
    float yVelocity = 0.0f;
    private CharacterController characterController;
    private float gravity = -9.8f;
    public float verticalVelocity;
    public bool isJump;
    public bool isJumpRD;
    public float maxHeight = 0.6f;

    private void Start()
    {
        animator= GetComponent<Animator>();
        characterController= GetComponent<CharacterController>();
        animator.SetFloat("ScaleFactor", 1 / animator.humanScale);
        _input = GetComponent<PlayerInput>();
        isJumpRD = true;
        init();
        
    }
    private void Awake()
    {

        
        
       
    }
    private void init()
    {
        fsm = new FSM(playerBoard);
        fsm.AddState(StateType.Move, new MoveState(fsm));
        fsm.AddState(StateType.Jump, new JumpState(fsm));
        fsm.SwitchState(StateType.Move);
    }

    private void Move()
    {
        float curSpeed;
        if (_input.isRun)
        {
            curSpeed = RunningVelocity;
        }
        else
        {
            curSpeed = WalkingVelocity;
        }
        playerBoard.velocity = curSpeed * _input.Move.magnitude;
    }
    public void OnJump(InputAction.CallbackContext callback)
    {
        if(callback.action.phase == InputActionPhase.Started)
        {
            isJump= true;
            
            //
        }
    }
    public void GetJumpRD(int flag)
    {

        isJumpRD = true?flag==0:flag==1;

    }
    public void GetJump()
    {
        characterController.height = animator.GetFloat("JumpHeight");
        characterController.center = new Vector3(0, animator.GetFloat("JumpCenter"), 0);
        
        if (characterController.isGrounded && isJump &&isJumpRD)
        {
            fsm.SwitchState(StateType.Jump);
   
            verticalVelocity = Mathf.Sqrt(-2 * gravity * maxHeight);


        }
        else
        {
            isJump = false;
        }

    }

    private void Rotation()
    {
        if (_input.Move.magnitude != 0)
        {

            float angle = Mathf.Atan2(_input.Move.x, _input.Move.y) * Mathf.Rad2Deg + myCamera.eulerAngles.y;
            float yAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref yVelocity, smooth);
            transform.rotation = Quaternion.Euler(0, yAngle, 0);
        }
    }


    private void Update()
    {
        if (fsm == null)
        {

            init();

        }


        fsm.OnUpdate();
        Move();
        Rotation();
        
        GetGravity();
        GetJump();
        //Debug.Log(Input.touchCount);
  



    }
    private void GetGravity()
    {
        
        if (characterController.isGrounded)
        {
            verticalVelocity = gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity += 2f*gravity * Time.deltaTime;
        }
    }
    public void OnAnimatorMove()
    {
        Vector3 Po = animator.deltaPosition;
        Po.y = verticalVelocity*Time.deltaTime;
        characterController.Move(Po);
    }

}

