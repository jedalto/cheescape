using UnityEngine;

public class Push : MonoBehaviour
{
    public float pushForce = 5f; // Force applied to the block

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        // Ensure we're hitting a Rigidbody that's not kinematic
        if (rb != null && !rb.isKinematic)
        {
            // Get the direction to push
            Vector3 pushDirection = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}
