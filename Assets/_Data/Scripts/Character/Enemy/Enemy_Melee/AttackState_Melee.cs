using UnityEngine;

public class AttackState_Melee : CharacterState
{
    private Enemy_Melee enemy;
    private float lastAttackTime = -10f;


    public AttackState_Melee(Enemy enemyBase, StateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;
    }

    public override void Enter()
    {
        base.Enter();

        //Debug.Log("Enter Attack State");

        enemy.EnableCollider2D(true);

    }

    public override void Exit()
    {
        base.Exit();

        enemy.EnableCollider2D(false);

    }

    public override void Update()
    {
        base.Update();

        //Debug.Log("Update Attack State");

        Attack();

        enemy.DeadAnimation();
    }

    private void Attack()
    {
        float attackAnimDuration = enemy.GetAnimationClipDuration("Attack");

        if (Time.time >= lastAttackTime + attackAnimDuration + enemy.attackCooldown)
        {
            lastAttackTime = Time.time;
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
