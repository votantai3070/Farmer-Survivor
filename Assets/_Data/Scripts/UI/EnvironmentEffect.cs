using UnityEngine;

public class EnvironmentEffect : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] GameObject[] environmentEffectPrefab;

    private GameObject currentEffect;

    private void Start()
    {
        if (player == null)
            player = GameObject.Find("Player").GetComponent<PlayerController>();

        RandomEnvironmentEffect();
    }

    private void RandomEnvironmentEffect()
    {
        int random = Random.Range(0, environmentEffectPrefab.Length);

        currentEffect = environmentEffectPrefab[random];

        GameObject currentEnviPrefab = ObjectPool.instance.GetObject(currentEffect);
        currentEnviPrefab.transform.position = Vector2.Lerp(currentEnviPrefab.transform.position, player.transform.position, 0.2f);
    }
}
