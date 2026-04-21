using UnityEngine;
using VContainer;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PathFollower))]
public class EnemyRasher : Enemy
{
    public bool isChangeDirection { get; set; }

    [SerializeField] private float _minSpeed = 1, _maxSpeed = 20;
    [SerializeField] private float _rayRangeJump;
    [SerializeField] private float _gravityForce, _minDelayStopFall;
    [SerializeField] private float _coolDownUnreachablePath;
    [SerializeField] private float _coolDownRecalculatePath;
    [SerializeField] private float jupmHeight;
    [SerializeField] private float _checkCircleRadius;
    [SerializeField] private float _intervalChangeDirection;
    [SerializeField] private int _maxCountJumps = 3;
    [SerializeField] private int _coolDownJumps = 2;
    [SerializeField] private LayerMask _checkGroundMask;

    protected int countJumps;
    protected bool isGrounded;
    protected float coolDownTimer;
    protected float currentDirection;

    protected Transform checkCirclePoint;
    protected Vector2 moveDirection;
    protected Transform playerTransform;
    protected Coroutine fightCoroutine;

    protected PathFollower PathFollower;

    [Inject] private IPathFinder _pathFinder;
    [Inject] private ILevelGenerator _levelGenerator;
    [Inject] private IPlayerProvider _playerProvider;

    private MovementPlayer _movementPlayer;
    private bool _targetUnreachable;
    private float _unreachableTimer;
    private float _recalculatePathTimer;
    private float _timerGravity;
    private float _yVelocity = 0;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (_coolDownRecalculatePath > _coolDownUnreachablePath)
        {
            _coolDownRecalculatePath = _coolDownUnreachablePath;
        }
    }
#endif

    private void Start()
    {
        _playerProvider.OnPlayerSpawned += OnPlayerSpawned;
        
        if (_playerProvider.Player != null)
            OnPlayerSpawned(_playerProvider.Player);
    }
    protected override void Awake()
    {
        PathFollower = GetComponent<PathFollower>();
        checkCirclePoint = transform.GetChild(1);
    }

    private void OnPlayerSpawned(GameObject player)
    {
        playerTransform = player.transform;
        _movementPlayer = playerTransform.GetComponent<MovementPlayer>();

        fightCoroutine = StartCoroutine(nameof(Fighting));
    }

    private void CalculateYVelocity()
    {
        if (!isGrounded)
        {
            _yVelocity -= _gravityForce * Time.fixedDeltaTime;
            RigidBody.linearVelocity = new Vector2(0, _yVelocity);
            _timerGravity = 0;
        }
        else
        {
            _timerGravity += Time.deltaTime;
            if (_timerGravity >= _minDelayStopFall)
            {
                _yVelocity = 0;
                RigidBody.linearVelocity = new Vector2(0, _yVelocity);
            }
        }
    }

    private IEnumerator Fighting()
    {
        var wait = new WaitForFixedUpdate();

        while (playerTransform != null)
        {
            CheckGround();
            CalculateDirection(playerTransform.position);

            bool recalculatePath = Time.timeSinceLevelLoad >= _recalculatePathTimer;
            if ((!PathFollower.HasPath && !_targetUnreachable) || (recalculatePath && PathFollower.HasPath))
            {
                List<Vector2> newPath = _pathFinder.FindPath(
                    transform.position, 
                    _movementPlayer.checkCirclePoint.position, 
                    _levelGenerator,
                    PathFollower.MaxDepthAstar
                );

                if (newPath != null)
                {
                    PathFollower.SetPath(newPath);
                }
                else
                {
                    _targetUnreachable = true;
                    _unreachableTimer = Time.timeSinceLevelLoad + _coolDownRecalculatePath;
                }

                _recalculatePathTimer = Time.timeSinceLevelLoad + _coolDownRecalculatePath;
            }

            if (_targetUnreachable && Time.timeSinceLevelLoad >= _unreachableTimer)
            {
                _targetUnreachable = false;
            }

            if (PathFollower.HasPath)
            {
                Debug.Log("HasPath = true");
                PathFollower.MoveAlongPath();
            }
            // CalculateYVelocity();

            // JumpControl();

            yield return wait;
        }

        fightCoroutine = null;
    }

    protected void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(checkCirclePoint.position,
        _checkCircleRadius, _checkGroundMask);
    }

    protected void Jump()
    {
        moveDirection.y = CalculateJumpHeight(jupmHeight);
        RigidBody.linearVelocity = moveDirection;
    }

    protected float CalculateJumpHeight(float _jumpHeight)
    {
        return Mathf.Sqrt(2 * RigidBody.gravityScale * _jumpHeight);
    }

    protected void JumpControl()
    {
        if (isGrounded && Physics2D.BoxCast(transform.position - new Vector3(0.5f, 0.6f), new Vector2(_rayRangeJump, 0.5f),
         0, Vector2.left, 0, _checkGroundMask) && countJumps < _maxCountJumps && currentDirection == -1)
        {
            countJumps++;
            Jump();
        }
        else if (isGrounded && Physics2D.BoxCast(transform.position + new Vector3(0.5f, -0.6f), new Vector2(_rayRangeJump, 0.5f),
         0, Vector2.left, 0, _checkGroundMask) && countJumps < _maxCountJumps && currentDirection == 1)
        {
            countJumps++;
            Jump();
        }
        if (countJumps >= _maxCountJumps)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= _coolDownJumps)
            {
                coolDownTimer = 0;
                countJumps = 0;
            }
        }
    }

    public void CalculateDirection(Vector3 position)
    {
        float x1 = position.x;
        float x2 = transform.position.x;
        float distance = Mathf.Abs(x1 - x2);
        if (distance >= _intervalChangeDirection)
        {
            if (x1 < x2)
            {
                currentDirection = -1;
                Expend(false);
            }
            else if (x1 > x2)
            {
                currentDirection = 1;
                Expend(true);
            }
            else { currentDirection = 0; }
        }
        else { currentDirection = 0; }
    }

    public void Expend(bool turn)
    {
        if (turn && !isChangeDirection)
        {
            isChangeDirection = true;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (!turn && isChangeDirection)
        {
            isChangeDirection = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public override void OnDifficultyChanged(float playerSkill)
    {
        PathFollower.CurrentSpeed = Mathf.Clamp(PathFollower.StartSpeed * playerSkill, _minSpeed, _maxSpeed);
        Debug.Log($"Changed speed to: {PathFollower.CurrentSpeed}");
    }
}