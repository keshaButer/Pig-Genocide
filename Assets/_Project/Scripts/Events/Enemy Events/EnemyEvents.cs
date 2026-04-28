public class EnemyEvents : IEnemyEvents
{
    public event System.Action OnEnemyDied;

    public void NotifyEnemyDied() => OnEnemyDied?.Invoke();
}