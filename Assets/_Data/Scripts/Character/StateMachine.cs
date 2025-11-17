public class StateMachine
{
    public CharacterState currentState { get; private set; }

    public void Initialize(CharacterState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(CharacterState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

}
