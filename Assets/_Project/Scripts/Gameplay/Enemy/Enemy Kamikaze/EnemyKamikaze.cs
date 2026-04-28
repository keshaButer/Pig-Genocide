using UnityEngine;

[RequireComponent(typeof(EnemyCollision))]
public class EnemyKamikaze : EnemyRasher
{
    [SerializeField] private Bomb _bomb;

    public override void HandleCollision(Collision2D other) => Explode();

    protected override void OnDeath() => Explode();

    private void Explode()
    {
        _bomb.Explode();
        Destroy(gameObject);
    }
}