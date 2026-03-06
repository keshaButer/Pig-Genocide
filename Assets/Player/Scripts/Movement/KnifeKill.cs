using UnityEngine;

public class KnifeKill : MonoBehaviour
{
    private Transform pointRay;
    [SerializeField] private float rayDistance;
    [SerializeField] private int knifeDamage = 100;
    private MovementPlayer movementPlayer;
    void Start()
    {
        movementPlayer = GetComponent<MovementPlayer>();
        pointRay = transform.GetChild(1).GetChild(3);
    }
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(pointRay.position,
         pointRay.right, rayDistance, LayerMask.GetMask("Enemy"));
        if (movementPlayer.IsCrouch && hit && Input.GetKeyDown(movementPlayer.inputConfig.knifeKill))
        {
            if (hit.transform.GetComponent<Enemy>().CurrrentState == Enemy.States.chill)
            {
                hit.transform.GetComponent<Enemy>().ApplyDamage(knifeDamage);

                //анимация использования ножа
            }
        }
    }
}
