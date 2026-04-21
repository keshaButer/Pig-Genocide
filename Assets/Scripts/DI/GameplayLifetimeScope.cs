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
        builder.RegisterComponent<IPlayerProvider>(_playerSpawner);
        builder.RegisterComponent(_intervalInvokingPerformer);
        
        builder.RegisterComponent<ILevelGenerator>(_chunkedLevelGenerator);

        builder.RegisterComponent<ISoundManager>(_soundManager);
        
        builder.RegisterComponent<IHealthWindow>(_healthWindow);

        builder.RegisterInstance(_difficultyConfig);
        builder.Register<IDifficultyManager, DifficultyManager>(Lifetime.Singleton);
        builder.Register<IInvokerFactory, IntervalInvoker>(Lifetime.Singleton);
    }
};