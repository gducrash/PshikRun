using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController cc;
    public PlayerAnimator playerAnimator;
    public GameManager gameManager;

    [Header("Vertical Movement Settings")]
    public float gravity = 10;
    public float jumpHeight = 2;
    public float rollSpeed = 10;

    [Header("Horizontal Movement Settings")]
    public float speedX = 5;
    public float accelerationX = 12;
    public float decelerationX = 8;
    public float boundsX = 2;

    [Header("Forward Movement Settings")]
    public float zHitDeathThreshold = 5;
    public bool noClip = false;


    [Header("State"), HideInInspector]
    public bool isDead = false;
    [HideInInspector]
    public Vector3 velocity = Vector3.zero;
    [HideInInspector]
    public bool isOnFloor = false;

    InputSytemActions inputActions;
    Vector2 moveInput;
    bool prevIsOnFloor = false;

    private void Awake()
    {
        inputActions = new InputSytemActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        transform.localPosition = new Vector3(0, 1, 0);
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        float speedMultiplier = gameManager.speedMultiplier;

        isOnFloor = cc.isGrounded;
        if (isOnFloor && !prevIsOnFloor)
        {
            playerAnimator.TriggerGrounded();
        }
        prevIsOnFloor = isOnFloor;

        // Y velocity
        velocity.y -= gravity * dt * speedMultiplier * speedMultiplier;
        if (isOnFloor)
        {
            velocity.y = 0;
            if (moveInput.y > 0)
            {
                Jump();
            }
        } else
        {
            if (moveInput.y < 0)
            {
                velocity.y = -rollSpeed * speedMultiplier;
            }
        }

        // X velocity
        float targetVelocityX = moveInput.x * speedX * speedMultiplier;
        float dampingX = targetVelocityX == 0
            ? decelerationX 
            : accelerationX;

        velocity.x += (targetVelocityX - velocity.x) * dampingX * dt * speedMultiplier;

        // Movement
        Vector3 initPos = transform.localPosition + velocity * dt;
        cc.Move(velocity * dt);
        Vector3 afterPos = transform.localPosition;

        // Gameover detection
        if (!gameManager.isGameOver)
        {
            Vector3 hit = (afterPos - initPos) / dt;
            if (
                Mathf.Abs(hit.z) > Mathf.Abs(hit.y) &&
                Mathf.Abs(hit.z) > Mathf.Abs(hit.x) &&
                -hit.z > zHitDeathThreshold &&
                !noClip
            )
            {
                gameManager.GameOver();
            }

            transform.localPosition = new Vector3(
                Mathf.Clamp(transform.localPosition.x, -boundsX, boundsX),
                transform.localPosition.y,
                0
            );
        }

    }

    void Update()
    {
        if (gameManager.isGameOver)
        {
            moveInput = Vector2.zero;
            return;
        }
        moveInput = inputActions.Player.Move.ReadValue<Vector2>();
    }

    public void Jump()
    {
        float speedMultiplier = gameManager.speedMultiplier;
        velocity.y = Mathf.Sqrt(jumpHeight * 2 * gravity * speedMultiplier * speedMultiplier);
        playerAnimator.TriggerJump();
    }
}
