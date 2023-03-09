using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlappingManager : MonoBehaviour
{
    public float maxRotation = 90f; // The maximum rotation in degrees
    public float rotationSpeed = 10f; // The rotation speed in degrees per second

    private float currentRotation = 0f; // The current rotation in degrees
    public bool rotatePositive = true; // Whether to rotate in the positive direction or negative direction

    // Update is called once per frame
    void Update()
    {
        // Calculate the amount to rotate based on the rotation speed and whether to rotate in the positive or negative direction
        float rotationAmount = rotationSpeed * Time.deltaTime * (rotatePositive ? 1 : -1);

        // Add the rotation amount to the current rotation
        currentRotation += rotationAmount;

        // Clamp the current rotation to the maximum limit in both the positive and negative directions
        float clampedRotation = Mathf.Clamp(currentRotation, -maxRotation, maxRotation);

        // If the current rotation has reached the maximum limit in either direction, switch the direction of rotation
        if (clampedRotation == -maxRotation || clampedRotation == maxRotation)
        {
            rotatePositive = !rotatePositive;
        }

        // Set the local rotation of the object around the y-axis
        transform.localRotation = Quaternion.Euler(0f, clampedRotation, 0f);
    }
}

