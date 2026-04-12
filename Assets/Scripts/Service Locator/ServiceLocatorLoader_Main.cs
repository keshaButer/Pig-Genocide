using UnityEngine;

public class ServiceLocatorLoader_Main : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private BarrelSpawner _barrelSpawner;
    [SerializeField] private RopeSpawner _ropeSpawner;
    [SerializeField] private SlowTimeManager _slowTimeManager;
    [SerializeField] private HealthWindow _healthWindow;
    [SerializeField] private CameraSway _cameraSway;
    [SerializeField] private ChunkedLevelGenerator _levelGenerator;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private UIStaminaControll _uiStaminaControll;

    private void Awake()
    {
        RegisterServices();
    }
    private void Start()
    {
        Initialize();
    }

    private void RegisterServices()
    {
        ServiceLocator.Initialize();
    }

    private void Initialize()
    {
        _soundManager.Subscribe();
        _enemySpawner.Subscribe();
        _levelGenerator.Initialize();
    }
}
