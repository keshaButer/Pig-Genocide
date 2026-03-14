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
    [SerializeField] private PlayerMovementConfig config;

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

    private void Start() => Initialize();

    public void Initialize()
    {
        _pathFollower = GetComponent<PathFollower>();

        checkCirclePoint = transform.GetChild(1);
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _movementPlayer = playerTransform.GetComponent<MovementPlayer>();

        fightCoroutine = StartCoroutine(nameof(Fighting));
    }

    private void CalculateYVelocity()
    {
        if (!isGrounded)
        {
            _yVelocity -= _gravityForce * Time.fixedDeltaTime;
            _timerGravity = 0;
        }
        else
        {
            _timerGravity += Time.deltaTime;
            if (_timerGravity >= _minDelayStopFall)
            {
                _yVelocity = 0;
                // RigidBody.linearVelocity = new Vector2(RigidBody.linearVelocityX, -5.5f);
            }
        }
    }

    private IEnumerator Fighting()
    {
        while (playerTransform != null)
        {
            CheckGround();
            CalculateDirection(playerTransform.position);

            if ((!_pathFollower.HasPath && !_targetUnreachable))
            {
                List<Vector2> newPath = PathFinder.FindPath(
                    transform.position, 
                    _movementPlayer.checkCirclePoint.position, 
                    ChunkedLevelGenerator.SingleTon
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
                Debug.Log("unreachable = false");
                _targetUnreachable = false;
            }

            CalculateYVelocity();
            if (_pathFollower.HasPath)
                Move(_pathFollower.GetDirectionAlongPath());
            else
                Move(Vector2.zero);

            // JumpControl();

            yield return new WaitForFixedUpdate();
        }

        fightCoroutine = null;
    }

    private void Move(Vector2 moveDirection)
    {
        float horizontalMove = moveDirection.x * _pathFollower.Speed * Time.fixedDeltaTime;

        if (moveDirection.x != 0)
        {
            float direction = Mathf.Sign(moveDirection.x);
            float rayDistance = config.stepCheckDistance;

            Vector2 rayOrigin = checkCirclePoint.position + Vector3.up * 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayDistance, config.checkGroundMask);

            if (hit.collider != null)
            {
                Vector2 stepCheckStart = (Vector2)checkCirclePoint.position + new Vector2(0, config.stepHeight);
                RaycastHit2D stepHit = Physics2D.Raycast(stepCheckStart, Vector2.right * direction, rayDistance, config.checkGroundMask);

                if (stepHit.collider == null)
                {
                    Debug.Log("MOVE TP");
                    RigidBody.MovePosition(RigidBody.position + new Vector2(horizontalMove * Time.fixedDeltaTime, config.stepHeight));
                }
                else
                {
                    RigidBody.linearVelocity = new Vector2(0, moveDirection.y + _yVelocity);

                    return;
                }
            }
        }

        Debug.Log("MOVE");
        float verticalMove = moveDirection.y * _pathFollower.Speed * Time.fixedDeltaTime;
        RigidBody.linearVelocity = new Vector2(horizontalMove, verticalMove + _yVelocity);
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
