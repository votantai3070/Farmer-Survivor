using UnityEngine;

public class AutoReturnHitEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 1f; // Thời gian tồn tại

    private ParticleSystem ps;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        // Nếu có ParticleSystem, return khi particle stop
        if (ps != null)
        {
            // Set stop action
            var main = ps.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }
        else
        {
            // Không có particle, return sau 'lifetime' giây
            Invoke(nameof(ReturnToPool), lifetime);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnParticleSystemStopped()
    {
        // Callback khi particle stop
        ReturnToPool();
    }

    private void ReturnToPool()
    {
        if (ObjectPool.instance != null)
        {
            ObjectPool.instance.ReturnToPool(gameObject);
        }
    }
}
