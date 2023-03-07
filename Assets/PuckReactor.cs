using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckReactor : MonoBehaviour
{
    [SerializeField] private Color _highlightColor = Color.red;
    private Color _originalColor;
    private Renderer _renderer;
    private int beams = 0;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    public void OnTriggerEnter(Collider other)
    {
        _renderer.material.color = _highlightColor;
        beams++;
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
        
    }
}
