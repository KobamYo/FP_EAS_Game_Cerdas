using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerBaseState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) 
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        Debug.Log("Grounded");
        ctx.CurrentMovementY = ctx.GroundedGravity;
        ctx.AppliedMovementY = ctx.GroundedGravity;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void InitializeSubState()
    {
        if(!ctx.IsMovementPressed && !ctx.IsRunPressed)
        {
            SetSubState(factory.Idle());
        }
        else if(ctx.IsMovementPressed && !ctx.IsRunPressed)
        {
            SetSubState(factory.Walk());
        }
        else
        {
            SetSubState(factory.Run());
        }
    }

    public override void CheckSwitchStates()
    {
        // If player is grounded and jump is pressed, switch to jump state
        if(ctx.IsJumpPressed && !ctx.RequireNewJumpPress)
        {
            SwitchState(factory.Jump());
        }
    }
}
