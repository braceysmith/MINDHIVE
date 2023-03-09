using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject target;

    [SerializeField] private bool followOnAxis = true;


    // Update is called once per frame
    void Update()
    {
        Vector3 cameraPosition = cam.transform.position;

        Vector3 cameraDirection = cameraPosition - transform.position;

        Quaternion cameraRotation = Quaternion.LookRotation(new Vector3(cameraDirection.x,cameraDirection.y, 0));

        transform.rotation = cameraRotation;

        transform.LookAt(target.transform);

        if (followOnAxis)
        {
            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, transform.position.z));
        }
        else
        {
            transform.LookAt(new Vector3(transform.position.x, target.transform.position.y, transform.position.z));
        }
    }
}
