using UnityEngine;

public class BulletItemController : Item
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        spriteRenderer.sprite = GameManager.instance.UIAtlas.GetSprite(itemData.itemName);
    }
}
