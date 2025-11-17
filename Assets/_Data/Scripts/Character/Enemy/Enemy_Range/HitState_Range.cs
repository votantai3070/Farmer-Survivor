using UnityEngine;

public class HitState_Range : CharacterState
{
    private Enemy_Range enemy;

    public HitState_Range(Character characterBase, StateMachine stateMachine, string animBoolName) : base(characterBase, stateMachine, animBoolName)
    {
        enemy = characterBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
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
