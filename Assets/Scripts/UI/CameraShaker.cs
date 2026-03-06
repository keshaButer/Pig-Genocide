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
    private void Start()
    {
        Initialize();
    }
    public void Initialize()
    {
        EventManager.EnemyDied += ShakeCamera;
        EventManager.DashDownKick += () => ShakeCamera(strengthShakeDash);
        EventManager.PlayerTookDamage += () => ShakeCamera(strengthPlayerTookDamage, durationPlayerTookDamage);
        EventManager.PlayerDied += ShakeCamera;
        EventManager.Parry += ShakeCamera;
        EventManager.Explosion += ShakeCamera;
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
