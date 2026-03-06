using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class ItemTakable : InteractableObject
{
    [SerializeField] public Item item;
    private SpriteRenderer renderer2 => GetComponent<SpriteRenderer>();
    [SerializeField] float minTextSize = 10, maxTextSize = 20;
    private TextMeshPro text;
    private float playerDistance;
    private Transform playerPos;
    private void Awake()
    {
        if (item.icon != null)
            renderer2.sprite = item.icon;
        renderer2.sortingOrder = 4;
        text = transform.GetChild(0).GetComponent<TextMeshPro>();
        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
        text.text = item.name;
    }
    private void FixedUpdate()
    {
        if (playerPos != null)
        {
            playerDistance = Vector2.Distance(transform.position, playerPos.position);
            playerDistance = Mathf.Clamp(playerDistance, minTextSize, maxTextSize);
            text.fontSize = playerDistance + 4;
        }
    }
    public override void Interact()
    {
        if (Inventory.SingleTone.items.Count < Inventory.SingleTone.maxItemsCount)
        {
            Inventory.SingleTone.AddItem(item);
            Use();
            Destroy(gameObject);
        }
    }
    public virtual void Use() {}
}
