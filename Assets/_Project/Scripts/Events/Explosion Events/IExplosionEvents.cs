public interface IExplosionEvents
{
    event System.Action OnExplosion;
    void NotifyExplosionPerformed();
}