public interface IPlayerStateEvents
{
    event System.Action OnTookDamage;
    event System.Action OnDied;

    void NotifyTookDamage();
    void NotifyDied();
}