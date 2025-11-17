using UnityEngine;
using DG.Tweening;

public class ChestController : MonoBehaviour
{
    [SerializeField] GameObject[] itemPrefab;

    [Space]

    [Header("Chest Setting")]
    private SpriteRenderer sr;
    private Sprite openedChestSprite;
    private Sprite closedChestSprite;

    public bool isOpened = false;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        closedChestSprite = GameManager.instance.UIAtlas.GetSprite("Box 0");
        openedChestSprite = GameManager.instance.UIAtlas.GetSprite("Box 1");
        sr.sprite = closedChestSprite;
    }

    public void OpenChest()
    {
        isOpened = true;

        sr.sprite = openedChestSprite;

        int random = Random.Range(0, itemPrefab.Length);

        var item = ObjectPool.instance.GetObject(itemPrefab[random]);
        item.transform.SetPositionAndRotation(transform.position + Vector3.up, transform.rotation);


        transform.tag = "Untagged";
    }

}
