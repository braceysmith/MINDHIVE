using UnityEngine;

public class MouseRotateCamera : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 2.0f;
    [SerializeField] private float _smoothing = 2.0f;

    private Vector2 _mouseLook;
    private Vector2 _smoothV;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(_sensitivity * _smoothing, _sensitivity * _smoothing));
        _smoothV.x = Mathf.Lerp(_smoothV.x, mouseDelta.x, 1f / _smoothing);
        _smoothV.y = Mathf.Lerp(_smoothV.y, mouseDelta.y, 1f / _smoothing);
        _mouseLook += _smoothV;

        transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
        transform.localRotation *= Quaternion.AngleAxis(_mouseLook.x, transform.InverseTransformDirection(Vector3.up));

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}