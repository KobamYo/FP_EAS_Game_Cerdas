using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        ctx.Animator.SetBool(ctx.IsWalkingHash, true);
        ctx.Animator.SetBool(ctx.IsRunningHash, false);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        ctx.AppliedMovementX = ctx.CurrentMovementInput.x;
        ctx.AppliedMovementZ = ctx.CurrentMovementInput.z;
    }

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if(!ctx.IsMovementPressed)
        {
            SwitchState(factory.Idle());
        }
        else if(ctx.IsMovementPressed && ctx.IsRunPressed)
        {
            SwitchState(factory.Run());
        }
    }
}
