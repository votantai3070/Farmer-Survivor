using System.Xml;
using UnityEngine;

[SerializeField]

public class Enemy_Range : Enemy
{
    [Header("Enemy Range Controller Setting")]
    public float attackCooldown = 1f;
    [SerializeField] private GameObject stonePrefab;
    [SerializeField] private string spriteName;

    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public AttackState_Range attackState { get; private set; }
    public DeadState_Range deadState { get; private set; }
    public HitState_Range hitState { get; private set; }


    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        attackState = new AttackState_Range(this, stateMachine, "Attack");
        deadState = new DeadState_Range(this, stateMachine, "Dead");
        hitState = new HitState_Range(this, stateMachine, "Hit");

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        Sprite sprite = GameManager.instance.characterAtlas.GetSprite(spriteName);
        spriteRenderer.sprite = sprite;


        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        if (stateMachine.currentState != null)
            stateMachine.currentState.Update();
    }

    public void DeadAnimation()
    {
        if (CurrentHealth <= 0)
        {
            stateMachine.ChangeState(deadState);
        }
    }

    public void HandleAttack()
    {
        GameObject stone = ObjectPool.instance.GetObject(stonePrefab);
        stone.transform.position = transform.position;
        stone.transform.rotation = Quaternion.identity;

        BulletMovement stoneBullet = stone.GetComponent<BulletMovement>();
        stoneBullet.InitializeWeapon(stoneBullet.weaponData);

        stoneBullet.HandleBulletMovement(true, stoneBullet.weapon);
    }

    protected override void Die()
    {
        base.Die();
    }
}
