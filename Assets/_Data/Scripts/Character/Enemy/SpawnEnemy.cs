using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    [Header("Spawn Enemy Setting")]
    //public Prefabs enemyPrefabs;
    public Transform player;
    [SerializeField] private DifficultData defaultDiffData;

    [SerializeField] private List<GameObject> enemiesPhase1 = new();
    [SerializeField] private List<GameObject> enemiesPhase2 = new();
    [SerializeField] private List<GameObject> enemiesPhase3 = new();
    [SerializeField] private List<GameObject> enemiesPhase4 = new();


    [SerializeField] private float spawnRadius;
    private int maxEnemiesPhase1;
    private int maxEnemiesPhase2;
    private int maxEnemiesPhase3;
    private int maxEnemiesPhase4;

    [SerializeField]
    private float phase1Time;
    [SerializeField]
    private float phase2Time;
    [SerializeField]
    private float phase3Time;

    [SerializeField] private float spawnTime;

    [SerializeField] int maxEnemies;
    [SerializeField] int startTime;

    private List<GameObject> activeEnemies = new();
    private void Start()
    {
        StartCoroutine(Wait());

        InvokeRepeating(nameof(HandleSpawnEnemy), 2f, spawnTime);
    }

    IEnumerator Wait()
    {
        yield return null;
        if (GameManager.instance.currentDifficultData != null)
        {
            SetDifficulty(GameManager.instance.currentDifficultData);
            GameManager.instance.currentTime = startTime;
        }
        else
        {
            Debug.LogWarning("No difficulty selected, fallback to Easy");
        }
    }

    private void Update()
    {
        //Debug.Log("maxEnemies: " + maxEnemies);
        //Debug.Log("activeEnemies Count: " + activeEnemies.Count);

        GameManager.instance.ShowTime(startTime);
    }

    void HandleSpawnEnemy()
    {
        if (player == null
          && enemiesPhase1.Count == 0
          && enemiesPhase2.Count == 0
          && enemiesPhase3.Count == 0
          && enemiesPhase4.Count == 0)
            return;

        if (activeEnemies.Count >= maxEnemies) return;

        float currentTime = GameManager.instance.currentTime;

        float phase1Start = startTime * phase1Time;
        float phase2Start = startTime * phase2Time;
        float phase3Start = startTime * phase3Time;

        if (currentTime > phase1Start)
        {
            RandomEnemyInPhase(enemiesPhase1);

            maxEnemies = maxEnemiesPhase1;
            Debug.Log("Phase 1");

        }
        else if (currentTime > phase2Start)
        {
            RandomEnemyInPhase(enemiesPhase2);

            maxEnemies = maxEnemiesPhase2;
            Debug.Log("Phase 2");
        }
        else if (currentTime > phase3Start)
        {
            RandomEnemyInPhase(enemiesPhase3);

            maxEnemies = maxEnemiesPhase3;
            Debug.Log("Phase 3");
        }
        else
        {
            RandomEnemyInPhase(enemiesPhase4);

            maxEnemies = maxEnemiesPhase4;
            Debug.Log("Phase 4");
        }
    }

    private void RandomEnemyInPhase(List<GameObject> enemyPhase)
    {
        int randomEnemy = Random.Range(0, enemyPhase.Count);

        GameObject enemy = enemyPhase[randomEnemy];

        GameObject enemyGetToPool = ObjectPool.instance.GetObject(enemy);

        Vector3 spawnPos = GetRandomPositionNearPlayer();

        enemyGetToPool.transform.position = spawnPos;

        activeEnemies.Add(enemyGetToPool);
    }

    private Vector3 GetRandomPositionNearPlayer()
    {
        Vector3 pos = Random.insideUnitCircle.normalized * spawnRadius;
        return player.position + new Vector3(pos.x, pos.y, 0);
    }

    public void ReturnEnemy(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
    }

    void SetDifficulty(DifficultData difficultData)
    {
        maxEnemies = difficultData.phase1EnemyCount;

        maxEnemiesPhase1 = difficultData.phase1EnemyCount;
        maxEnemiesPhase2 = difficultData.phase2EnemyCount;
        maxEnemiesPhase3 = difficultData.phase3EnemyCount;
        maxEnemiesPhase4 = difficultData.phase4EnemyCount;

        startTime = difficultData.timeCountdown;

        phase1Time = difficultData.phase1Time;
        phase2Time = difficultData.phase2Time;
        phase3Time = difficultData.phase3Time;
    }
}
