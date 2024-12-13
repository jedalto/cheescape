using UnityEngine;
using TMPro; // For TextMeshPro; if using UI Text, replace accordingly.

public class PressurePlateManager : MonoBehaviour
{
    public PressurePlate[] pressurePlates; // Array to store all pressure plates
    public TextMeshProUGUI completionText; // Reference to the UI Text
    public DoorManager doorManager; // Reference to the DoorManager script
    private bool allPlatesEverActivated = false;

    private int activatedCount = 0; // Tracks the number of plates activated

    private void Start()
    {
        // Initialize text with progress count
        UpdateCompletionText();

        // Ensure doorManager is assigned
        if (doorManager == null)
        {
            doorManager = FindObjectOfType<DoorManager>();
        }
    }

    private void Update()
    {
        // Only check if we haven't already completed the puzzle
        if (!allPlatesEverActivated)
        {
            // Count how many plates have been activated
            int currentActivatedCount = 0;
            foreach (PressurePlate plate in pressurePlates)
            {
                if (plate.HasEverBeenActivated)
                {
                    currentActivatedCount++;
                }
            }

            // Check if there's a change in the activated count
            if (currentActivatedCount != activatedCount)
            {
                activatedCount = currentActivatedCount;
                UpdateCompletionText();
            }

            // If all plates are activated
            if (activatedCount == pressurePlates.Length)
            {
                allPlatesEverActivated = true;
                Debug.Log("All objectives achieved!");
                completionText.text = "All Objectives Achieved! The door is open.";

                // Set the puzzles completed flag on the door manager
                if (doorManager != null)
                {
                    doorManager.puzzlesCompleted = true;
                }
                else
                {
                    Debug.LogWarning("DoorManager reference is missing!");
                }
            }
        }
    }

    private void UpdateCompletionText()
    {
        if (!allPlatesEverActivated)
        {
            completionText.text = $"{activatedCount} of {pressurePlates.Length} objectives achieved.";
        }
    }
}

