using UnityEngine;

public class BulletPlayer : Bullet
{
    protected override void HandleHit(Transform other)
    {
        if (_isExplode)
            return;

        if (CanHit(other.gameObject))
        {
            other.GetComponent<IDamagable>()?.ApplyDamage(Damage);
            if (other.GetComponent<Rigidbody2D>() != null)
                other.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * _force,
                 transform.position, ForceMode2D.Impulse);
            
            Explosion();
        }
    }
}
