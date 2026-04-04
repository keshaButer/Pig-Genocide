using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PathFollower))]
public class EnemyRasher : Enemy
{
    public bool isChangeDirection { get; set; }

    [SerializeField] private float _rayRangeJump;
    [SerializeField] private float _gravityForce, _minDelayStopFall;
    [SerializeField] private float _coolDownFindPath;
    [SerializeField] private float jupmHeight, fightJupmHeight, chillJumpHeight;
    [SerializeField] private float _checkCircleRadius;
    [SerializeField] private float _intervalChangeDirection;
    [SerializeField] private int _maxCountJumps = 3;
    [SerializeField] private int _coolDownJumps = 2;
    [SerializeField] private LayerMask _checkMask;
    [SerializeField] private EnemyMovementConfig config;

    protected int countJumps;
    protected bool isJump, isFastExpend;
    protected bool isGrounded;
    protected float coolDownTimer;
    protected float playerDistance;
    protected float currentDirection;

    protected Transform checkCirclePoint;
    protected Vector2 moveDirection;
    protected Transform playerTransform;
    protected Coroutine fightCoroutine;

    private MovementPlayer _movementPlayer;
    private PathFollower _pathFollower;

    private bool _targetUnreachable;
    private float _unreachableTimer;
    private float _timerGravity;
    private float _yVelocity = 0;

    private void OnEnable()
    {
        PlayerSpawner.OnPlayerSpawned += Initialize;
    }
    private void OnDisable()
    {
        PlayerSpawner.OnPlayerSpawned -= Initialize;
    }
    public void Initialize(GameObject player)
    {
        _pathFollower = GetComponent<PathFollower>();

        checkCirclePoint = transform.GetChild(1);
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
        while (playerTransform != null)
        {
            CheckGround();
            CalculateDirection(playerTransform.position);

            if (!_pathFollower.HasPath && !_targetUnreachable)
            {
                List<Vector2> newPath = PathFinder.FindPath(
                    transform.position, 
                    _movementPlayer.checkCirclePoint.position, 
                    ChunkedLevelGenerator.SingleTon,
                    _pathFollower.MaxDepthAstar
                );

                if (newPath != null)
                {
                    _pathFollower.SetPath(newPath);
                }
                else
                {
                    _targetUnreachable = true;
                    _unreachableTimer = Time.time + _coolDownFindPath;
                }
            }

            if (_targetUnreachable && Time.time > _unreachableTimer)
            {
                _targetUnreachable = false;
            }

            if (_pathFollower.HasPath)
                _pathFollower.MoveAlongPath();
            // CalculateYVelocity();

            // JumpControl();

            yield return new WaitForFixedUpdate();
        }

        fightCoroutine = null;
    }
    protected void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(checkCirclePoint.position,
        _checkCircleRadius, _checkMask);
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
         0, Vector2.left, 0, _checkMask) && countJumps < _maxCountJumps && currentDirection == -1)
        {
            countJumps++;
            Jump();
        }
        else if (isGrounded && Physics2D.BoxCast(transform.position + new Vector3(0.5f, -0.6f), new Vector2(_rayRangeJump, 0.5f),
         0, Vector2.left, 0, _checkMask) && countJumps < _maxCountJumps && currentDirection == 1)
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
}
