using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public bool puzzlesCompleted = false;
    public GameObject door; // Reference to the door GameObject

    private void Start()
    {
        // Ensure door is assigned
        if (door == null)
        {
            door = gameObject; // If not assigned, assume the script is on the door
        }

        puzzlesCompleted = false;
    }

    private void Update()
    {
        if (puzzlesCompleted)
        {
            // Deactivate the door when puzzles are completed
            door.SetActive(false);
        }
    }
}
