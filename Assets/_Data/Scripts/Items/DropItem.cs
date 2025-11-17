using UnityEngine;

public class DropItem : MonoBehaviour
{
    [SerializeField] private GameObject bulletItemPrefab;
    [SerializeField] private GameObject speedItemPrefab;
    [SerializeField] private GameObject exp1Prefab;
    [SerializeField] private GameObject exp2Prefab;
    [SerializeField] private GameObject exp3Prefab;
    [SerializeField] private GameObject potionPrefab;

    private void Drop(GameObject prefab, Vector3 pos, Quaternion rot)
    {
        if (prefab == null) return;

        GameObject obj = ObjectPool.instance.GetObject(prefab);
        obj.transform.SetPositionAndRotation(pos, rot);
    }

    public void DropExp1(Transform transform) => Drop(exp1Prefab, transform.position, transform.rotation);
    public void DropExp2(Transform transform) => Drop(exp2Prefab, transform.position, transform.rotation);
    public void DropExp3(Transform transform) => Drop(exp3Prefab, transform.position, transform.rotation);
    void DropPotion(Transform transform) => Drop(potionPrefab, transform.position, transform.rotation);
    void DropSpeedItem(Transform transform) => Drop(speedItemPrefab, transform.position, transform.rotation);
    void DropBulletItem(Transform transform) => Drop(bulletItemPrefab, transform.position, transform.rotation);


    public void SetEnemyDropItem(Transform transform, CharacterData data)
    {
        bool droppedBullet = Random.value < data.dropBulletChange;
        bool droppedPotion = Random.value < data.dropPotionChange;
        bool droppedSpeed = Random.value < data.dropSpeedChange;

        if (droppedBullet)
            DropBulletItem(transform);

        else if (droppedPotion)
            DropPotion(transform);

        else if (droppedSpeed)
            DropSpeedItem(transform);
    }
}
