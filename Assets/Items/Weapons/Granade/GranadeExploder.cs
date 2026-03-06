using UnityEngine;

public class GranadeExploder : Explosives
{
    [SerializeField] float delayExplode = 3;
    [SerializeField] float delayDestroyVfx = 1;

    [SerializeField] GameObject vfxPrefab;

    private void Start()
    {
        Physics2D.IgnoreCollision(GameObject.FindGameObjectWithTag("Player").GetComponent<Collider2D>(), GetComponent<CircleCollider2D>());

        Invoke(nameof(Explode), delayExplode);
    }
    protected override void Explode()
    {
        DealDamage();

        Instantiate(vfxPrefab, transform.position + new Vector3(0, 1f, 0),
         Quaternion.Euler(0, 0, 0));

        Destroy(gameObject);
    }
}
