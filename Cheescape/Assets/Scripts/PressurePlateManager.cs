using UnityEngine;
using TMPro; // For TextMeshPro; if using UI Text, replace accordingly.

public class PressurePlateManager : MonoBehaviour
{
    public PressurePlate[] pressurePlates; // Array to store all pressure plates
    public TextMeshProUGUI completionText; // Reference to the UI Text
    public DoorManager doorManager; // Reference to the DoorManager script
    private bool allPlatesEverActivated = false;

    private void Start()
    {
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
            // Check if all plates are activated at least once
            bool allPressed = true;
            foreach (PressurePlate plate in pressurePlates)
            {
                if (!plate.HasEverBeenActivated)
                {
                    allPressed = false;
                    break;
                }
            }

            // Update the UI text and door state
            if (allPressed)
            {
                allPlatesEverActivated = true;
                Debug.Log("All plates have been activated!");
                completionText.text = "All Plates Activated!";

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
            else
            {
                completionText.text = "";
            }
        }
    }
}
