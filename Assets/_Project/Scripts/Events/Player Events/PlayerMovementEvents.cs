public class PlayerMovementEvents : IPlayerMovementEvents
{
    public event System.Action OnDash;
    public event System.Action OnDashDownKick;
    public event System.Action OnSitDown;
    public event System.Action OnStandUp;

    public void NotifySitDown() => OnSitDown?.Invoke();
    public void NotifyStandUp() => OnStandUp?.Invoke();
    public void NotifyDash() => OnDash?.Invoke();
    public void NotifyDashDownKick() => OnDashDownKick?.Invoke();
}