using UnityEngine;

public class HitState_Melee : CharacterState
{
    private Enemy_Melee enemy;

    public HitState_Melee(Character characterBase, StateMachine stateMachine, string animBoolName) : base(characterBase, stateMachine, animBoolName)
    {
        enemy = characterBase as Enemy_Melee;
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
