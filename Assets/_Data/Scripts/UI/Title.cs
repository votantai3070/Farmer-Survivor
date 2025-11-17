using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    private Image image;

    [SerializeField] string titleSpriteName;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        image.sprite = GameManager.instance.UIAtlas.GetSprite(titleSpriteName);
    }
}
