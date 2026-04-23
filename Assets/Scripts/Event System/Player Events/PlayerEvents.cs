public class PlayerEvents : IPlayerEvents
{
    public event System.Action OnPlayerDied;
    public event System.Action OnPlayerTookDamage;
    public event System.Action OnDash;
    public event System.Action OnDashDownKick;
    public event System.Action OnPlayerSitDown;
    public event System.Action OnPlayerStandUp;
    public event System.Action OnParry;

    public void NotifyPlayerSitDown() => OnPlayerSitDown?.Invoke();
    public void NotifyPlayerStandUp() => OnPlayerStandUp?.Invoke();
    public void NotifyDashPerformed() => OnDash?.Invoke();
    public void NotifyParryPerformed() => OnParry?.Invoke();
    public void NotifyDashDownKickPerformed() => OnDashDownKick?.Invoke();
    public void NotifyPlayerTookDamage() => OnPlayerTookDamage?.Invoke();
    public void NotifyPlayerDied() => OnPlayerDied?.Invoke();
}