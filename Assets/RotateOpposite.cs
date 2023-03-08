using UnityEngine;

public class RotateOpposite : MonoBehaviour
{
    public Transform target; // The target object to rotate opposite to
    public float speedFraction = 0.5f; // The fraction of the target object's rotation speed to use

    private Quaternion lastTargetRotation; // The previous target rotation
    private float targetAngularSpeed; // The target object's angular speed

    // Start is called before the first frame update
    void Start()
    {
        // Get the initial target rotation
        lastTargetRotation = target.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the target angular speed by comparing the current and previous target rotations
        targetAngularSpeed = Quaternion.Angle(lastTargetRotation, target.rotation) / Time.deltaTime;

        // Get the direction from this object to the target, only considering the Y-axis
        Vector3 direction = target.position - transform.position;
        direction.y = 0f;

        // Calculate the target rotation based on the opposite direction, only rotating on the Y-axis
        Quaternion targetRotation = Quaternion.LookRotation((direction.normalized) * -1, Vector3.up);

        // Smoothly rotate towards the target rotation using a fraction of the target object's angular speed
        float rotationSpeed = targetAngularSpeed * speedFraction;
        Quaternion yRotation = Quaternion.AngleAxis(targetRotation.eulerAngles.y, Vector3.up);
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, yRotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        // Update the previous target rotation for next frame
        lastTargetRotation = target.rotation;
    }
}
