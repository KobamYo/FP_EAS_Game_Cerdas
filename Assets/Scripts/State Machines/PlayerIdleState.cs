using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        ctx.Animator.SetBool(ctx.IsWalkingHash, false);
        ctx.Animator.SetBool(ctx.IsRunningHash, false);
        ctx.AppliedMovementX = 0;
        ctx.AppliedMovementZ = 0;
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if(ctx.IsMovementPressed && ctx.IsRunPressed)
        {
            SwitchState(factory.Run());
        }
        else if(ctx.IsMovementPressed)
        {
            SwitchState(factory.Walk());
        }
    }
}
