using UnityEngine;
using VContainer;
using VContainer.Unity;

[RequireComponent(typeof(SoundSource))]
public class Kalash : Weapon
{
    [SerializeField] private Rifle _rifle;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnTransform;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private float _radiusCircleCheckFreeSpace;
    
    [Inject] private IObjectResolver _objectResolver;

    private SoundSource _soundSource;
    private bool _isFreeSpace;

    private void Start() => _soundSource = GetComponent<SoundSource>();

    private void Update()
    {
        _isFreeSpace = !Physics2D.OverlapCircle(_bulletSpawnTransform.position, _radiusCircleCheckFreeSpace, _obstacleMask);
    }
    public override void UseAttack()
    {
        if (_isFreeSpace)
        {
            _objectResolver.Instantiate(_bulletPrefab, _bulletSpawnTransform.position, _bulletSpawnTransform.rotation);

            _soundSource.PlaySound(_rifle.ShotSound, 10);
        }
    }
}
