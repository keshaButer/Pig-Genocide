using VContainer;
using UnityEngine;
using System.Collections;

public class RopeControl : MonoBehaviour
{
    [Range(1, 40)]
    [SerializeField] private float _maxRideSpeed, _minRideSpeed;
    [SerializeField] private float _offsetGetOff;

    private Transform graber1, graber2;
    private Transform playerTransform;
    private Coroutine _rideCoroutine;
    private IPlayerProvider _playerProvider;
    private float _rideSpeed;

    private void OnValidate()
    {
        if (_minRideSpeed > _maxRideSpeed)
        {
            _maxRideSpeed = _minRideSpeed;
        }
    }

    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        graber1 = transform.GetChild(0);
        graber2 = transform.GetChild(1);

        _playerProvider = playerProvider;
        _playerProvider.OnPlayerSpawned += OnPlayerSpawned;

        if (_playerProvider.Player != null)
            OnPlayerSpawned(_playerProvider.Player);
    }
    private void OnPlayerSpawned(GameObject playerObj)
    {
        graber1 = transform.GetChild(0);
        graber2 = transform.GetChild(1);

        playerTransform = playerObj.transform;

        _rideSpeed = Random.Range(_minRideSpeed, _maxRideSpeed);
    }
    private void OnDestroy()
    {
        _playerProvider.OnPlayerSpawned -= OnPlayerSpawned;
    }
    public void UseRope(Transform _graber)
    {
        if (playerTransform.GetComponent<ControlDisabler>().isUsing)
            return;

        playerTransform.GetComponent<ControlDisabler>().isUsing = true;
        if (_graber == graber1)
        {
            playerTransform.position = graber1.position;
            StartRide(graber2.position);
        }
        else if (_graber == graber2)
        {
            playerTransform.position = graber2.position;
            StartRide(graber1.position);
        }
    }
    private IEnumerator Ride(Vector3 _target)
    {
        while (Vector2.Distance(playerTransform.position, _target) > _offsetGetOff)
        {
            Vector3 _direction = _target - playerTransform.position;
            playerTransform.Translate(_direction.normalized * _rideSpeed * Time.deltaTime);

            yield return new WaitForEndOfFrame();
        }
        ControlDisabler controlDisabler = playerTransform.GetComponent<ControlDisabler>();
        controlDisabler.isUsing = false;
        controlDisabler.EnableControl();
    }
    private void StartRide(Vector3 _target)
    {
        if (_rideCoroutine != null)
        {
            StopCoroutine(_rideCoroutine);
            _rideCoroutine = null;
        }
        playerTransform.GetComponent<ControlDisabler>().DisableControl();
        _rideCoroutine = StartCoroutine(Ride(_target));
    }
}
