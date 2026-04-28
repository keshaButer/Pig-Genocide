public interface IPlayerCombatEvents
{
    event System.Action OnParry;

    void NotifyParry();
}