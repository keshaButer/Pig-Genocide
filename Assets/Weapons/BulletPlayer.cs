using UnityEngine;

public class BulletPlayer : Bullet
{
    [SerializeField] private LayerMask _ignoreMask;

    protected override void HandleHit(Transform other)
    {
        if (_isExplode)
            return;

        // if (other.gameObject.layer != 2 && other.gameObject.tag != "Player" &&
        //  other.gameObject.layer != 6 && other.gameObject.layer != 10 && other.gameObject.)
        if (CanHit(other.gameObject))
        {
            other.GetComponent<IDamagable>()?.ApplyDamage(Damage);
            if (other.GetComponent<Rigidbody2D>() != null)
                other.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * _force,
                 transform.position, ForceMode2D.Impulse);
            
            Explosion();
        }
    }
    private bool CanHit(GameObject other)
    {
        int layer = other.layer;
        int layerMaskValue = 1 << layer;
        
        if ((_ignoreMask.value & layerMaskValue) == 0)
            return true;
            
        return false;
    }
}
