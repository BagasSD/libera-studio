using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public bool isSealed = false;

    public GameObject enemiesPrefab;
    public Transform[] spawnPoints;
    public DetectionZone sealZone;
    private GameObject[] spawnedEnemies; // References to the instantiated enemies
    private Animator animator;
    public GameObject rune;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isSealed)
        {
            SpawnEnemies();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isSealed)
        {
            DeSpawnEnemies();
        }
    }

    private void DeSpawnEnemies()
    {

        for (int i = 0; i < spawnedEnemies.Length; i++)
        {
            if (spawnedEnemies[i] != null)
            {
                Animator enemyAnimator = spawnedEnemies[i].GetComponent<Animator>();

                if (enemyAnimator != null)
                {
                    // Trigger the despawn animation
                    enemyAnimator.SetBool(AnimationStrings.isDeSpawn, true);

                    // Get the length of the despawn animation and wait for it to finish
                    float despawnAnimationLength = GetAnimationLength(enemyAnimator, "Despawn");
                    StartCoroutine(WaitAndDestroy(spawnedEnemies[i], despawnAnimationLength));
                }
            }
        }
    }

    private float GetAnimationLength(Animator animator, string animationName)
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f;
    }

    private IEnumerator WaitAndDestroy(GameObject enemy, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Destroy(enemy);
    }
    private void SpawnEnemies()
    {
        if (!isSealed)
        {
            // Initialize the array of spawned enemies
            spawnedEnemies = new GameObject[spawnPoints.Length];

            for (int i = 0; i < spawnPoints.Length; i++)
            {
                // Instantiate each enemy prefab and store the reference
                spawnedEnemies[i] = Instantiate(enemiesPrefab, spawnPoints[i].position, Quaternion.identity);
            }

        }
    }

    private IEnumerator RespawnAfterDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Respawn the dead enemy at the specified spawn point
        if (index < spawnedEnemies.Length && !isSealed)            
        {
            spawnedEnemies[index] = Instantiate(enemiesPrefab, spawnPoints[index].position, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        if (spawnedEnemies != null)
        {
            // Check if the enemies are alive using the Animator component
            for (int i = 0; i < spawnedEnemies.Length; i++)
            {
                if (spawnedEnemies[i] != null)
                {
                    Animator enemyAnimator = spawnedEnemies[i].GetComponent<Animator>();

                    if (enemyAnimator != null)
                    {
                        // Assuming "isAlive" is a boolean parameter in your Animator Controller
                        bool isAlive = enemyAnimator.GetBool("isAlive");
                        bool respawned = enemyAnimator.GetBool("respawned");

                        if (!isAlive && !respawned && !isSealed)
                        {
                            // Start a coroutine to respawn the dead enemy after 10 seconds
                            StartCoroutine(RespawnAfterDelay(i, 10f));
                            enemyAnimator.SetBool("respawned", true);
                        }
                    }
                }
            }
        }

        if (isSealed)
        {
            DeSpawnEnemies();
        }
    }
    private bool isSealing = false; // Add a flag to track whether sealing is in progress

    private void Update()
    {
        if (sealZone.detectedCollider.Count > 0)
        {
            animator.SetBool(AnimationStrings.isHovered, true);

            if (Input.GetKey(KeyCode.R) && !isSealed && !isSealing)
            {
                StartCoroutine(SealStatues());
            }

            if (!Input.GetKey(KeyCode.R) && !isSealed)
            {
                // Stop sealing if the key is released
                StopCoroutine(SealStatues());
                isSealing = false;
                animator.SetBool(AnimationStrings.isSealing, false);
            }
        }
        else
        {
            animator.SetBool(AnimationStrings.isHovered, false);
        }
    }

    private IEnumerator SealStatues()
    {
        if (animator.GetBool(AnimationStrings.isHovered))
        {
            isSealing = true; // Set the flag to indicate sealing is in progress
            animator.SetBool(AnimationStrings.isSealing, true);

            float timer = 5f;
            float initialTimer = timer;

            while (timer > 0f)
            {
                // Check if the key is still pressed and isHovered is true
                if (Input.GetKey(KeyCode.R) && animator.GetBool(AnimationStrings.isHovered))
                {
                    // Continue waiting
                    yield return null;
                    timer -= Time.deltaTime;

                    // Update the rune color based on the timer
                    float normalizedTimer = timer / initialTimer;
                    Color runeColor = Color.Lerp(Color.white, Color.black, 1f - normalizedTimer);
                    rune.GetComponent<SpriteRenderer>().color = runeColor;
                }
                else
                {
                    // Key released or isHovered is false, reset the timer and stop sealing
                    timer = 5f;
                    isSealing = false;
                    animator.SetBool(AnimationStrings.isSealing, false);

                    // Reset the rune color to normal
                    rune.GetComponent<SpriteRenderer>().color = Color.white;

                    yield break; // Exit the coroutine
                }
            }

            // Timer has elapsed, complete the sealing process
            animator.SetBool(AnimationStrings.isSealing, false);
            animator.SetBool(AnimationStrings.isSealed, true);
            isSealed = true;
            isSealing = false; // Reset the flag
        }
    }

}
