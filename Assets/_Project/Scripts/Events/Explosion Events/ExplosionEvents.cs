public class ExplosionEvents : IExplosionEvents
{
    public event System.Action OnExplosion;
    public void NotifyExplosionPerformed() => OnExplosion?.Invoke();
}