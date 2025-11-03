public class SpeedItemController : Item
{
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        spriteRenderer.sprite = GameManager.Instance.UIAtlas.GetSprite(itemData.itemName);
    }
}
