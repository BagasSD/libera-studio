using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import the TextMeshPro namespace

public class statueCounter : MonoBehaviour
{
    // Reference to the allStatues GameObject
    private GameObject allStatues;

    // Reference to the TextMeshPro text component
    public TextMeshProUGUI textMeshProText;

    // Start is called before the first frame update
    void Start()
    {
        // Find the allStatues GameObject using its name or tag
        allStatues = GameObject.FindGameObjectWithTag("allStatues");

        if (allStatues == null)
        {
            Debug.LogError("allStatues GameObject not found in the scene. Make sure it exists and is named correctly.");
        }

        // Find the TextMeshPro text component on this GameObject
        textMeshProText = GetComponent<TextMeshProUGUI>();

        if (textMeshProText == null)
        {
            Debug.LogError("TextMeshPro text component not found on this GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (allStatues != null && textMeshProText != null)
        {
            // Assuming allStatues has a script with totalStatues and sealedStatues variables
            allStatues statueManager = allStatues.GetComponent<allStatues>();

            if (statueManager != null)
            {
                // Access the totalStatues and sealedStatues variables
                int totalStatues = statueManager.totalNumber;
                int sealedStatues = statueManager.sealedNumber;

                // Update the TextMeshPro text with the values
                textMeshProText.text = sealedStatues + " / " + totalStatues;
            }
            else
            {
                Debug.LogError("StatueManager script not found on the allStatues GameObject.");
            }
        }
    }
}
