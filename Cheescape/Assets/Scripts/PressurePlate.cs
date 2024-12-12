using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject requiredBlock; // The specific block that activates this pressure plate
    public GameObject targetObject; // The object to activate, e.g., a door

    // Track if the plate has ever been activated
    public bool HasEverBeenActivated { get; private set; }

    // Track current activation state
    public bool IsActivated { get; private set; }

    // Optional: Track the first time the plate is activated
    private float firstActivationTime;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the required block
        if (other.gameObject == requiredBlock)
        {
            // If this is the first activation, record the time
            if (!HasEverBeenActivated)
            {
                firstActivationTime = Time.time;
                HasEverBeenActivated = true;
            }

            IsActivated = true;
            ActivateTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the object leaving is the required block
        if (other.gameObject == requiredBlock)
        {
            IsActivated = false;
            DeactivateTarget();
        }
    }

    private void ActivateTarget()
    {
        if (targetObject != null)
        {
            // Perform the activation logic (e.g., open a door)
            targetObject.SetActive(true);
        }
    }

    private void DeactivateTarget()
    {
        if (targetObject != null)
        {
            // Perform the deactivation logic (e.g., close a door)
            targetObject.SetActive(false);
        }
    }

    // Optional method to get the time of first activation
    public float GetFirstActivationTime()
    {
        return HasEverBeenActivated ? firstActivationTime : -1f;
    }

    // Optional method to reset the plate's state (useful for puzzle resets)
    public void Reset()
    {
        IsActivated = false;
        HasEverBeenActivated = false;
        firstActivationTime = 0f;
        DeactivateTarget();
    }
}
