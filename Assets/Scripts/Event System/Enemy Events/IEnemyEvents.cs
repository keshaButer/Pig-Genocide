public interface IEnemyEvents
{
    event System.Action OnEnemyDied;
    void NotifyEnemyDied();
}