using UnityEngine;

public class skeletons : MonoBehaviour
{
    // Inspector-exposed variables
    public float walkSpeed = 1f;
    private float walkStopRate = 0.6f;
    public Transform attackCollider;
    public enum WalkableDirection { Right, Left }
    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector;

    // Components and references
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

            // Set the walking direction vector
            walkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            _walkDirection = value;
        }
    }

    // Booleans to control character state

    private void Awake()
    {
        // Get necessary components on Awake
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        UpdateWalkDirection();

    }
    public bool CanMove => animator.GetBool(AnimationStrings.canMove);
    private void FixedUpdate()
    {
        if (!damageable.isHit)
        {

            if (CanMove)
            {

                UpdateWalkDirection();
                UpdateLocalScale();

                // Move the character
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            }
            else
            {
                // If not moving, gradually slow down the character
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
            }

        }
        if (!animator.GetBool((string)AnimationStrings.isAlive))
        {

            if (attackCollider != null)
            {
                Destroy(attackCollider.gameObject);
            }

        }
    }

    private void UpdateLocalScale()
    {
        Vector3 localScale = transform.localScale;

        if (localScale.x < 0 && walkDirectionVector.x < 0)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        else if (localScale.x > 0 && walkDirectionVector.x > 0)
        {
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
    private void UpdateWalkDirection()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.Normalize();

        WalkDirection = (directionToPlayer.x < 0) ? WalkableDirection.Left : WalkableDirection.Right;
    }

    // Handle character being hit
    public void OnHit(int damage, Vector2 knockback)
    {
        if (animator.GetBool((string)AnimationStrings.isAlive))
        {
            rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        }
    }
}
