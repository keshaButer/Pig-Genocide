using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] protected int maxHealth;
    [SerializeField] private float _delayToDestroy;
    private AudioSource _audioSource;
    private AudioClip _deathScreamSound;
    private int _health;
    [field: SerializeField] public int Damage { get; private set; }
    protected Rigidbody2D rb;
    private BoxCollider2D collider2d;
    protected bool isDeath { get; private set; }
    public enum States { chill = 0, fight = 1 }
    public States CurrrentState { get; protected set; }
    public int Health
    {
        get { return _health; }
        protected set
        {
            _health = value;
            if (value <= 0)
            {
                Die();
                _health = 0;
            } 
        }
    }
    protected virtual void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _deathScreamSound = Resources.Load<AudioClip>("Sounds/Enemy/death_scream");
        collider2d = GetComponent<BoxCollider2D>();
    }
    private void Die()
    {
        EventManager.OnEnemyDied();

        isDeath = true;

        collider2d.enabled = false;

        rb.constraints = RigidbodyConstraints2D.None;
        rb.linearVelocity = new Vector3(0, 0, 0);
        rb.mass = 0.5f;

        _audioSource.PlayOneShot(_deathScreamSound);

        DisableComponents();
        print("Something DEATH");

        Invoke(nameof(DestroyEnemy), _delayToDestroy);
    }
    protected virtual void Attack() { }
    protected virtual void Attack(Collision2D other) { }
    public void ApplyDamage(int damage) { Health -= damage; }
    protected virtual void SetState(States _state) => CurrrentState = _state;
    protected virtual void DisableComponents() { }
    private void DestroyEnemy() => Destroy(gameObject);
}
