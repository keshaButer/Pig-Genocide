using UnityEngine;
using System.Collections;

public class RopeControl : MonoBehaviour
{
    [SerializeField] private float _rideSpeed;
    [SerializeField] private float _offset;

    private Transform graber1, graber2;
    private Transform playerTransform;
    private Coroutine _rideCoroutine;

    private void Awake()
    {
        graber1 = transform.GetChild(0);
        graber2 = transform.GetChild(1);

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
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
        while (Vector2.Distance(playerTransform.position, _target) > _offset)
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
