using UnityEngine;

public class PooledObject : MonoBehaviour
{
    public GameObject prefab;

    public void SetPrefab(GameObject prefab)
    {
        this.prefab = prefab;
    }
}
