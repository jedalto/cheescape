using UnityEngine;
using System.Collections;

public class CheeseInteraction : MonoBehaviour
{
    public Animator cheeseAnimator; // Drag the Animator component of the cheese in the Inspector
    private GameManager gameManager;
    public float animationDelay = 1f; // Adjust this to match your cheese win animation duration

    void Start()
    {
        // Find the GameManager if not assigned in Inspector
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        // If no animator is assigned, try to get it from this GameObject
        if (cheeseAnimator == null)
        {
            cheeseAnimator = GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Trigger the win sequence
            WinGame();
        }
    }

    void WinGame()
    {
        // Play cheese win animation if animator is assigned
        if (cheeseAnimator != null)
        {
            cheeseAnimator.SetTrigger("Win"); // Create this trigger in your Animator
        }

        // Start coroutine to delay the game win
        StartCoroutine(WinGameWithDelay());
    }

    IEnumerator WinGameWithDelay()
    {
        // Wait for the animation to play
        yield return new WaitForSecondsRealtime(animationDelay);

        // Call the GameManager's win method
        if (gameManager != null)
        {
            gameManager.WinGame();
        }
    }
}
