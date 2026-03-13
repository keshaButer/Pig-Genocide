public interface IDamagable
{
    int CurrentHealth { get; }
    void ApplyDamage(int damage);
}
