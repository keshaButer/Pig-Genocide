using System;
using VContainer;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class DeathHandler : MonoBehaviour
{
    [SerializeField] private float _destroyDelay;
    [SerializeField] private PhysicsMaterial2D _aliveMaterial;
    [SerializeField] private PhysicsMaterial2D _deadMaterial;
    [SerializeField] private MovementPlayer _movementPlayer;
    [SerializeField] private InteractPlayer _interactPlayer;
    [SerializeField] private Inventory _inventory;

    [SerializeField] private WeaponHandler _weaponHandler;
    [SerializeField] private PlayerAnimationController _animationController;

    [Inject] private IPlayerStateEvents _playerStateEvents;

    public event Action OnDeath;

    private IHealth _playerHealth;
    private Rigidbody2D _rigidBody;
    private BoxCollider2D _collider;

    private void Awake()
    {
        _playerHealth = GetComponent<IHealth>();

        if (_playerHealth != null)
            _playerHealth.OnHealthChanged += OnHealthChanged;

        _movementPlayer = GetComponent<MovementPlayer>();
        _interactPlayer = GetComponent<InteractPlayer>();
        _inventory = GetComponent<Inventory>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<BoxCollider2D>();
        _collider.sharedMaterial = _aliveMaterial;
    }

    private void OnHealthChanged(int health)
    {
        if (health != 0) return;

        _movementPlayer.enabled = false;
        _interactPlayer.enabled = false;
        _inventory.enabled = false;

        _rigidBody.constraints = RigidbodyConstraints2D.None;
        _rigidBody.linearVelocity = new Vector3(0, 0, 0);
        _rigidBody.mass = 0.5f;

        _collider.sharedMaterial = _deadMaterial;

// ВОТ ЭТО ДОЛЖЕН ОБРАБАТЫВАТЬ САМ WeaponHandler по событию OnDeath но я сейчас устал
        _weaponHandler.enabled = false;
        _weaponHandler.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        Rigidbody2D _weaponRb = _weaponHandler.gameObject.AddComponent<Rigidbody2D>();
        _weaponRb.simulated = true;
        _weaponRb.gravityScale = 3;
        _weaponRb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _weaponHandler.transform.parent = null;

        _animationController.TriggerDeath();
        Invoke(nameof(TriggerDeathEvent), _destroyDelay);
    }

    private void TriggerDeathEvent() => OnDeath?.Invoke();

    public void DestroyPlayer()
    {
        _playerStateEvents.NotifyDied();
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        _playerHealth.OnHealthChanged -= OnHealthChanged;
    }
}