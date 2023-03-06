using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckReactor : MonoBehaviour
{
    [SerializeField] private Color _highlightColor = Color.red;

    private Material _originalMaterial;
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.material;
    }

    public void OnTriggerEnter(Collider other)
    {

        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
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
    public void OnTriggerStay(Collider other)
    {
        

        // We are only interested in Beamers
        // we should be using tags but for the sake of distribution, let's simply check by name.
        if (!other.name.Contains("Beam"))
        {
            _renderer.material.color = _originalMaterial.color;
            return;
        }

        // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
        //this.Health -= 0.1f*Time.deltaTime;
    }
}
