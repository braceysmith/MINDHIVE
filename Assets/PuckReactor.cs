using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckReactor : MonoBehaviour
{
    [SerializeField] private Color _highlightColor = Color.red;
    private Color _originalColor;
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalColor = _renderer.material.color;
    }

    public void OnTriggerEnter(Collider other)
    {
        _renderer.material.color = _highlightColor;
        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
            Debug.Log("Beam hit");
            _renderer.material.color = _highlightColor;
            return;
        }

        //this.Health -= 0.1f;
    }

    /// <summary>
    /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
    /// We're going to affect health while the beams are interesting the player
    /// </summary>
    /// <param name="other">Other.</param>
    public void OnTriggerExit(Collider other)
    {

        _renderer.material.color = _originalColor;

        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
            Debug.Log("Beam left");
            _renderer.material.color = _originalColor;
            return;
        }

        // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
        //this.Health -= 0.1f*Time.deltaTime;
    }
}
