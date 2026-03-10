using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyRasher : Enemy, IFollowing
{
    [SerializeField] protected float chillSpeed, fightSpeed;
    private float _speed;
    [SerializeField] float rayRange;
    [SerializeField] float timeAfterExpend = 0.3f;
    [SerializeField] LayerMask checkMask;
    [SerializeField] float jupmHeight, fightJupmHeight, chillJumpHeight;
    [SerializeField] float checkCircleRadius;
    [SerializeField] int maxCountJumps = 3;
    [SerializeField] int coolDownJumps = 2;
    [SerializeField] float timeToDestroy = 3;
    [SerializeField] float offsetDirection, maxTimeNotSeePlayer;
    [SerializeField] public float distanceActivating;
    [Range(-1, 1)]
    public int direction;
    protected Transform checkCirclePoint, eysePoint;
    protected Vector2 moveDirection;
    protected float x, timeNotSeePlayer;
    protected bool isJump, isFastExpend;
    protected bool isGrounded;
    protected bool isSeePlayer;
    protected int countJumps;
    protected float coolDownTimer;
    protected float playerDistance, pointDistance;
    protected bool was;
    public bool isChangeDirection { get; set; }
    protected Transform playerPos;
    private PolygonCollider2D viewCollider;
    protected Coroutine fightCoroutine, chillCoroutine, getToPointCoroutine, startChillAlgorithm;
    [SerializeField] private Transform baseAlgorithmPoint;
    private List<Transform> points;
    private ContactFilter2D filter = new ContactFilter2D();
    private Collider2D[] collidersInView = new Collider2D[10];
    private RaycastHit2D hit;
    private Vector2 directionToPlayer;
    [SerializeField] LayerMask layerMask;
    private void Start() => Init();
    protected override void Init()
    {
        base.Init();

        eysePoint = transform.GetChild(3);
        Health = maxHealth;
        filter.SetLayerMask(LayerMask.GetMask("Player"));
        viewCollider = transform.GetChild(2).GetComponent<PolygonCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        checkCirclePoint = transform.GetChild(1);
        x = direction;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        points = baseAlgorithmPoint.GetComponent<BaseAlgotrithmPoint>().points;

        SetState(States.chill);
    }
    private IEnumerator StartChillAlgorithm()
    {
        yield return getToPointCoroutine = StartCoroutine(GetToPoint(baseAlgorithmPoint.position, 0, chillSpeed));
        chillCoroutine = StartCoroutine(Chilling());
        startChillAlgorithm = null;
    }
    protected virtual void Look()
    {
        if (playerPos != null)
            directionToPlayer = playerPos.position - eysePoint.position;
        hit = Physics2D.Raycast(eysePoint.position, directionToPlayer.normalized, 60, layerMask);
        if (viewCollider.Overlap(filter, collidersInView) > 0 && hit)
        {
            if (hit.transform.tag == "Player")
            {
                isSeePlayer = true;
                timeNotSeePlayer = 0;
                was = false;

                if (CurrrentState != States.fight)
                    SetState(States.fight);
            }
            else { isSeePlayer = false; timeNotSeePlayer += Time.deltaTime; }
        }
        else { isSeePlayer = false; timeNotSeePlayer += Time.deltaTime; }

        if (timeNotSeePlayer >= maxTimeNotSeePlayer && !was)
        {
            was = true;
            timeNotSeePlayer = 0;
            if (CurrrentState != States.chill)
                SetState(States.chill);
        }
    }
    private void FixedUpdate()
    {
        Look();

        if (CurrrentState == States.chill && getToPointCoroutine == null && chillCoroutine == null)
        {
            startChillAlgorithm = StartCoroutine(StartChillAlgorithm());
        }
    }
    protected IEnumerator GetToPoint(Vector3 pointPos, float time, float speed = 1.5f)
    {
        _speed = speed;
        jupmHeight = chillJumpHeight;
        CalculateDirection(pointPos, 0);
        pointDistance = Vector2.Distance(transform.position, pointPos);
        while (pointDistance > offsetDirection && !isDeath)
        {
            pointDistance = Vector2.Distance(transform.position, pointPos);
            Move(pointPos, 0);
            JumpControl();
            CheckGround();

            if (isSeePlayer)
                SetState(States.fight);

            yield return null;
        }
        if (!isDeath)
            rb.linearVelocity = new Vector2(0, rb.linearVelocityY);

        float timer = 0;
        while (timer < time && !isDeath)
        {
            timer += Time.deltaTime;
            rb.linearVelocity = Vector2.zero;

            yield return null;
        }
        getToPointCoroutine = null;
    }
    private IEnumerator Chilling()
    {
        foreach (Transform point in points)
            yield return getToPointCoroutine = StartCoroutine(GetToPoint(point.position, 3, chillSpeed));
        StartCoroutine(Chilling());
    }
    private IEnumerator Fighting()
    {
        _speed = fightSpeed;
        jupmHeight = fightJupmHeight;
        while (playerPos != null)
        {
            playerDistance = Vector2.Distance(transform.position, playerPos.position);
            if (!isDeath && playerDistance <= distanceActivating)
            {
                Move(playerPos.position);
                JumpControl();
            }
            CheckGround();

            yield return null;
        }
        fightCoroutine = null;
    }
    protected override void SetState(States _state)
    {
        base.SetState(_state);
        switch (CurrrentState)
        {
            case States.chill:
                if (getToPointCoroutine != null)
                {
                    StopCoroutine(getToPointCoroutine);
                    getToPointCoroutine = null;
                }
                if (fightCoroutine != null)
                {
                    StopCoroutine(fightCoroutine);
                    fightCoroutine = null;
                }
                startChillAlgorithm = StartCoroutine(StartChillAlgorithm());
                break;
            case States.fight:
                if (getToPointCoroutine != null)
                {
                    StopCoroutine(getToPointCoroutine);
                    getToPointCoroutine = null;
                }
                if (chillCoroutine != null)
                {
                    StopCoroutine(chillCoroutine);
                    chillCoroutine = null;
                }
                fightCoroutine = StartCoroutine(Fighting());
                break;
        }
    }
    protected void Move(Vector3 position)
    {
        if (!isFastExpend)
            CalculateDirection(position);

        moveDirection.x = x * _speed * Time.fixedDeltaTime * 100;
        moveDirection.y = rb.linearVelocityY;
        rb.linearVelocity = moveDirection;
    }
    protected void Move(Vector3 position, int num)
    {
        if (!isFastExpend)
            CalculateDirection(position, num);

        moveDirection.x = x * _speed * Time.fixedDeltaTime * 100;
        moveDirection.y = rb.linearVelocityY;
        rb.linearVelocity = moveDirection;
    }
    protected void CheckGround()
    {
        isGrounded = Physics2D.CircleCast(checkCirclePoint.position,
        checkCircleRadius, -transform.up, 0, checkMask);
    }
    protected void Jump()
    {
        moveDirection.y = CalculateAccelJump(jupmHeight);
        rb.linearVelocity = moveDirection;
    }
    protected float CalculateAccelJump(float _jumpHeight)
    {
        return Mathf.Sqrt(2 * rb.gravityScale * _jumpHeight);
    }
    protected void JumpControl()
    {
        if (isGrounded && Physics2D.BoxCast(transform.position - new Vector3(0.5f, 0.6f), new Vector2(rayRange, 0.5f),
         0, Vector2.left, 0, checkMask) && countJumps < maxCountJumps && x == -1)
        {
            countJumps++;
            Jump();
        }
        else if (isGrounded && Physics2D.BoxCast(transform.position + new Vector3(0.5f, -0.6f), new Vector2(rayRange, 0.5f),
         0, Vector2.left, 0, checkMask) && countJumps < maxCountJumps && x == 1)
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
    public int CalculateDirection(Vector3 position)
    {
        float x1 = position.x;
        float x2 = transform.position.x;
        float distance = Mathf.Abs(x1 - x2);
        if (distance >= offsetDirection)
        {
            if (x1 < x2)
            {
                x = -1;
                Expend(false);
                return -1;
            }
            else if (x1 > x2)
            {
                x = 1;
                Expend(true);
                return 1;
            }
            else { x = 0; return 0; }
        }
        else { x = 0; return 0; }
    }
    public int CalculateDirection(Vector3 position, int num)
    {
        float x1 = position.x;
        float x2 = transform.position.x;
        if (x1 < x2)
        {
            x = -1;
            Expend(false);
            return -1;
        }
        else if (x1 > x2)
        {
            x = 1;
            Expend(true);
            return 1;
        }
        else { x = 0; return 0; }
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
    private void FastExpend()
    {
        isFastExpend = true;

        if (transform.rotation == Quaternion.Euler(0, 0, 0))
            Expend(true);
        else Expend(false);

        Invoke(nameof(ResetIsFastExpend), timeAfterExpend);
    }
    private void OnCollisionEnter2D(Collision2D other) => Attack(other);
    protected override void Attack(Collision2D other)
    {
        if (other.gameObject.GetComponent<HealthPlayer>())
        {
            print("expend");
            FastExpend();
        }
            
    }
    private void ResetIsFastExpend() => isFastExpend = false;
}
