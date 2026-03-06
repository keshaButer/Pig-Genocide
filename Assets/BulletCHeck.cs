using UnityEngine;

public class BulletCHeck : CheckForCollisions
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Bullet>())
        {
            print("Collision");
        }
    }
}
