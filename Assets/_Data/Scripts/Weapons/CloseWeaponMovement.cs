using UnityEngine;

public class CloseWeaponMovement : TakeDamaged
{
    [SerializeField] float detectionRadius = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent<IDamagable>(out var damagable))
                Attack(damagable);

            ObjectPool.instance.DelayReturnToPool(gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
