using System.Collections;

using UnityEngine;
public class projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public int AttackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    public float destroyDelay = 3f; // Adjust this value as needed
    public float breakTime = 0.3f; // Adjust this value as needed


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Destroy the projectile after a delay
        StartCoroutine(DestroyAfterDelay());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();
        animator.SetBool(AnimationStrings.isHit, true);

        if (damageable != null)
        {
            Vector2 deliveredKnockBack = knockback;

            // Compare projectile's position with player's position
            if (transform.position.x < collision.transform.position.x)
            {
                // Projectile comes from the left, so apply knockback to the right
                deliveredKnockBack = new Vector2(Mathf.Abs(knockback.x), knockback.y);
            }


            damageable.Hit(AttackDamage, deliveredKnockBack);
        }
        rb.velocity = Vector2.zero;
        Destroy(gameObject, breakTime);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);
        animator.SetBool(AnimationStrings.isHit, true);

        // Optionally reset the animation state before destroying

        Destroy(gameObject, breakTime);
    }
}
