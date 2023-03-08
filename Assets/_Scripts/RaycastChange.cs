using UnityEngine;

public class RaycastChange : MonoBehaviour
{
    [SerializeField] private Color _highlightColor = Color.red;
    [SerializeField] private float _raycastDistance = 10.0f;

    private Material _originalMaterial;
    private Renderer _renderer;
    public bool _isHighlighted = false;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _originalMaterial = _renderer.material;
    }

    void Update()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, _raycastDistance);

        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject == gameObject && !_isHighlighted)
                {
                    _renderer.material.color = _highlightColor;
                    _isHighlighted = true;
                    break;
                }
            }
        }
        else if (_isHighlighted)
        {
            _renderer.material.color = _originalMaterial.color;
            _isHighlighted = false;
        }
}
}