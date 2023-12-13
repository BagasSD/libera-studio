using UnityEngine;

public class wolf : MonoBehaviour
{
    // Inspector-exposed variables
    public float walkSpeed = 6f;
    public DetectionZone cliffDetectionZone;

    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection = WalkableDirection.Left; // Set the initial direction to Left
    private Vector2 walkDirectionVector = Vector2.left;

    // Components and references
    private TouchingDirections touchingDirections;
    private Animator animator;
    private Damageable damageable;
    private Rigidbody2D rb;

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

    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    private float walkStopRate = 0.6f;

    private void Awake()
    {
        // Get necessary components on Awake
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
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
        // You can add additional logic in Update if needed
    }

    // Handle character being hit
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
