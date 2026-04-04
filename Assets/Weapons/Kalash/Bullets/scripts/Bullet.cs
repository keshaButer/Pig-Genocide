using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class Bullet : MonoBehaviour
{
    public int Damage;
    public bool IsParry;
    public float Speed { get; set; }

    protected bool _isExplode;
    [SerializeField] protected float _force;

    [SerializeField] private float _speed, _delayDestroy, _radius;
    [SerializeField] private LayerMask _layerMask;

    private Animator _animator;
    private CircleCollider2D _circleCollider;
    private float _timer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _circleCollider = GetComponent<CircleCollider2D>();
        _circleCollider.isTrigger = true;
        _circleCollider.radius = _radius;
    }

    private void Update()
    {
        if (_isExplode) return;

        if (IsParry)
            ParryBackRun();
        else
            Move();
    }
    private void Move()
    {
        transform.Translate(Vector2.right * _speed * Time.deltaTime);
        _timer += Time.deltaTime;
        if (_timer >= _delayDestroy)
            Destroy(gameObject);
    }
    private void ParryBackRun()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime);
    }
    private void Explode() => Destroy(gameObject);
    protected virtual void Explosion()
    {
        _isExplode = true;
        _animator.SetTrigger("isExplode");
    }
    private void CheckHit(Collider2D collider)
    {
        print($"BULLET HIT {collider.gameObject.name}");
        HandleHit(collider.transform);

        if (collider.tag == "Ground")
        {
            // ChunkedLevelGenerator.SingleTon.DestroyTileAtWorldPosition(hitInside);
            ChunkedLevelGenerator.SingleTon.DestroyTilesInRadius(transform.position, _radius);
        }
    }
    protected abstract void HandleHit(Transform other);
    private void OnTriggerEnter2D(Collider2D other) => CheckHit(other);
}
