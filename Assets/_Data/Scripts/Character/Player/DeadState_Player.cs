using UnityEngine;

public class DeadState_Player : CharacterState
{
    private PlayerController player;

    public DeadState_Player(Character characterBase, StateMachine stateMachine, string animBoolName) : base(characterBase, stateMachine, animBoolName)
    {
        player = characterBase as PlayerController;
    }

    public override void Enter()
    {
        base.Enter();

        GameManager.instance.GameOver();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
