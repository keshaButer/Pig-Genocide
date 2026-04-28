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

    [Inject] private IPlayerMovementEvents _playerMovementEvents;
    [Inject] private IPlayerStateEvents _playerStateEvents;
    [Inject] private IPlayerCombatEvents _playerCombatEvents;
    [Inject] private IEnemyEvents _enemyEvents;
    [Inject] private IExplosionEvents _explosionEvents;

    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        _enemyEvents.OnEnemyDied += ShakeCamera;

        _playerMovementEvents.OnDashDownKick += () => ShakeCamera(strengthShakeDash);
        _playerStateEvents.OnTookDamage += () => ShakeCamera(strengthPlayerTookDamage, durationPlayerTookDamage);
        _playerStateEvents.OnDied += ShakeCamera;
        _playerCombatEvents.OnParry += ShakeCamera;

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
