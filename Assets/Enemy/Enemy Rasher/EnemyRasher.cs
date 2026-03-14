using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[RequireComponent(typeof(PathFollower))]
public class EnemyRasher : Enemy
{
    public bool isChangeDirection { get; set; }

    [SerializeField] private float rayRangeJump;
    [SerializeField] private float jupmHeight, fightJupmHeight, chillJumpHeight;
    [SerializeField] private float checkCircleRadius;
    [SerializeField] private float intervalChangeDirection;
    [SerializeField] private int maxCountJumps = 3;
    [SerializeField] private int coolDownJumps = 2;
    [SerializeField] private LayerMask checkMask;

    protected int countJumps;
    protected bool isJump, isFastExpend;
    protected bool isGrounded;
    protected float coolDownTimer;
    protected float playerDistance;
    protected float currentDirection;

    protected Transform checkCirclePoint;
    protected Vector2 moveDirection;
    protected Transform playerPos;
    protected Coroutine fightCoroutine;
    private PathFollower _pathFollower;

    private void Start() => Initialize();

    public void Initialize()
    {
        _pathFollower = GetComponent<PathFollower>();

        checkCirclePoint = transform.GetChild(1);
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        fightCoroutine = StartCoroutine(nameof(Fighting));
    }

    private IEnumerator Fighting()
    {
        while (playerPos != null)
        {
            CheckGround();
            CalculateDirection(playerPos.position);

            if (!_pathFollower.HasPath || _pathFollower.IsPathComplete())
            {
                List<Vector2> newPath = PathFinder.FindPath(
                    transform.position, 
                    playerPos.position, 
                    ChunkedLevelGenerator.SingleTon
                );

                if (newPath != null)
                    _pathFollower.SetPath(newPath);
            }

            _pathFollower.MoveAlongPath();
            JumpControl();

            yield return new WaitForFixedUpdate();
        }

        fightCoroutine = null;
    }
    protected void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(checkCirclePoint.position,
        checkCircleRadius, checkMask);
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
        if (isGrounded && Physics2D.BoxCast(transform.position - new Vector3(0.5f, 0.6f), new Vector2(rayRangeJump, 0.5f),
         0, Vector2.left, 0, checkMask) && countJumps < maxCountJumps && currentDirection == -1)
        {
            countJumps++;
            Jump();
        }
        else if (isGrounded && Physics2D.BoxCast(transform.position + new Vector3(0.5f, -0.6f), new Vector2(rayRangeJump, 0.5f),
         0, Vector2.left, 0, checkMask) && countJumps < maxCountJumps && currentDirection == 1)
        {
            countJumps++;
            Jump();
        }
        if (countJumps >= maxCountJumps)
        {
            coolDownTimer += Time.deltaTime;
            if (coolDownTimer >= coolDownJumps)
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
        if (distance >= intervalChangeDirection)
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
