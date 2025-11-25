using UnityEngine;

public class CloseWeaponMovement : TakeDamaged
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<IDamagable>(out var damagable))
                Attack(damagable);

            ObjectPool.instance.DelayReturnToPool(gameObject);
        }
    }
}
