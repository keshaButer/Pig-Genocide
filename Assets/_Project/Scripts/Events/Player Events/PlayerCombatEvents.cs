public class PlayerCombatEvents : IPlayerCombatEvents
{
    public event System.Action OnParry;

    public void NotifyParry() => OnParry?.Invoke();
}