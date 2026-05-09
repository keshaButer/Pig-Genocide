using VContainer;
using UnityEngine;

public class ParryCheckBox : MonoBehaviour
{
    [SerializeField] private float _additionalSpeed = 1;
    [SerializeField] private WeaponHandler _weaponHandler;
    [Inject] private IPlayerCombatEvents _playerCombatEvents;

    private ParryInput _parryInput;

    private void Awake()
    {
        _parryInput = transform.parent.GetComponent<ParryInput>();
    }
    public void HandleBullet(Bullet bullet)
    {
        if (_parryInput.CanParry)
        {
            bullet.IsParry = true;
            bullet.Speed += _additionalSpeed;

            Vector2 direction = _weaponHandler.MouseDirection;
            if (direction.sqrMagnitude < 0.01f)
                direction = Vector2.left;

            bullet.ParryDirection = direction.normalized;

            _playerCombatEvents.NotifyParry();
        }
    }
}
