using UnityEngine;

public class Kalash : Weapon
{
    [SerializeField] Rifle rifle;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform bulletSpawn;
    [SerializeField] LayerMask freeSpaceMask;
    private SoundSource _soundSource;
    private bool isFreeSpace;
    private void Start() => _soundSource = GetComponent<SoundSource>();
    private void Update()
    {
        isFreeSpace = Physics2D.CircleCast(bulletSpawn.position, 0.2f, transform.up, 0, freeSpaceMask);
    }
    public override void WeaponAttack()
    {
        if (!isFreeSpace)
        {
            Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

            _soundSource.PlaySound(rifle.audioClipShot, rifle.radiusSoundShot);
        }
    }

    public override void Initialize()
    {
        item = rifle;
        bulletSpawn = transform.GetChild(0);
    }
}
