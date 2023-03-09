using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAxis : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 60f, 0); // The rotation speed for each axis

    // Update is called once per frame
    void Update()
    {
        // Rotate the object around each axis at the given speed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}
