using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] float speed, delayDestroy, radius;
    [SerializeField] public int damage;
    [SerializeField] protected float force;
    [SerializeField] private LayerMask layerMask;
    private Animator animator;
    private bool isExplode;
    public bool isParry;
    private float timer;
    private void Awake() => animator = GetComponent<Animator>();
    private void Update()
    {
        CheckHit();
        if (!isExplode && !isParry)
            Move();
        else if (isParry && !isExplode)
            ParryBackRun();
    }
    private void Move()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= delayDestroy)
            Destroy(gameObject);
    }
    private void ParryBackRun()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    private void Explode() => Destroy(gameObject);
    protected virtual void Explosion()
    {
        isExplode = true;
        animator.SetTrigger("isExplode");
    }
    private void CheckHit()
    {
        Collider2D other = Physics2D.OverlapCircle(transform.position, radius);
        if (other != null)
        {
            print($"BULLET HIT {other.gameObject.name}");
            HandleHit(other.transform);
        }
    }
    protected abstract void HandleHit(Transform other);
}
