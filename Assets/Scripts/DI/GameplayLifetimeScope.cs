using UnityEngine;
using VContainer;
using VContainer.Unity;

public class GameplayLifetimeScope : LifetimeScope
{
    [SerializeField] private PlayerSpawner _playerSpawner;
    [SerializeField] private DifficultyConfig _difficultyConfig;
    [SerializeField] private CoroutineOwner _intervalInvokingPerformer;

    protected override void Configure(IContainerBuilder builder)
    {
        builder.RegisterComponent<IPlayerProvider>(_playerSpawner);
        builder.RegisterComponent(_intervalInvokingPerformer);

        builder.RegisterInstance(_difficultyConfig);
        builder.Register<IDifficultyManager, DifficultyManager>(Lifetime.Singleton);
        builder.Register<IInvokerFactory, IntervalInvoker>(Lifetime.Singleton);
    }
};