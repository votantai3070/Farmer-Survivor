using UnityEngine;

public class FourWallGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private GameObject enemyPrefab;

    [Header("Wall Dimensions")]
    [Tooltip("Khoảng cách từ player đến bức tường")]
    [SerializeField] private float distanceFromPlayer = 5f;

    [Tooltip("Chiều dài bức tường (nửa chiều dài)")]
    [SerializeField] private float halfWallLength = 5f;

    [Header("Enemy Count Per Wall")]
    [SerializeField] private int topWallEnemyCount = 10;
    [SerializeField] private int bottomWallEnemyCount = 10;
    [SerializeField] private int leftWallEnemyCount = 10;
    [SerializeField] private int rightWallEnemyCount = 10;

    [Header("Wall Control")]
    [SerializeField] private bool generateTop = true;
    [SerializeField] private bool generateBottom = true;
    [SerializeField] private bool generateLeft = true;
    [SerializeField] private bool generateRight = true;

    [Header("Settings")]
    [SerializeField] private bool useObjectPool = true;
    [SerializeField] private bool showGizmos = true;

    private GameObject[][] walls; // [4 walls][variable enemy count]

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        GenerateAllWalls();
    }

    [ContextMenu("Generate All Walls")]
    public void GenerateAllWalls()
    {
        ClearAllWalls();

        walls = new GameObject[4][];

        // Tạo từng tường với số lượng enemy riêng
        if (generateTop)
            walls[0] = GenerateWall(WallDirection.Top, topWallEnemyCount);
        else
            walls[0] = new GameObject[0];

        if (generateBottom)
            walls[1] = GenerateWall(WallDirection.Bottom, bottomWallEnemyCount);
        else
            walls[1] = new GameObject[0];

        if (generateLeft)
            walls[2] = GenerateWall(WallDirection.Left, leftWallEnemyCount);
        else
            walls[2] = new GameObject[0];

        if (generateRight)
            walls[3] = GenerateWall(WallDirection.Right, rightWallEnemyCount);
        else
            walls[3] = new GameObject[0];

        int totalEnemies = walls[0].Length + walls[1].Length + walls[2].Length + walls[3].Length;
        Debug.Log($"Generated 4 walls with total {totalEnemies} enemies " +
                  $"(Top:{walls[0].Length}, Bottom:{walls[1].Length}, Left:{walls[2].Length}, Right:{walls[3].Length})");
    }

    GameObject[] GenerateWall(WallDirection direction, int enemyCount)
    {
        if (enemyCount <= 0) return new GameObject[0];

        GameObject[] wallEnemies = new GameObject[enemyCount];
        Vector3 playerPos = player.position;

        // Khoảng cách giữa các enemy
        float spacing = (halfWallLength * 2f) / Mathf.Max(1, enemyCount - 1);

        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 position = GetEnemyPosition(direction, i, spacing, playerPos);

            GameObject enemy;

            if (useObjectPool && ObjectPool.instance != null)
            {
                enemy = ObjectPool.instance.GetObject(enemyPrefab, transform);
                enemy.transform.position = position;
            }
            else
            {
                enemy = Instantiate(enemyPrefab, position, Quaternion.identity, transform);
            }

            enemy.name = $"Wall_{direction}_{i}";
            wallEnemies[i] = enemy;
        }

        return wallEnemies;
    }

    Vector3 GetEnemyPosition(WallDirection direction, int index, float spacing, Vector3 playerPos)
    {
        float offset = -halfWallLength + (index * spacing);

        switch (direction)
        {
            case WallDirection.Top:
                return playerPos + new Vector3(offset, distanceFromPlayer, 0);

            case WallDirection.Bottom:
                return playerPos + new Vector3(offset, -distanceFromPlayer, 0);

            case WallDirection.Left:
                return playerPos + new Vector3(-distanceFromPlayer, offset, 0);

            case WallDirection.Right:
                return playerPos + new Vector3(distanceFromPlayer, offset, 0);

            default:
                return playerPos;
        }
    }

    [ContextMenu("Clear All Walls")]
    public void ClearAllWalls()
    {
        if (walls == null) return;

        foreach (var wall in walls)
        {
            if (wall != null)
            {
                foreach (var enemy in wall)
                {
                    if (enemy != null)
                    {
                        if (useObjectPool && ObjectPool.instance != null)
                            ObjectPool.instance.DelayReturnToPool(enemy);
                        else
                            Destroy(enemy);
                    }
                }
            }
        }

        walls = null;
    }

    // Public methods để thay đổi số lượng enemy từ code
    public void SetTopWallEnemyCount(int count)
    {
        topWallEnemyCount = Mathf.Max(0, count);
        GenerateAllWalls();
    }

    public void SetBottomWallEnemyCount(int count)
    {
        bottomWallEnemyCount = Mathf.Max(0, count);
        GenerateAllWalls();
    }

    public void SetLeftWallEnemyCount(int count)
    {
        leftWallEnemyCount = Mathf.Max(0, count);
        GenerateAllWalls();
    }

    public void SetRightWallEnemyCount(int count)
    {
        rightWallEnemyCount = Mathf.Max(0, count);
        GenerateAllWalls();
    }

    public void SetAllWallEnemyCount(int count)
    {
        topWallEnemyCount = count;
        bottomWallEnemyCount = count;
        leftWallEnemyCount = count;
        rightWallEnemyCount = count;
        GenerateAllWalls();
    }

    void OnDrawGizmos()
    {
        if (!showGizmos || player == null) return;

        Vector3 playerPos = player.position;

        // Vẽ Top Wall
        if (generateTop)
        {
            Gizmos.color = Color.red;
            DrawWallGizmo(WallDirection.Top, topWallEnemyCount, playerPos);
        }

        // Vẽ Bottom Wall
        if (generateBottom)
        {
            Gizmos.color = Color.blue;
            DrawWallGizmo(WallDirection.Bottom, bottomWallEnemyCount, playerPos);
        }

        // Vẽ Left Wall
        if (generateLeft)
        {
            Gizmos.color = Color.yellow;
            DrawWallGizmo(WallDirection.Left, leftWallEnemyCount, playerPos);
        }

        // Vẽ Right Wall
        if (generateRight)
        {
            Gizmos.color = Color.magenta;
            DrawWallGizmo(WallDirection.Right, rightWallEnemyCount, playerPos);
        }

        // Player position
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(playerPos, 0.3f);
    }

    void DrawWallGizmo(WallDirection direction, int enemyCount, Vector3 playerPos)
    {
        if (enemyCount <= 0) return;

        float spacing = (halfWallLength * 2f) / Mathf.Max(1, enemyCount - 1);

        // Vẽ line của tường
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.zero;

        switch (direction)
        {
            case WallDirection.Top:
                start = playerPos + new Vector3(-halfWallLength, distanceFromPlayer, 0);
                end = playerPos + new Vector3(halfWallLength, distanceFromPlayer, 0);
                break;
            case WallDirection.Bottom:
                start = playerPos + new Vector3(-halfWallLength, -distanceFromPlayer, 0);
                end = playerPos + new Vector3(halfWallLength, -distanceFromPlayer, 0);
                break;
            case WallDirection.Left:
                start = playerPos + new Vector3(-distanceFromPlayer, -halfWallLength, 0);
                end = playerPos + new Vector3(-distanceFromPlayer, halfWallLength, 0);
                break;
            case WallDirection.Right:
                start = playerPos + new Vector3(distanceFromPlayer, -halfWallLength, 0);
                end = playerPos + new Vector3(distanceFromPlayer, halfWallLength, 0);
                break;
        }

        Gizmos.DrawLine(start, end);

        // Vẽ vị trí các enemy
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 pos = GetEnemyPosition(direction, i, spacing, playerPos);
            Gizmos.DrawWireSphere(pos, 0.2f);
        }
    }

    void OnDestroy()
    {
        ClearAllWalls();
    }
}

public enum WallDirection
{
    Top = 0,
    Bottom = 1,
    Left = 2,
    Right = 3
}
