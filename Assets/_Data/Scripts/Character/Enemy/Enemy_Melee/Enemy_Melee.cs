using UnityEngine;

public class Enemy_Melee : Enemy
{
    [Header("Enemy Melee Controller Setting")]

    [Space]
    private CapsuleCollider2D capsuleCollider;
    [SerializeField] private string spriteName;
    public float attackCooldown = 1f;

    public IdleState_Melee idleState { get; private set; }
    public MoveState_Melee moveState { get; private set; }
    public AttackState_Melee attackState { get; private set; }
    public DeadState_Melee deadState { get; private set; }
    public HitState_Melee hitState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Melee(this, stateMachine, "Idle");
        moveState = new MoveState_Melee(this, stateMachine, "Move");
        attackState = new AttackState_Melee(this, stateMachine, "Attack");
        deadState = new DeadState_Melee(this, stateMachine, "Dead");
        hitState = new HitState_Melee(this, stateMachine, "Hit");

        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    protected override void Start()
    {
        stateMachine.Initialize(idleState);

        Sprite sprite = GameManager.instance.characterAtlas.GetSprite(spriteName);
        spriteRenderer.sprite = sprite;
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }

    public void DeadAnimation()
    {
        if (CurrentHealth <= 0)
        {
            stateMachine.ChangeState(deadState);
        }
    }

    public void EnableCollider2D(bool active)
    {
        capsuleCollider.enabled = active;
    }

    public bool NearPlayer()
    {
        float distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
        if (distance <= aIPathSettings.endReachedDistance)
            return true;

        return false;
    }

    protected override void Die()
    {
        base.Die();
    }
}
