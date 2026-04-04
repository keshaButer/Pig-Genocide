using UnityEngine;

public class BulletEnemy : Bullet
{
    protected override void HandleHit(Transform other)
    {
        if (_isExplode)
            return;

        other.GetComponent<ParryCheckBox>()?.HandleBullet(this);

        if (other.GetComponent<Bullet>()) 
            Explosion();

        if (CanHit(other.gameObject))
        {
            if (IsParry)
            {
                other.GetComponent<IDamagable>()?.ApplyDamage(Damage * 2);
                if (other.GetComponent<Rigidbody2D>())
                {
                    other.GetComponent<Rigidbody2D>().AddForceAtPosition(-transform.right * _force,
                     transform.position, ForceMode2D.Impulse);
                }

                Explosion();
            }
            else
            {
                if (!other.gameObject.GetComponent<Enemy>())
                    other.GetComponent<IDamagable>()?.ApplyDamage(Damage);

                if (other.GetComponent<Rigidbody2D>())
                {
                    other.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * _force,
                     transform.position, ForceMode2D.Impulse);
                }

                if(!other.gameObject.GetComponent<Enemy>() && !other.GetComponent<ParryCheckBox>())
                    Explosion();
            }
        } 
    }
}
