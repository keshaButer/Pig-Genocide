public interface IDamagable
{
    int Health { get; }
    void ApplyDamage(int damage);
}
