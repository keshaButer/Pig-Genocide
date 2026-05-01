using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private DifficultyConfig _difficultyConfig;
    [SerializeField] private CoroutineOwner _intervalInvokingPerformer;
    [SerializeField] private ChunkedLevelGenerator _chunkedLevelGenerator;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private HealthWindow _healthWindow;
    [SerializeField] private SoundManager _soundManager;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterServices(builder);

        RegisterComponents(builder);

        RegisterInstances(builder);
    }

    private void RegisterServices(IContainerBuilder builder)
    {
        builder.Register<IDifficultyManager, DifficultyManager>(Lifetime.Singleton);
        builder.Register<IInvokerFactory, IntervalInvoker>(Lifetime.Singleton);

        RegisterEventServices(builder);
    }

    private void RegisterComponents(IContainerBuilder builder)
    {
        builder.RegisterComponent<IPlayerProvider>(_playerSpawner);
        builder.RegisterComponent(_intervalInvokingPerformer);
        builder.RegisterComponent<ILevelGenerator>(_chunkedLevelGenerator);
        builder.RegisterComponent<ISoundManager>(_soundManager);
        builder.RegisterComponent<IHealthWindow>(_healthWindow);
    }

    private void RegisterInstances(IContainerBuilder builder)
    {
        builder.RegisterInstance(_difficultyConfig);
    }

    private void RegisterEventServices(IContainerBuilder builder)
    {
        builder.Register<IPlayerCombatEvents, PlayerCombatEvents>(Lifetime.Singleton);
        builder.Register<IPlayerMovementEvents, PlayerMovementEvents>(Lifetime.Singleton);
        builder.Register<IPlayerStateEvents, PlayerStateEvents>(Lifetime.Singleton);

        builder.Register<IEnemyEvents, EnemyEvents>(Lifetime.Singleton);
        builder.Register<IExplosionEvents, ExplosionEvents>(Lifetime.Singleton);
    }
};
