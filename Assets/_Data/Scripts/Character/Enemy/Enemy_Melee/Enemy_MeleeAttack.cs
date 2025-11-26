using UnityEngine;

public class Enemy_MeleeAttack : MonoBehaviour
{
    public WeaponData enemyWeaponData;
    [SerializeField] GameObject hitEffectPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<IDamagable>(out var damagable))
            {
                int randomValue = Random.Range(enemyWeaponData.firstDamage, enemyWeaponData.lastDamage + 1);

                GameObject effect = ObjectPool.instance.GetObject(hitEffectPrefab);
                effect.transform.position = collision.transform.position;

                damagable.TakeDamage(randomValue);

                ObjectPool.instance.DelayReturnToPool(effect, 1f);
            }
        }
    }
}
