public interface IPlayerEvents
{
    event System.Action OnPlayerDied;
    event System.Action OnPlayerTookDamage;
    event System.Action OnDash;
    event System.Action OnDashDownKick;
    event System.Action OnPlayerSitDown;
    event System.Action OnPlayerStandUp;
    event System.Action OnParry;

    void NotifyPlayerSitDown();
    void NotifyPlayerStandUp();
    void NotifyDashPerformed();
    void NotifyParryPerformed();
    void NotifyDashDownKickPerformed();
    void NotifyPlayerTookDamage();
    void NotifyPlayerDied();
}