using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public PlayerRunState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) {}

    public override void EnterState()
    {
        ctx.Animator.SetBool(ctx.IsWalkingHash, true);
        ctx.Animator.SetBool(ctx.IsRunningHash, true);
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        ctx.AppliedMovementX = ctx.CurrentMovementInput.x * ctx.RunMultiplier;
        ctx.AppliedMovementZ = ctx.CurrentMovementInput.z * ctx.RunMultiplier;
    }

    public override void ExitState(){}

    public override void InitializeSubState(){}

    public override void CheckSwitchStates()
    {
        if(!ctx.IsMovementPressed)
        {
            SwitchState(factory.Idle());
        }
        else if(ctx.IsMovementPressed && !ctx.IsRunPressed)
        {
            SwitchState(factory.Walk());
        }
    }
}
