using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform playerTransform; // Reference to the player's transform

    void FixedUpdate()
    {
        // Get the player's current position
        Vector3 newPosition = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);

        // Update the camera's position
        transform.position = newPosition;
    }
}
