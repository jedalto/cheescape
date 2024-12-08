using UnityEngine;
using TMPro; // For TextMeshPro; if using UI Text, replace accordingly.

public class PressurePlateManager : MonoBehaviour
{
    public PressurePlate[] pressurePlates; // Array to store all pressure plates
    public TextMeshProUGUI completionText; // Reference to the UI Text
    DoorManager door;

    private void Update()
    {
        // Check if all plates are activated
        bool allPressed = true;
        foreach (PressurePlate plate in pressurePlates)
        {
            if (!plate.IsActivated)
            {
                allPressed = false;
                break;
            }
        }

        // Update the UI text
        if (allPressed)
        {
            Debug.Log("All plates are activated!");
            completionText.text = "All Plates Activated!";
            door.pushPuzzle = true;
        }
        else
        {
            completionText.text = "";
        }
    }
}
