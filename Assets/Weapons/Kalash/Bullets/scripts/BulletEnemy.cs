using UnityEngine;

public class BulletEnemy : Bullet
{
    protected override void HandleHit(Transform other)
    {
        if (isExplode)
            return;

        if (other.GetComponent<Bullet>()) Explosion();
        if (!other.GetComponent<CheckForCollisions>() && other.gameObject.layer != 2 && other.gameObject.layer != 6)
        {
            if (isParry)
            {
                other.GetComponent<IDamagable>()?.ApplyDamage(damage);
                if (other.GetComponent<Rigidbody2D>())
                {
                    other.GetComponent<Rigidbody2D>().AddForceAtPosition(-transform.right * force,
                     transform.position, ForceMode2D.Impulse);
                }

                Explosion();
            }
            else
            {
                if (!other.gameObject.GetComponent<Enemy>())
                    other.GetComponent<IDamagable>()?.ApplyDamage(damage);

                if (other.GetComponent<Rigidbody2D>())
                {
                    other.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * force,
                     transform.position, ForceMode2D.Impulse);
                }

                if(!other.gameObject.GetComponent<Enemy>())
                    Explosion();
            }
        } 
    }
}
