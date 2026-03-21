using UnityEngine;

public class GranadeThrower : Weapon
{
    [SerializeField] private GranadeItem granadeItem;
    [SerializeField] private GameObject granadePrefab;
    [SerializeField] private float throwForce, rotateForce;

    public override void UseAttack()
    {
        GameObject granade = Instantiate(granadePrefab, transform.position, transform.rotation);
        granade.GetComponent<Rigidbody2D>().AddForce(transform.right * throwForce, ForceMode2D.Impulse);
        granade.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * rotateForce,
        transform.position + new Vector3(0, 0.3f),  ForceMode2D.Impulse);
    }
}
