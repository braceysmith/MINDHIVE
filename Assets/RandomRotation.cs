using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    public float minRotationX = 0.0f;
    public float maxRotationX = 360.0f;
    public float minRotationY = 0.0f;
    public float maxRotationY = 360.0f;
    public float minRotationZ = 0.0f;
    public float maxRotationZ = 360.0f;
    public float rotationSpeed = 1.0f;

    private void Start()
    {
        StartCoroutine(Rotate());
    }

    private IEnumerator Rotate()
    {
        while (true)
        {
            Quaternion targetRotation = Quaternion.Euler(Random.Range(minRotationX, maxRotationX),
                                                         Random.Range(minRotationY, maxRotationY),
                                                         Random.Range(minRotationZ, maxRotationZ));
            while (transform.rotation != targetRotation)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                yield return null;
            }
        }
    }
}
