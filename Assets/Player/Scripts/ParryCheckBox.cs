using VContainer;
using UnityEngine;

public class ParryCheckBox : MonoBehaviour
{
    [SerializeField] private float _additionalSpeed = 1;
    [Inject] private IPlayerEvents _playerEvents;

    private ParryInput _parryInput;

    private void Start()
    {
        _parryInput = transform.parent.GetComponent<ParryInput>();
    }
    public void HandleBullet(Bullet bullet)
    {
        if (_parryInput.CanParry)
        {
            if (!bullet.IsParry)
                bullet.IsParry = true;

            bullet.Speed += _additionalSpeed;

            _playerEvents.NotifyParryPerformed();
        }
    }
}
