using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public GameObject requiredBlock; // The specific block that activates this pressure plate
    public GameObject targetObject; // The object to activate, e.g., a door
    public bool IsActivated { get; private set; } // Property to track activation state

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the required block
        if (other.gameObject == requiredBlock)
        {
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
}
