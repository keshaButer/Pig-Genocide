using UnityEngine;

public class BulletEnemy : Bullet
{
    protected override void HandleHit(Transform other)
    {
        if (_isExplode)
        return;

        if (other.GetComponent<ParryCheckBox>())
        {
            other.GetComponent<ParryCheckBox>()?.HandleBullet(this);
            return;
        }

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
            if (other.gameObject.GetComponent<BulletPlayer>()) return;
            if (other.gameObject.GetComponent<Enemy>()) return;

            other.GetComponent<IDamagable>()?.ApplyDamage(Damage);

            if (other.GetComponent<Rigidbody2D>())
            {
                other.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * _force,
                 transform.position, ForceMode2D.Impulse);
            }

            Explosion();
        }
    }
}
