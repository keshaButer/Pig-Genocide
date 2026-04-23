using VContainer;
using UnityEngine;
using DG.Tweening;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] float
    duration,
    strength,
    strengthShakeDash,
    strengthPlayerTookDamage = 1f,
    durationPlayerTookDamage = 0.5f;

    [Inject] private IPlayerEvents _playerEvents;
    [Inject] private IEnemyEvents _enemyEvents;
    [Inject] private IExplosionEvents _explosionEvents;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        _enemyEvents.OnEnemyDied += ShakeCamera;

        _playerEvents.OnDashDownKick += () => ShakeCamera(strengthShakeDash);
        _playerEvents.OnPlayerTookDamage += () => ShakeCamera(strengthPlayerTookDamage, durationPlayerTookDamage);
        _playerEvents.OnPlayerDied += ShakeCamera;
        _playerEvents.OnParry += ShakeCamera;

        _explosionEvents.OnExplosion += ShakeCamera;
    }
    public void ShakeCamera()
    {
        Camera.main.DOShakePosition(duration, strength, 10, 90, true, ShakeRandomnessMode.Full);
    }
    public void ShakeCamera(float _strength)
    {
        Camera.main.DOShakePosition(duration, _strength, 10, 90, true, ShakeRandomnessMode.Full);
    }
    public void ShakeCamera(float _strength, float _duration)
    {
        Camera.main.DOShakePosition(_duration, _strength, 10, 90, true, ShakeRandomnessMode.Full);
    }
}
