public interface IPlayerMovementEvents
{
    event System.Action OnDash;
    event System.Action OnDashDownKick;
    event System.Action OnSitDown;
    event System.Action OnStandUp;

    void NotifySitDown();
    void NotifyStandUp();
    void NotifyDash();
    void NotifyDashDownKick();
}