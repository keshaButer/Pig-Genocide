using VContainer;
using UnityEngine;

public class Barrel : Explosives, IDamagable
{
    [Range(0, 3)]
    [SerializeField] int _startHealth;

    [SerializeField] private GameObject effect;
    [SerializeField] private AudioClip _audioClip;
    [Inject] private ISoundManager _soundManager;

    public int CurrentHealth { get; private set; }

    private SpriteRenderer sprite;
    private GameObject lines;

    private void Start()
    {
        CurrentHealth = _startHealth;
        lines = transform.GetChild(0).gameObject;
    }
    public void ApplyDamage(int damage)
    {
        CurrentHealth -= damage;
        sprite = GetComponent<SpriteRenderer>();
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            OnExplode();
        }
    }
    protected override void OnExplode()
    {
        DealDamage();

        _soundManager.PlaySound(_audioClip, 0.1f);

        GameObject obj = Instantiate(effect, transform.position + new Vector3(0, 0.7f, 0), transform.rotation);
        obj.transform.parent = transform;

        sprite.enabled = false;
        lines.SetActive(false);
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject);
    }
}
