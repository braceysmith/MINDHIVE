using UnityEngine;

public class FollowTargetWithDelay : MonoBehaviour
{

    [Tooltip("The distance in the local x-z plane to the target")]
    [SerializeField]
    private float distance = 7.0f;

    [Tooltip("The height we want the camera to be above the target")]
    [SerializeField]
    private float height = 3.0f;

    [Tooltip("Allow the camera to be offseted vertically from the target, for example giving more view of the sceneray and less ground.")]
    [SerializeField]
    private Vector3 centerOffset = Vector3.zero;

    [Tooltip("The Smoothing for the camera to follow the target")]
    [SerializeField]
    private float smoothSpeed = 0.125f;

    // cached transform of the target
    public Transform targetTransform;


    // Cache for camera offset
    Vector3 cameraOffset = Vector3.zero;


    // Update is called once per frame
    void Update()
    {
        Follow();
    }

    void Follow()
    {
        cameraOffset.z = -distance;
        cameraOffset.y = height;

        this.transform.position = Vector3.Lerp(this.transform.position, targetTransform.position + targetTransform.TransformVector(cameraOffset), smoothSpeed * Time.deltaTime);

        this.transform.LookAt(targetTransform.position + centerOffset);

    }
}
