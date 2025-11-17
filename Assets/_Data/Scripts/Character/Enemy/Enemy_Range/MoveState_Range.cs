using UnityEngine;

public class MoveState_Range : CharacterState
{
    private Enemy_Range enemy;

    public MoveState_Range(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.CanMove(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();



        if (enemy.aIPath.reachedEndOfPath)
        {
            stateMachine.ChangeState(enemy.attackState);
        }

        enemy.DeadAnimation();
    }
}
