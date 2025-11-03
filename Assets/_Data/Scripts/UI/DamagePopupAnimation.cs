using TMPro;
using UnityEngine;

public class DamagePopupAnimation : MonoBehaviour
{
    [SerializeField] AnimationCurve opacityCurve;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve heightCurve;
    [SerializeField] AnimationCurve widthCurve;

    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;

    private void Start()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;

        Invoke(nameof(ReturnDamagePopupToPool), 1f);
    }

    void Update()
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(widthCurve.Evaluate(time), 1 + heightCurve.Evaluate(time), 0);
        time += Time.deltaTime;
    }

    private void ReturnDamagePopupToPool()
    {
        ObjectPool.instance.DelayReturnToPool(gameObject);
    }
}
