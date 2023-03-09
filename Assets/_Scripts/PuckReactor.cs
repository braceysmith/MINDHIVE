using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckReactor : MonoBehaviour
{
    [SerializeField] private Color _highlightColor = Color.red;
    private Color _originalColor;
    private Renderer _renderer;
    private int beams = 0;

    [SerializeField] private float _rotationAngle = 180.0f;
    [SerializeField] private float _rotationTime = .25f;
    private float rotX;
    private float rotY;
    [SerializeField]
    private GameObject tile;
    [SerializeField]
    private GameObject[] location;
    private GameObject selectedLocation;
    private Quaternion startRotation;
    private Quaternion endRotation;

    // Start is called before the first frame update
    void Start()
    {
        startRotation = tile.transform.localRotation;
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    public void OnTriggerEnter(Collider other)
    {
        _renderer.material.color = _highlightColor;
        beams++;
        if (tile != null)
        {
            SelectLocation();
            StopCoroutine("RotateBack");
            StartCoroutine("Rotate");
        }
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
            Debug.Log("Beam hit");
            _renderer.material.color = _highlightColor;
            return;
        }
    }

    /// <summary>
    /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
    /// We're going to affect health while the beams are interesting the player
    /// </summary>
    /// <param name="other">Other.</param>
    public void OnTriggerExit(Collider other)
    {

        beams--;
        if(beams==0)
            _renderer.material.color = _originalColor;
        if (tile != null)
        {
            StopCoroutine("Rotate");
            StartCoroutine("RotateBack");
        }

    }

    IEnumerator Rotate()
    {
        Quaternion currentRotation = tile.transform.localRotation;
        endRotation = startRotation * Quaternion.Euler(rotX, rotY, _rotationAngle);

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / _rotationTime;
            tile.transform.localRotation = Quaternion.Lerp(currentRotation, endRotation, t);
            yield return null;
        }
    }

    IEnumerator RotateBack()
    {
        Quaternion currentRotation = tile.transform.localRotation;

        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime / _rotationTime;
            tile.transform.localRotation = Quaternion.Lerp(currentRotation, startRotation, t);
            yield return null;
        }
        if (selectedLocation != null)
            Destroy(selectedLocation);
    }

    private void SelectLocation()
    {
        if (location.Length == 0)
        {
            Debug.LogError("No objects no locations.");
            return;
        }
        GameObject tileChild = tile.transform.GetChild(0).gameObject;
        int randomIndex = Random.Range(0, location.Length);
        GameObject selectedObjectPrefab = location[randomIndex];

        selectedLocation = Instantiate(selectedObjectPrefab);
        selectedLocation.transform.parent = tileChild.transform;
        selectedLocation.transform.localRotation = Quaternion.Euler(0, 0, 0);
        selectedLocation.transform.localPosition = new Vector3(0, .01f, 0);
        selectedLocation.transform.localScale = Vector3.one;
        //Debug.Log($"Selected object: {selectedObject.name}");
    }
}
