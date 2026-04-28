using VContainer;
using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [SerializeField] float _startAccelCam, _accelText, 
     velocityToChangeCamAccel, _freeCamSpeed, _playerHorizontalSpeed, _rotateCameraSpeed;

    [SerializeField] Transform _cam, _healthText;
    [SerializeField] KeyCode _freeViewKey;
    [SerializeField] KeyCode _rotateCameraKey;

    private float _accelCam, _xMouse, _yMouse, _xHorizontal;
    private Rigidbody2D _rb2D => _playerTransform.GetComponent<Rigidbody2D>(); 
    private Vector2 _cameraTarget;
    private Transform _playerTransform;
    private bool _wasFreeView = false;

    [Inject]
    public void Construct(IPlayerProvider playerProvider)
    {
        playerProvider.OnPlayerSpawned += Initialize;
        
        if (playerProvider.Player != null)
            Initialize(playerProvider.Player);
    }

    private void Initialize(GameObject player)
    {
        _playerTransform = player.transform;
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            if (_rb2D.linearVelocityY <= velocityToChangeCamAccel) _accelCam = 0.15f;
            else _accelCam = _startAccelCam;
        }

        if (Input.GetKey(_rotateCameraKey))
            RotateCamera(_rotateCameraSpeed);
        else if (!(Mathf.Abs(_cam.eulerAngles.z) < 0.2f))
            RotateCamera(-_rotateCameraSpeed);

        if (_playerTransform != null)
            Sway();
    }
    void Sway()
    {
        if (_cam != null && _playerTransform != null)
        {
            if (Input.GetKey(_freeViewKey))
            {
                _playerTransform.GetComponent<MovementPlayer>().isStop = true;

                if (!_wasFreeView)
                { 
                    _cameraTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    _wasFreeView = true;
                }
                _cameraTarget = CalculateMousePosition();
            }
            else
            {
                _cameraTarget = _playerTransform.position + new Vector3(0, 2f);
                _wasFreeView = false;
                _playerTransform.GetComponent<MovementPlayer>().isStop = false;
            }

            _cam.position = Vector2.Lerp(_cam.position, _cameraTarget, _accelCam * 100 * Time.deltaTime);
        }

        if (_healthText != null && _playerTransform != null)
        {
            Vector2 textPos = _playerTransform.position + new Vector3(-7, 4.5f);
            _healthText.position = Vector2.Lerp(_healthText.position, textPos, _accelText * 100 * Time.deltaTime);
        }
    }

    Vector2 CalculateMousePosition()
    {
        _xMouse = Input.GetAxis("Mouse X") * Time.deltaTime * _freeCamSpeed;
        _yMouse = Input.GetAxis("Mouse Y") * Time.deltaTime * _freeCamSpeed;

        return _cameraTarget + new Vector2(_xMouse, _yMouse);
    }

    void RotateCamera(float rotValue)
    {
        _cam.eulerAngles = new Vector3(_cam.eulerAngles.x, _cam.eulerAngles.y, _cam.eulerAngles.z + rotValue);
    }
}
