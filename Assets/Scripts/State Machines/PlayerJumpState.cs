using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerBaseState
{
    IEnumerator JumpResetRoutine()
    {
        yield return new WaitForSeconds(.5f);
        ctx.JumpCount = 0;
    }

    public PlayerJumpState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base (currentContext, playerStateFactory) 
    {
        InitializeSubState();
    }

    public override void EnterState()
    {
        HandleJump();
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
        HandleGravity();
    }

    public override void ExitState()
    {
        ctx.Animator.SetBool(ctx.IsJumpingHash, false);
        if(ctx.IsJumpPressed)
        {
            ctx.RequireNewJumpPress = true;
        }
        
        ctx.CurrentJumpResetRoutine = ctx.StartCoroutine(JumpResetRoutine());
        if(ctx.JumpCount == 3)
        {
            ctx.JumpCount = 0;
            ctx.Animator.SetInteger(ctx.JumpCountHash, ctx.JumpCount);
        }
    }

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
        if(ctx.CharacterController.isGrounded)
        {
            SwitchState(factory.Grounded());
        }
    }

    void HandleJump()
    {
        if(ctx.JumpCount < 3 && ctx.CurrentJumpResetRoutine != null)
            {
                ctx.StopCoroutine(ctx.CurrentJumpResetRoutine);
            }
            ctx.Animator.SetBool(ctx.IsJumpingHash, true);
            ctx.IsJumping = true;
            ctx.JumpCount += 1;
            ctx.Animator.SetInteger(ctx.JumpCountHash, ctx.JumpCount);
            ctx.CurrentMovementY = ctx.InitialJumpVelocities[ctx.JumpCount];
            ctx.AppliedMovementY = ctx.InitialJumpVelocities[ctx.JumpCount];
    }

    void HandleGravity()
    {
        bool isFalling = ctx.CurrentMovementY <= 0.0f || !ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if(isFalling)
        {
            float previousYVelocity = ctx.CurrentMovementY;
            ctx.CurrentMovementY = ctx.CurrentMovementY + (ctx.JumpGravities[ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            ctx.AppliedMovementY = Mathf.Max((previousYVelocity + ctx.CurrentMovementY) * .5f, -20.0f);
        }
        else
        {
            float previousYVelocity = ctx.CurrentMovementY;
            ctx.CurrentMovementY = ctx.CurrentMovementY + (ctx.JumpGravities[ctx.JumpCount] * Time.deltaTime);
            ctx.AppliedMovementY = (previousYVelocity + ctx.CurrentMovementY) * .5f;
        }
    }
}
