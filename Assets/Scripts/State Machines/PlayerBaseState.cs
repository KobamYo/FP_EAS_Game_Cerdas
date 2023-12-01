public abstract class PlayerBaseState
{
    protected PlayerStateMachine ctx;
    protected PlayerStateFactory factory;
    protected PlayerBaseState currentSubState;
    protected PlayerBaseState currentSuperState;

    public PlayerBaseState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    {
        ctx = currentContext;
        factory = playerStateFactory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void ExitState();

    public abstract void CheckSwitchStates();

    public abstract void InitializeSubState();

    void UpdateStates(){}

    protected void SwitchState(PlayerBaseState newState)
    {
        // Current state exits state
        ExitState();

        // New state enters state
        newState.EnterState();

        // Switch current state of context
        ctx.CurrentState = newState;
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        currentSuperState = newSuperState;
    }

    protected void SetSubState(PlayerBaseState newSubState)
    {
        currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
