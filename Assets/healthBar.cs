using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healthBar : MonoBehaviour
{
    private GameObject player; // Reference to the player GameObject
    private float maxHP; // Maximum health (initial width of RectTransform)
    private float currentWidth; // Current health (current width of RectTransform)

    // Start is called before the first frame update
    void Start()
    {
        // Find the player GameObject using its tag
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found in the scene. Make sure the player has the 'Player' tag.");
        }

        // Get the initial width of the health bar RectTransform as the maximum health
        RectTransform healthBarRectTransform = GetComponent<RectTransform>();
        if (healthBarRectTransform != null)
        {
            maxHP = healthBarRectTransform.sizeDelta.x;
            currentWidth = maxHP; // Set the current health to the maximum health initially
        }
        else
        {
            Debug.LogError("RectTransform component not found on the health bar GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            // Assuming the player has a "damageable" component with a "health" variable
            Damageable playerDamageable = player.GetComponent<Damageable>();

            if (playerDamageable != null)
            {
                // Access the health variable from the damageable component
                float playerHealth = playerDamageable.Health;

                // Update the currentWidth variable based on the player's health
                currentWidth = Mathf.Lerp(currentWidth, maxHP * (playerHealth / playerDamageable.MaxHealth), Time.deltaTime * 5f);

                // Now you can use the currentWidth variable to update the health bar width.
                UpdateHealthBar(currentWidth);
            }
            else
            {
                Debug.LogError("Damageable component not found on the player GameObject.");
            }
        }
    }

    // Example method to update the health bar width based on the current health
    void UpdateHealthBar(float width)
    {
        // Assuming the health bar has a RectTransform component
        RectTransform healthBarRectTransform = GetComponent<RectTransform>();

        if (healthBarRectTransform != null)
        {
            // Update the width of the health bar RectTransform
            healthBarRectTransform.sizeDelta = new Vector2(width, healthBarRectTransform.sizeDelta.y);
        }
        else
        {
            Debug.LogError("RectTransform component not found on the health bar GameObject.");
        }
    }
}
