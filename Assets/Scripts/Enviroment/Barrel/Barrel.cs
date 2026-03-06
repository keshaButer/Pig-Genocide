using UnityEngine;

public class Barrel : Explosives, IDamagable
{
    [Range(0, 3)]
    [SerializeField] int _startHealth;

    [SerializeField] GameObject effect;
    [SerializeField] AudioClip _audioClip;

    public int Health { get; private set; }

    private SpriteRenderer sprite;
    private GameObject lines;
    private SoundSource _soundSource;

    void Start()
    {
        Health = _startHealth;
        lines = transform.GetChild(0).gameObject;
        _soundSource = GetComponent<SoundSource>();
    }
    public void ApplyDamage(int damage)
    {
        Health -= damage;
        sprite = GetComponent<SpriteRenderer>();
        if (Health <= 0)
        {
            Health = 0;
            Explode();
        }
    }
    protected override void Explode()
    {
        DealDamage();

        SoundManager.SingleTone.PlaySound(_audioClip, 0.1f);

        GameObject obj = Instantiate(effect, transform.position + new Vector3(0, 0.7f, 0), transform.rotation);
        obj.transform.parent = transform;

        sprite.enabled = false;
        lines.SetActive(false);
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject);
    }
}
