using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 50.0f;

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        float verticalRotation = verticalInput * _rotationSpeed * Time.deltaTime;
        float horizontalRotation = horizontalInput * _rotationSpeed * Time.deltaTime;

        transform.Rotate(-verticalRotation, horizontalRotation, 0);
    }
}
