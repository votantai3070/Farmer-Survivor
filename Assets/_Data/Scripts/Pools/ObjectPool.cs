using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] private int poolSize = 5;
    private Dictionary<GameObject, Queue<GameObject>> poolDict = new Dictionary<GameObject, Queue<GameObject>>();

    #region Prefabs
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject ratPrefab;
    [SerializeField] private GameObject batPrefab;
    [SerializeField] private GameObject undead1Prefab;
    [SerializeField] private GameObject undead2Prefab;
    [SerializeField] private GameObject undead3Prefab;
    [SerializeField] private GameObject bonePrefab;
    [SerializeField] private GameObject golemPrefab;

    [Header("Effect Prefabs")]
    [SerializeField] private GameObject damagePrefab;
    [SerializeField] private GameObject hitEffectPrefab;
    #endregion

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeAllPools();
    }

    private void InitializeAllPools()
    {
        if (ratPrefab != null) InitializeNewPool(ratPrefab);
        if (batPrefab != null) InitializeNewPool(batPrefab);
        if (undead1Prefab != null) InitializeNewPool(undead1Prefab);
        if (undead2Prefab != null) InitializeNewPool(undead2Prefab);
        if (undead3Prefab != null) InitializeNewPool(undead3Prefab);
        if (bonePrefab != null) InitializeNewPool(bonePrefab);
        if (golemPrefab != null) InitializeNewPool(golemPrefab);
        if (damagePrefab != null) InitializeNewPool(damagePrefab);
        if (hitEffectPrefab != null) InitializeNewPool(hitEffectPrefab);
    }

    private void InitializeNewPool(GameObject prefab)
    {
        if (prefab == null) return;

        poolDict[prefab] = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateNewPoolObject(prefab);
        }
    }

    private void CreateNewPoolObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform);

        PooledObject pooledObj = obj.GetComponent<PooledObject>();
        if (pooledObj == null)
        {
            pooledObj = obj.AddComponent<PooledObject>();
        }
        pooledObj.SetPrefab(prefab);

        obj.SetActive(false);
        poolDict[prefab].Enqueue(obj);
    }

    public GameObject GetObject(GameObject prefab, Transform parent = null)
    {
        if (prefab == null)
        {
            Debug.LogError("Trying to get object with null prefab!");
            return null;
        }

        if (!poolDict.ContainsKey(prefab))
        {
            InitializeNewPool(prefab);
        }

        if (poolDict[prefab].Count == 0)
        {
            CreateNewPoolObject(prefab);
        }

        GameObject objectToGet = poolDict[prefab].Dequeue();

        objectToGet.SetActive(true);
        objectToGet.transform.SetParent(parent, false);

        return objectToGet;
    }

    #region Return Pool

    public void ReturnToPool(GameObject objectToReturn)
    {
        if (objectToReturn == null)
        {
            Debug.LogWarning("Trying to return null object to pool!");
            return;
        }

        ReturnPool(objectToReturn);
    }

    private void ReturnPool(GameObject objectToReturn)
    {
        // Validate object
        if (!ValidatePoolObject(objectToReturn))
            return;

        PooledObject pooledObj = objectToReturn.GetComponent<PooledObject>();
        GameObject originalPrefab = pooledObj.prefab;

        if (!poolDict.ContainsKey(originalPrefab))
        {
            InitializeNewPool(originalPrefab);
        }

        objectToReturn.SetActive(false);

        // ✅ DÙNG SetParent thay vì gán parent trực tiếp
        objectToReturn.transform.SetParent(transform, false);

        poolDict[originalPrefab].Enqueue(objectToReturn);
    }

    private bool ValidatePoolObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("Object is null!");
            return false;
        }

#if UNITY_EDITOR
        // Check nếu là prefab asset
        if (PrefabUtility.IsPartOfPrefabAsset(obj))
        {
            Debug.LogError($"Cannot return prefab asset '{obj.name}' to pool! Must be an instance.");
            return false;
        }
#endif

        // Check nếu không phải scene object
        if (obj.scene.name == null)
        {
            Debug.LogError($"Object '{obj.name}' is not in a scene! Cannot return to pool.");
            return false;
        }

        PooledObject pooledObj = obj.GetComponent<PooledObject>();
        if (pooledObj == null)
        {
            Debug.LogError($"Object '{obj.name}' doesn't have PooledObject component!");
            return false;
        }

        if (pooledObj.prefab == null)
        {
            Debug.LogError($"PooledObject on '{obj.name}' has null prefab reference!");
            return false;
        }

        return true;
    }

    public void DelayReturnToPool(GameObject objectToReturn, float delay = 0.01f)
    {
        if (objectToReturn == null)
        {
            Debug.LogWarning("Trying to delay return null object!");
            return;
        }

        StartCoroutine(DelayReturnCoroutine(delay, objectToReturn));
    }

    private IEnumerator DelayReturnCoroutine(float delay, GameObject objectToReturn)
    {
        yield return new WaitForSeconds(delay);

        if (objectToReturn != null)
        {
            ReturnPool(objectToReturn);
        }
    }

    #endregion
}
