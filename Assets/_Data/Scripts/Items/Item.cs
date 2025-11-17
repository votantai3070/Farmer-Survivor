using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemData itemData;
    protected SpriteRenderer spriteRenderer;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        spriteRenderer.sprite = GameManager.instance.itemAtlas.GetSprite(itemData.itemName);
    }
}
