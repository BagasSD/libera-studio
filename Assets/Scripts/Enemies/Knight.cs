using UnityEngine;

public class Knight : MonoBehaviour
{
    // Inspector-exposed variables
    public float walkSpeed = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;


    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    // Components and references
    private TouchingDirections touchingDirections;
    private Animator animator;
    private Damageable damageable;
    private Rigidbody2D rb;
    private Transform player;


    // Properties
    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set
        {
            if (_walkDirection != value)
            {
                // Flip the character's sprite horizontally
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;

                // Set the walking direction vector
                walkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            }
            _walkDirection = value;
        }
    }

    // Booleans to control character state
    public bool HasTarget
    {
        get { return _hasTarget; }
        set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
            if (_hasTarget)
            {
                _onBattle = true;
                animator.SetBool(AnimationStrings.battleMode, true);
            }
            else
            {
                _onBattle = false;
                animator.SetBool(AnimationStrings.battleMode, false);
            }
        }
    }
    private bool _hasTarget = false;
    private bool _onBattle = false;


    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        private set { animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); }
    }

    private float walkStopRate = 0.6f;

    private void Awake()
    {
        // Get necessary components on Awake
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        // Handle movement and flipping direction
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall || cliffDetectionZone.detectedCollider.Count == 0)
        {
            FlipDirection();
        }

        // Control character movement
        if (!damageable.isHit)
        {
            if (CanMove)
            {
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }
        }
    }

    // Change the walking direction
    private void FlipDirection()
    {

        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Error: Unexpected walk direction.");
        }
    }

    void Update()
    {
        // Update target detection and attack cooldown
        HasTarget = attackZone.detectedCollider.Count > 0;
        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }

        if (_onBattle)
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();

            // Check if the player is on the left
            if (AttackCooldown != 0)
            {
                if (direction.x < 0)
                {
                    WalkDirection = WalkableDirection.Left;
                }
                else
                {
                    WalkDirection = WalkableDirection.Right;
                }

            }
        }
    }

    // Handle character being hit
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
