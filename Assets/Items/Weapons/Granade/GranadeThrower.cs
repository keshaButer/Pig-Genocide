using UnityEngine;

public class GranadeThrower : Weapon
{
    [SerializeField] GranadeItem granadeItem;
    [SerializeField] GameObject granadePrefab;
    [SerializeField] float throwForce, rotateForce;
    public override void WeaponAttack()
    {
        GameObject granade = Instantiate(granadePrefab, transform.position, transform.rotation);
        granade.GetComponent<Rigidbody2D>().AddForce(transform.right * throwForce, ForceMode2D.Impulse);
        granade.GetComponent<Rigidbody2D>().AddForceAtPosition(transform.right * rotateForce,
        transform.position + new Vector3(0, 0.3f),  ForceMode2D.Impulse);
    }

    public override void Initialize() => item = granadeItem as Item;
}
