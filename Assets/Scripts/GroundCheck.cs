using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    // The layermask to identify platforms
    [SerializeField] private LayerMask platformLayerMask;

    // Boolean indicating whether the object is grounded
    public bool isGrounded;

    private void OnTriggerStay2D(Collider2D collider)
    {
        // CHeck if the collider belongs to the platform layer mask
        isGrounded = collider != null && (((1 << collider.gameObject.layer) & platformLayerMask) != 0);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Set isGrounded to false when the collider exits
        isGrounded = false;
    }

}
