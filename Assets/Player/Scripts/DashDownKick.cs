using UnityEngine;

public class DashDownKick : MonoBehaviour
{
    private MovementPlayer movementPlayerScript;
    [SerializeField] float force;
    [SerializeField] int damage;
    [SerializeField] float timeSlowMo;
    [SerializeField] float timeSpeedInSlowMo;

    private void Awake()
    {
        movementPlayerScript = GetComponent<MovementPlayer>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (movementPlayerScript.isDashDown)
        {
            movementPlayerScript.ResetIsDashDown();

            if (other.transform.GetComponent<IDamagable>() != null)
            {
                other.transform.GetComponent<IDamagable>().ApplyDamage(damage);
                EventManager.OnDashDownKick();
                SlowTimeManager.SingleTon.SlowTime(timeSpeedInSlowMo, timeSlowMo);
            }

            if (other.transform.GetComponent<Rigidbody2D>())
                other.transform.GetComponent<Rigidbody2D>().AddForceAtPosition(Vector2.down * force,
                 other.transform.position, ForceMode2D.Impulse);
        }
    }
}
