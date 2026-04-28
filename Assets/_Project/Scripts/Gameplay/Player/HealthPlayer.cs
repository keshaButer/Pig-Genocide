using VContainer;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthPlayer : MonoBehaviour, IDamagable
{
    [Range(0, 15)]
    [SerializeField] public int MaxHealth;
    [SerializeField] float _timeToDestroy;
    Rigidbody2D _rb;
    BoxCollider2D _capsule;
    Transform _weaponHandler;
    [SerializeField] bool _isImmortalMode;
    private AnimationController _animationController;
    private int _health;
    private bool _wasDeath;
    [SerializeField] public PhysicsMaterial2D alivePhysicsMaterial, deadPhysicsMaterial;
    [Inject] private IHealthWindow _healthWindow;

    [Inject] private IEnemyEvents _enemyEvents;
    [Inject] private IPlayerStateEvents _playerStateEvents;

    public int CurrentHealth
    {
        get { return _health; }
        private set
        {
            if (!_isImmortalMode)
            {
                if (value <= 0)
                {
                    _health = 0;
                    if (!_wasDeath)
                        Die();
                }
                else _health = value;

                _healthWindow.UpdateHealthText();
            }
        }
    }

    private void Awake()
    {
        _capsule = GetComponent<BoxCollider2D>();
        _capsule.sharedMaterial = alivePhysicsMaterial;
        _weaponHandler = transform.GetChild(2);
        _enemyEvents.OnEnemyDied += () => AddHP(1);
    }

    private void Start() => Initialize();

    public void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        CurrentHealth = MaxHealth;
        _healthWindow.UpdateHealthText();
        _animationController = transform.GetChild(0).GetComponent<AnimationController>();
    }
    public void AddHP(int hp)
    {
        CurrentHealth += hp;
        CurrentHealth = Math.Clamp(_health, 0, MaxHealth);
    }
    private void Die()
    {
        _wasDeath = true;

        SetActiveComponents(false);

        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.linearVelocity = new Vector3(0, 0, 0);
        _rb.mass = 0.5f;
        _capsule.sharedMaterial = deadPhysicsMaterial;
        _weaponHandler.GetComponent<WeaponHandler>().enabled = false;
        _weaponHandler.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        Rigidbody2D _weaponRb = _weaponHandler.gameObject.AddComponent<Rigidbody2D>();

        _weaponRb.simulated = true;
        _weaponRb.gravityScale = 3;
        _weaponRb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _weaponHandler.parent = null;

        _playerStateEvents.NotifyDied();

        Invoke(nameof(Kill), _animationController.Death());
    }

    public void SetActiveComponents(bool turn)
    {
        GetComponent<MovementPlayer>().enabled = turn;
        GetComponent<InteractPlayer>().enabled = turn;
        GetComponent<Inventory>().enabled = turn;
    }

    private void Kill()
    {
        Destroy(gameObject);
        SceneManager.LoadScene(0);
        Invoke(nameof(RestartGame), 4);
    }
    private void RestartGame() => SceneManager.LoadScene(0);

    public void ApplyDamage(int damage)
    {
        if (!_wasDeath)
        {
            damage = Math.Clamp(damage, 0, 5);
            CurrentHealth -= damage;

            _playerStateEvents.NotifyTookDamage();
        }
    }
}
