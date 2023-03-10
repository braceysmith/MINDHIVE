using UnityEngine;

public class PointObjectDirection : MonoBehaviour
{
    public Transform targetTransform; // The target object to look at
    public Vector3 axis = Vector3.up; // The axis to rotate around
    public float rotationSpeed = 10f; // The rotation speed in degrees per second

    // Update is called once per frame
    void Update()
    {
        // Calculate the direction to the target
        Vector3 direction = targetTransform.position - transform.position;

        // Calculate the target rotation using LookRotation and the specified axis
        Quaternion targetRotation = Quaternion.LookRotation(direction, axis);

        // Use Slerp to rotate towards the target rotation
        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}