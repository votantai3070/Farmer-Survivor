using System.Collections;
using UnityEngine;

[System.Serializable]
public struct PlayerSettings
{
    public float speed;
}

public class PlayerController : Character
{
    [Header("Player Controller Setting")]
    public PlayerControls controls;
    public PlayerSettings settings;
    [HideInInspector] public bool isAttacked = false;

    [Header("Speed")]
    private bool isBootedSpeed = false;
    [SerializeField] private GameObject speedEffectPrefab;

    public Rigidbody2D rb { get; private set; }
    public PlayerController controller { get; private set; }
    public PlayerDash movement { get; private set; }
    public PlayerVisuals visuals { get; private set; }
    public IdleState_Player idleState { get; private set; }
    public MoveState_Player moveState { get; private set; }
    public DeadState_Player deadState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        controls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        visuals = GetComponent<PlayerVisuals>();
        movement = GetComponentInChildren<PlayerDash>();

        idleState = new IdleState_Player(this, stateMachine, "Idle");
        moveState = new MoveState_Player(this, stateMachine, "Move");
        deadState = new DeadState_Player(this, stateMachine, "Dead");

    }

    private void Start()
    {
        InitializePlayerSettings();

        stateMachine.Initialize(idleState);

        Sprite sprite = GameManager.instance.characterAtlas.GetSprite("Stand 0");
        spriteRenderer.sprite = sprite;
    }

    private void InitializePlayerSettings()
    {
        settings.speed = characterData.speed;
    }

    protected override void Update()
    {
        base.Update();

        DeadAnimation();

        stateMachine.currentState.Update();
    }

    public IEnumerator BoostSpeed(float boostAmount, float duration)
    {
        isBootedSpeed = true;
        settings.speed += boostAmount;

        GameObject newEffect = ObjectPool.instance.GetObject(speedEffectPrefab);
        newEffect.transform.localPosition = Vector3.zero;
        newEffect.transform.SetParent(transform, false);

        yield return new WaitForSeconds(duration);

        ObjectPool.instance.DelayReturnToPool(newEffect);

        settings.speed -= boostAmount;
        isBootedSpeed = false;
    }

    public bool IsBoostedSpeed() => isBootedSpeed;

    public void HandleAttack() => isAttacked = true;

    public void StopAttack() => isAttacked = false;

    protected override void Die()
    {
        base.Die();
    }

    public void DeadAnimation()
    {
        if (CurrentHealth <= 0)
            stateMachine.ChangeState(deadState);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
