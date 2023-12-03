using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damagableHit; // Event invoked when object is hit

    // Health properties and fields
    [SerializeField] private int _maxHealth = 100;
    public int MaxHealth { get { return _maxHealth; } set { _maxHealth = value; } }

    private int _health = 100;
    public int Health { get { return _health; } set { _health = value; if (_health <= 0) { IsAlive = false; } } }

    // Health status properties
    private bool _isAlive = true;
    [SerializeField] private bool isInvincible = false;

    // Hit and invincibility properties
    public bool isHit { get { return animator.GetBool(AnimationStrings.isHit); } private set { animator.SetBool(AnimationStrings.isHit, value); } }
    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    // Properties to handle life and death states
    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool((string)AnimationStrings.isAlive, value);
        }
    }

    private Animator animator;

    private void Awake()
    {
        // Get the Animator component
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Manage invincibility time
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    // Function to handle the object being hit
    public void Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;
            isHit = true;
            damagableHit?.Invoke(damage, knockback); // Invoke event with damage and knockback
        }
    }
}
