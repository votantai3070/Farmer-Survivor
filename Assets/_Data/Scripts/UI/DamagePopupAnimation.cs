using System.Collections;
using TMPro;
using UnityEngine;

public class DamagePopupAnimation : MonoBehaviour
{
    [SerializeField] private AnimationCurve opacityCurve;
    [SerializeField] private AnimationCurve scaleCurve;
    [SerializeField] private AnimationCurve heightCurve;
    [SerializeField] private AnimationCurve widthCurve;

    [SerializeField] private float duration = 1f;

    private TextMeshProUGUI tmp;
    private Vector3 startPosition;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        if (tmp == null)
            tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // ✅ Lưu position ngay khi enable
        startPosition = transform.position;

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(PlayAnimation());
    }

    private void OnDisable()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    private IEnumerator PlayAnimation()
    {
        float elapsed = 0;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            if (tmp != null)
            {
                tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, opacityCurve.Evaluate(t));
                transform.localScale = Vector3.one * scaleCurve.Evaluate(t);

                // ✅ Dùng startPosition đã lưu
                float x = widthCurve.Evaluate(t);
                float y = 1 + heightCurve.Evaluate(t);
                transform.position = startPosition + new Vector3(x, y, 0);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        if (ObjectPool.instance != null)
        {
            ObjectPool.instance.DelayReturnToPool(gameObject);
        }
    }

    // ✅ Method public để set position từ bên ngoài
    public void SetStartPosition(Vector3 pos)
    {
        startPosition = pos;
        transform.position = pos;
    }
}
