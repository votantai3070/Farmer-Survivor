using Pathfinding;
using System.Collections;
using UnityEngine;
using static CharacterData;

[System.Serializable]
public struct AIPathSettings
{
    public float maxSpeed;
    public float endReachedDistance;
    public float repathRate;
}

public enum EnemyType { Melee, Range }

public enum EnemyName { Rat, Bat, Undead, Bone, Golem, Slime }

public class Enemy : Character
{
    [Header("Enemy Setting")]
    protected DropItem drop;

    public float idleTime = 2f;

    public AIPathSettings aIPathSettings;
    public EnemyType enemyType;
    public EnemyName enemyName;

    public SpawnEnemy spawnEnemy { get; private set; }
    public PlayerController player { get; private set; }
    public AIPath aIPath { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        aIPath = GetComponentInParent<AIPath>();
        spawnEnemy = GameObject.Find("SpawnEnemy").GetComponent<SpawnEnemy>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        drop = GameObject.Find("GameManager").GetComponent<DropItem>();
        InitializeAIPath();
    }

    protected virtual void Start()
    {
        if (characterData.characterType == CharacterType.Enemy)
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }

    protected override void Update()
    {
        Flip();
    }

    protected override void Die()
    {
        base.Die();
        if (characterData.characterType == CharacterType.Enemy)
        {
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        UIManager.instance.UpdateDefeatEnemy(characterData.reward);

        StartCoroutine(ReturnToPoolAfterDelay());

        DropItem();
    }

    IEnumerator ReturnToPoolAfterDelay(float delay = 1f)
    {
        yield return new WaitForSeconds(delay);
        ObjectPool.instance.DelayReturnToPool(transform.parent.gameObject);
    }

    private void DropItem()
    {
        Transform enemyRoot = transform.parent;

        drop.SetEnemyDropItem(enemyRoot, characterData);

        if (enemyName == EnemyName.Rat || enemyName == EnemyName.Slime || enemyName == EnemyName.Undead)
            drop.DropExp1(enemyRoot);
        else if (enemyName == EnemyName.Bat || enemyName == EnemyName.Bone)
            drop.DropExp2(enemyRoot);
        else if (enemyName == EnemyName.Golem)
            drop.DropExp3(enemyRoot);
    }

    public void CanMove(bool activeMove)
    {
        aIPath.canMove = activeMove;
    }

    public float GetAnimationClipDuration(string animName)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;

        foreach (var clip in clips)
        {
            if (clip.name == animName)
            {
                return clip.length;
            }
        }
        return 0f;
    }

    public void InitializeAIPath()
    {
        aIPath.maxSpeed = aIPathSettings.maxSpeed;
        aIPath.endReachedDistance = aIPathSettings.endReachedDistance;
        aIPath.repathRate = aIPathSettings.repathRate;
        aIPath.orientation = OrientationMode.YAxisForward;
    }

    private void Flip()
    {
        if (aIPath.desiredVelocity.x >= 0.01f)
            transform.parent.localScale = new Vector3(1f, 1f, 1f);
        else if (aIPath.desiredVelocity.x <= -0.01f)
            transform.parent.localScale = new Vector3(-1f, 1f, 1f);
    }
}
