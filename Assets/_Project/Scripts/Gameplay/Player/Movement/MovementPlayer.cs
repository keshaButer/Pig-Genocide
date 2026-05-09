using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class MovementPlayer : MonoBehaviour
{
    public InputPlayerMovementConfig inputConfig;
    public PlayerMovementConfig config;

    [Inject] private IPlayerMovementEvents _playerMovementEvents;

    private Rigidbody2D rb;
    [SerializeField] private WeaponHandler _weaponHandler;
    public Transform checkCirclePoint, circleStandUp;
    private BoxCollider2D colliderBody;
    private SoundSource _soundSource;

    private float _timerStepSound;
    private float jumpTimer;
    private float _timerGravity;

    private float horizontalInputDirection;
    private bool isJump;
    public bool isInput, isStop;
    private bool isGrounded, isCeiling;
    private bool wasIsGroundFalse;
    private bool isJumping, needToStandUp;
    public bool IsCrouch { get; private set; }
    private bool isExploded;

    private Transform startRayDown;
    private Transform startRayLeft;
    private Transform startRayRight;

    private StaminaControll staminaControll;

    public bool isDashDown { get; private set; }
    private Transform pointsTransform, spriteBody, parryPoints;
    private Coroutine IsGroundedCoroutine;

    private void Start() => Initialize();

    public void Initialize()
    {
        _weaponHandler.OnExpand += Expand;
        isInput = true;
        parryPoints = transform.GetChild(3);
        colliderBody = GetComponent<BoxCollider2D>();
        spriteBody = transform.GetChild(0);
        staminaControll = GetComponent<StaminaControll>();
        rb = GetComponent<Rigidbody2D>();
        pointsTransform = transform.GetChild(1);
        checkCirclePoint = pointsTransform.GetChild(0);
        startRayDown = checkCirclePoint;
        startRayLeft = pointsTransform.GetChild(1);
        startRayRight = pointsTransform.GetChild(2);
        circleStandUp = pointsTransform.GetChild(4);
        _soundSource = transform.GetComponent<SoundSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);


        CheckCircles();
        DashInput();
        if (isInput) GetInput();
        if (!IsCrouch) SoundStep();
        if (Input.GetKeyDown(KeyCode.H))
            PostEffectsController.SingleTon.FlashBang(0.2f);
    }

    private void FixedUpdate()
    {
        if (!isExploded)
        {
            ApplyGravity();
            Jump();
            Move();
        }
    }

    private void SoundStep()
    {
        if (Input.GetButton("Horizontal"))
        {
            _timerStepSound += Time.deltaTime;
            if (_timerStepSound >= config.delaySoundStep)
            {
                _soundSource.PlaySound(config.soundStep, config.radiusStep);
                _timerStepSound = 0;
            }
        }
        else _timerStepSound = 0;
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(inputConfig.crouch) && isGrounded)
        {
            if (!IsCrouch) SitDown();
            else needToStandUp = true;
        }
        if (needToStandUp && !isCeiling) StandUp();

        if (isStop) horizontalInputDirection = 0;
        else horizontalInputDirection = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && isGrounded)
            isJump = true;
    }

    private void ApplyGravity()
    {
        if (!isGrounded && rb.linearVelocity.y >= config.minFallSpeed)
        {
            rb.linearVelocity += Vector2.down * config.gravityForce * Time.fixedDeltaTime;
            _timerGravity = 0;
        }
        else
        {
            _timerGravity += Time.deltaTime;
            if (_timerGravity >= config.minDelayStopFall)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, -5.5f);
        }
    }

    private void Move()
    {
        float targetSpeed = IsCrouch ? config.crouchSpeed : config.speed;
        float horizontalMove = horizontalInputDirection * targetSpeed;

        if (horizontalInputDirection != 0)
        {
            float direction = Mathf.Sign(horizontalInputDirection);
            float rayDistance = config.stepCheckDistance;

            Vector2 rayOrigin = checkCirclePoint.position + Vector3.up * 0.1f;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * direction, rayDistance, config.checkGroundMask);

            if (hit.collider != null)
            {
                Vector2 stepCheckStart = (Vector2)checkCirclePoint.position + new Vector2(0, config.stepHeight);
                RaycastHit2D stepHit = Physics2D.Raycast(stepCheckStart, Vector2.right * direction, rayDistance, config.checkGroundMask);

                if (stepHit.collider == null)
                {
                    rb.MovePosition(rb.position + new Vector2(horizontalMove * Time.fixedDeltaTime, config.stepHeight));
                }
                else
                {
                    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

                    return;
                }
            }
        }

        rb.linearVelocity = new Vector2(horizontalMove, rb.linearVelocity.y);
    }

    private void Jump()
    {
        if (isJump)
        {
            isJump = false;
            jumpTimer = 0;
            _timerGravity = 0;
            isJumping = true;

            float jumpHeight = IsCrouch ? config.crouchJupmHeight : config.jupmHeight;
            float jumpVelocity = Mathf.Sqrt(2 * config.gravityForce * jumpHeight);
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpVelocity);
        }

        if (isJumping)
        {
            jumpTimer += Time.fixedDeltaTime;

            if (jumpTimer >= config.minJumpTime && isGrounded)
                isJumping = false;

            if (!Input.GetButton("Jump") && rb.linearVelocity.y > -4.5f)
            {
                rb.linearVelocity -= new Vector2(0, config.fallSpeed);
            }
        }
    }

    private void CheckCircles()
    {
        if (Physics2D.CircleCast(checkCirclePoint.position, config.checkCircleRadius, Vector2.down, config.checkCircleRadius, config.checkGroundMask))
        {
            isGrounded = true;
        }
        else if (!wasIsGroundFalse)
        {
            if (IsGroundedCoroutine != null)
                StopCoroutine(IsGroundedCoroutine);

            IsGroundedCoroutine = StartCoroutine(FalseIsGrounded());
            wasIsGroundFalse = true;
        }

        isCeiling = Physics2D.Raycast(circleStandUp.position, circleStandUp.up, 0.5f, config.checkGroundMask);
    }

    private IEnumerator FalseIsGrounded()
    {
        float timer = 0;
        while (true)
        {
            timer += Time.deltaTime;
            if (timer >= config.delayFalseIsGrounded)
            {
                isGrounded = false;
                wasIsGroundFalse = false;
                break;
            }
            yield return null;
        }
        wasIsGroundFalse = false;
    }

    private void Expand(bool side)
    {
        Quaternion rot = side ? Quaternion.Euler(0, 180, 0) : Quaternion.Euler(0, 0, 0);

        pointsTransform.rotation = rot;
        spriteBody.rotation = rot;
        parryPoints.rotation = rot;
    }

    private IEnumerator ResetRigidBodyCoroutine()
    {
        isInput = false;

        yield return new WaitForSeconds(config.delayResetRB);

        isInput = true;
        isExploded = false;
        rb.mass = 2;
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.eulerAngles = Vector3.zero;
        colliderBody.sharedMaterial = GetComponent<HealthPlayer>().alivePhysicsMaterial;
    }

    public void ResetRigidBody() => StartCoroutine(ResetRigidBodyCoroutine());

    public void SetRigidBody()
    {
        isExploded = true;
        rb.mass = 0.5f;
        rb.gravityScale = 3;
        rb.constraints = RigidbodyConstraints2D.None;
        colliderBody.sharedMaterial = GetComponent<HealthPlayer>().deadPhysicsMaterial;
    }

    private void Dash(Vector2 posRay, Vector2 direction)
    {
        if (direction == Vector2.down)
        {
            if (!Physics2D.OverlapBox(transform.position - new Vector3(0, config.dashDistance, 0), new Vector2(0.2f, 2.3f), 90,
                config.checkDashMask))
            {
                rb.MovePosition(rb.position + direction * config.dashDistance);
                _playerMovementEvents.NotifyDash();
                isDashDown = true;
            }
        }
        else
        {
            Debug.Log($"direction.normalized.x is: {direction.normalized.x}.");
            if (direction.normalized.x < 0)
            {
                rb.MovePosition(rb.position + direction * config.dashDistance);
                _playerMovementEvents.NotifyDash();
            }
            if (!Physics2D.OverlapBox(transform.position + new Vector3(0, config.dashDistance, 0) * direction.normalized.x, new Vector2(0.7f, 1), 90,
                config.checkDashMask))
            {
                rb.MovePosition(rb.position + direction * config.dashDistance);
                _playerMovementEvents.NotifyDash();
            }
        }
    }

    private void DashInput()
    {
        if (staminaControll != null && staminaControll.CurrentStamina >= 1 && !IsCrouch)
        {
            bool dashKeyMain = Input.GetKeyDown(inputConfig.mainDash);

            if (Input.GetKey(inputConfig.down) && dashKeyMain)
                Dash(startRayDown.position, Vector2.down);

            bool leftPressed = Input.GetKey(inputConfig.left) && !Input.GetKey(inputConfig.right);
            bool rightPressed = Input.GetKey(inputConfig.right) && !Input.GetKey(inputConfig.left);

            if (leftPressed && dashKeyMain)
                Dash(startRayLeft.position, Vector2.left);
            if (rightPressed && dashKeyMain)
                Dash(startRayRight.position, Vector2.right);
        }
    }

    private void SitDown()
    {
        needToStandUp = false;
        colliderBody.size = new Vector2(colliderBody.size.x, 1);
        colliderBody.offset = new Vector2(colliderBody.offset.x, -0.5f);
        _playerMovementEvents.NotifySitDown();
        IsCrouch = true;
    }

    private void StandUp()
    {
        needToStandUp = false;
        colliderBody.size = new Vector2(colliderBody.size.x, 2);
        colliderBody.offset = new Vector2(colliderBody.offset.x, 0);
        IsCrouch = false;
        _playerMovementEvents.NotifyStandUp();
    }

    public void ResetIsDashDown() => isDashDown = false;
}