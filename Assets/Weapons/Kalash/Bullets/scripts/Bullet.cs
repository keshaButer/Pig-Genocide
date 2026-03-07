using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] float speed, delayDestroy, radius;
    [SerializeField] public int damage;
    [SerializeField] protected float force;
    [SerializeField] private LayerMask layerMask;
    public float Speed { get; set; }
    private Vector2 previousPosition;
    private Animator animator;
    protected bool isExplode;
    public bool isParry;
    private float timer;
    private void Awake() => animator = GetComponent<Animator>();
    private void Start()
    {
        previousPosition = transform.position;
    }
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
        Vector2 direction = ((Vector2)transform.position - previousPosition).normalized;
        float distance = Vector2.Distance(transform.position, previousPosition);

        RaycastHit2D hit = Physics2D.Raycast(previousPosition, direction, distance, layerMask);

        if (hit.collider != null)
        {
            print($"BULLET HIT {hit.collider.gameObject.name}");
            Vector2 hitInside = hit.point + direction * 0.15f;
            HandleHit(hit.collider.transform);

            if (hit.collider.tag == "Ground")
            {
                ChunkedLevelGenerator.SingleTon.DestroyTileAtWorldPosition(hitInside);
            }
        }

        previousPosition = transform.position;
    }
    protected abstract void HandleHit(Transform other);
}
