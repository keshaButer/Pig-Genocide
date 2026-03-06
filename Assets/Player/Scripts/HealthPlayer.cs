using System;
using UnityEngine;

public class HealthPlayer : MonoBehaviour, IDamagable
{
    [Range(0, 15)]
    [SerializeField] int _startHealth;
    [SerializeField] float _timeToDestroy;
    Rigidbody2D _rb;
    BoxCollider2D _capsule;
    Transform _weaponHandler;
    [SerializeField] bool _isImmortalMode;
    private AnimationController _animationController;
    private int _health;
    private bool _wasDeath;
    [SerializeField] public PhysicsMaterial2D alivePhysicsMaterial, deadPhysicsMaterial;
    public int Health
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

                HealthWindow.SingleTon.UpdateHealthText();
            }
        }
    }
    void Awake()
    {
        _capsule = GetComponent<BoxCollider2D>();
        _capsule.sharedMaterial = alivePhysicsMaterial;
        _weaponHandler = transform.GetChild(2);
        EventManager.EnemyDied += () => AddHP(1);
    }
    void Start() => Initialize();
    public void Initialize()
    {
        _rb = GetComponent<Rigidbody2D>();
        Health = _startHealth;
        HealthWindow.SingleTon.UpdateHealthText();
        _animationController = transform.GetChild(0).GetComponent<AnimationController>();
    }
    public void AddHP(int hp)
    {
        Health += hp;
        Health = Math.Clamp(_health, 0, _startHealth);
    }
    private void Die()
    {
        _wasDeath = true;

        SetActiveComponents(false);

        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.linearVelocity = new Vector3(0, 0, 0);
        _rb.mass = 0.5f;
        _capsule.sharedMaterial = deadPhysicsMaterial;
        _weaponHandler.GetComponent<PlayerWeaponHandler>().enabled = false;
        _weaponHandler.gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        Rigidbody2D _weaponRb = _weaponHandler.gameObject.AddComponent<Rigidbody2D>();

        _weaponRb.simulated = true;
        _weaponRb.gravityScale = 3;
        _weaponRb.interpolation = RigidbodyInterpolation2D.Interpolate;
        _weaponHandler.parent = null;

        EventManager.OnPlayerDied();

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
        //SceneManager.LoadScene(0);
        Destroy(gameObject);
        print("ваще смерть");
    }

    public void ApplyDamage(int damage)
    {
        if (!_wasDeath)
        {
            damage = Math.Clamp(damage, 0, 5);
            Health -= damage;

            EventManager.OnPlayerTookDamage();
        }
    }
}
