using UnityEngine;

public class BulletPlayer : Bullet
{
    protected override void HandleHit(Transform other)
    {
        if (isExplode)
            return;

        if (other.gameObject.layer != 2 && other.gameObject.tag != "Player" &&
         other.gameObject.layer != 6 && other.gameObject.layer != 10)
        {
            other.GetComponent<IDamagable>()?.ApplyDamage(damage);
            if (other.GetComponent<Rigidbody2D>() != null)
                other.GetComponent<Rigidbody2D>()?.AddForceAtPosition(transform.right * force,
                 transform.position, ForceMode2D.Impulse);
            
            Explosion();
        }
    }
}
