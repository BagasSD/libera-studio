using UnityEngine;

public class FlyingEnemies : MonoBehaviour
{
    public float speed = 5f;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    private Animator animator;
    private Rigidbody2D rb;
    public DetectionZone attackZone;



    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public float AttackCooldown
    {
        get { return animator.GetFloat(AnimationStrings.attackCooldown); }
        private set { animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0)); }
    }


    public bool HasTarget
    {
        get { return _hasTarget; }
        set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }
    private bool _hasTarget = false;

    public bool IsAttacking
    {
        get { return _isAttacking; }
        set
        {
            _isAttacking = value;
            animator.SetBool(AnimationStrings.isAttacking, value);
        }
    }

    private bool _isAttacking = false;
    // Update is called once per frame
    void Update()
    {
        HasTarget = attackZone.detectedCollider.Count > 0;
        IsAttacking = animator.GetBool(AnimationStrings.isAttacking);
        if (AttackCooldown > 0 && _hasTarget)
        {
            AttackCooldown -= Time.deltaTime;
        }
        else
        {
            if (_hasTarget && _isAttacking)
            {
                LaunchFireballAttack();
            }
        }

        if (!_hasTarget)
        {
            AttackCooldown = 2f;
        }

    }



    void LaunchFireballAttack()
    {
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            GameObject fireball = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            // Set the fireball's direction towards the player
            Vector3 direction = GameObject.FindGameObjectWithTag("Player").transform.position - fireball.transform.position;
            fireball.GetComponent<Rigidbody2D>().velocity = direction.normalized * speed;
            IsAttacking = false;

        }
    }
}
