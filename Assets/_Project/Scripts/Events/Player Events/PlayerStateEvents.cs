public class PlayerStateEvents : IPlayerStateEvents
{
    public event System.Action OnTookDamage;
    public event System.Action OnDied;

    public void NotifyTookDamage() => OnTookDamage?.Invoke();
    public void NotifyDied() => OnDied?.Invoke();
}