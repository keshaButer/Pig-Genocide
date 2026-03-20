using TMPro;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class ItemTakable : MonoBehaviour
{
    [SerializeField] private float _minTextSize = 10, _maxTextSize = 20, _textSizeMultiplier = 4;
    [SerializeField] private TextMeshPro _text;

    private Item _item;
    private SpriteRenderer _spriteRenderer;
    private Transform _playerTransform;

    private void OnEnable() => MovementPlayer.OnPlayerSpawned += Initialize; //поменять потом
    private void OnDisable() => MovementPlayer.OnPlayerSpawned -= Initialize;

    private void Initialize()
    {
        InitializeSpriteRenderer();

        // playerPos = ServiceLocator.Current.Get<MovementPlayer>().transform;
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform; //поменять потом

        _text.text = _item.name;
        
        Debug.Log($"Item {_item.name} initialized.");
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
        if (_playerTransform != null)
        {
            float distance = Vector2.Distance(transform.position, _playerTransform.position);
            float distanceMultiplier = Mathf.Clamp(distance, _minTextSize, _maxTextSize);

            _text.fontSize = distanceMultiplier * _textSizeMultiplier;
        }
    }
    
    protected abstract void SetItemObjectToHandler(WeaponHandler weaponHandler);

    private void OnTriggerEnter2D(Collider2D other)
    {
        WeaponHandler weaponHandler;
        if (other.TryGetComponent<WeaponHandler>(out weaponHandler))
        {
            // Inventory.SingleTone.AddItem(item);
            SetItemObjectToHandler(weaponHandler);
            Destroy(gameObject);
        }
    }
}
