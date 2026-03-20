using UnityEngine;

public class ParryCheckBox : MonoBehaviour
{
    [SerializeField] private float _additionalSpeed = 1;

    private ParryInput _parryInput;

    private void Start()
    {
        _parryInput = transform.parent.GetComponent<ParryInput>();
    }
    public void HandleBullet(Bullet bullet)
    {
        if (_parryInput.CanParry)
        {
            if (!bullet.isParry)
                bullet.isParry = true;

            bullet.Speed += _additionalSpeed;

            EventManager.OnParry();
        }
    }
}
