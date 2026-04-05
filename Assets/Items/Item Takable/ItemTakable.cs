using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ItemTakable : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private float _minTextSize = 10, _maxTextSize = 20, _textSizeMultiplier = 4;
    [SerializeField] private TextMeshPro _text;

    private SpriteRenderer _spriteRenderer;
    protected Transform PlayerTransform;

    private void OnEnable() => PlayerSpawner.OnPlayerSpawned += Initialize; //поменять потом
    private void OnDisable() => PlayerSpawner.OnPlayerSpawned -= Initialize;

    private void Initialize(GameObject player)
    {
        InitializeSpriteRenderer();

        PlayerTransform = player.transform;

        _text.text = _item.name;
    }
    private void InitializeSpriteRenderer()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_item.Icon != null)
            _spriteRenderer.sprite = _item.Icon;

        _spriteRenderer.sortingOrder = 4; // поменять потом
    }

    private void FixedUpdate()
    {
        SetTextSizeByDistance();
    }
    private void SetTextSizeByDistance()
    {
        if (PlayerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, PlayerTransform.position);
            float distanceMultiplier = Mathf.Clamp(distance, _minTextSize, _maxTextSize);

            _text.fontSize = distanceMultiplier * _textSizeMultiplier;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Inventory inventory;
        if (other.TryGetComponent<Inventory>(out inventory))
        {
            // Inventory.SingleTone.AddItem(item);
            inventory.AddItem(_item);
            Destroy(gameObject);
        }
    }
}
