using UnityEngine;

public abstract class Explosives : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float radiusExplosion, force;
    [SerializeField] LayerMask damagableMask, obstacleMask;

    private bool isExploded;
    private RaycastHit2D hit;
    private Ray rayToCollider;

    private void Awake()
    {
        damagableMask = LayerMask.GetMask("Player", "Enemy", "Explodeable");
    }
    protected virtual void DealDamage()
    {
        isExploded = true;

        EventManager.OnExplosion();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radiusExplosion, damagableMask);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                Vector2 direction = collider.gameObject.transform.position - transform.position;
                direction.Normalize();

                if (!Physics2D.Raycast(transform.position, direction, radiusExplosion, obstacleMask))
                {
                    if (collider.transform.GetComponent<Explosives>())
                    {
                        if (!collider.transform.GetComponent<Explosives>().isExploded)
                            collider.GetComponent<IDamagable>()?.ApplyDamage(damage);
                    }
                    else 
                    {
                        collider.GetComponent<IDamagable>()?.ApplyDamage(damage);
                    }

                    if (collider.attachedRigidbody != null)
                    {
                        collider.transform.GetComponent<MovementPlayer>()?.SetRigidBody();

                        collider.transform.GetComponent<Rigidbody2D>().AddForceAtPosition(direction
                        * force, collider.transform.position + new Vector3(0, 0.5f, 0), ForceMode2D.Impulse);

                        collider.transform.GetComponent<MovementPlayer>()?.ResetRigidBody();
                    }
                }
            }
        }
    }
    protected abstract void Explode();
}
