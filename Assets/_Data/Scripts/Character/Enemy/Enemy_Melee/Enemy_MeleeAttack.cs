using System.Collections;
using UnityEngine;

public class Enemy_MeleeAttack : MonoBehaviour
{
    public WeaponData enemyWeaponData;
    [SerializeField] GameObject hitEffectPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("collision: " + collision);

        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<IDamagable>(out var damagable))
            {
                int randomValue = Random.Range(enemyWeaponData.firstDamage, enemyWeaponData.lastDamage + 1);

                GameObject effect = ObjectPool.instance.GetObject(hitEffectPrefab);
                effect.transform.position = collision.transform.position;

                damagable.TakeDamage(randomValue);

                StartCoroutine(ReturnPool(effect));
            }
        }
    }

    IEnumerator ReturnPool(GameObject effect)
    {
        yield return new WaitForSeconds(1f);
        ObjectPool.instance.DelayReturnToPool(effect);
    }
}
