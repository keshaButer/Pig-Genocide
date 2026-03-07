using UnityEngine;

public class ParryCheckBox : MonoBehaviour
{
    private ParryInput parryInput;
    private void Start()
    {
        parryInput = transform.parent.GetComponent<ParryInput>();
    }
    public void HandleBullet(Bullet bullet)
    {
        if (parryInput.CanParry)
        {
            if (!bullet.isParry)
                bullet.isParry = true;

            bullet.Speed += 1;

            EventManager.OnParry();
        }
    }
}
