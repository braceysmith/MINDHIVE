using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;

public class PuckReactor : MonoBehaviour
{
    PhotonView pV;

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

    [SerializeField]
    private GameObject[] safeIslands;
    [SerializeField]
    private GameObject[] foodIslands;
    [SerializeField]
    private GameObject[] deathIslands;


    [SerializeField]
    private GameObject[] adjacentIslands;
    //[SerializeField]
    //private GameObject[] IslandRowOne;
    //[SerializeField]
    //private GameObject[] IslandRowTwo;
    //[SerializeField]
   // private GameObject[] IslandRowThree;
   // [SerializeField]
   // private GameObject[] IslandRowFour;

    private GameObject selectedLocation;
    private Quaternion startRotation;
    private Quaternion endRotation;

    // Start is called before the first frame update
    void Start()
    {

        pV = GetComponent<PhotonView>();
        Debug.Log("Here ID: " + pV.ViewID);
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

        ActivateRotation();
        Flock();
        //int playerCount = PhotonNetwork.PlayerList.Length;
        //if (beams == playerCount)
            //PreFlockBroadcast();


       Debug.Log("TriggerEEnter for ID: " + pV.ViewID);
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


        Debug.Log("TriggerExit for ID: " + pV.ViewID);
        beams--;
        if(beams==0)
            _renderer.material.color = _originalColor;
        ActivateRotationBack();
        NoFlock();
        //PreNoFlockBroadcast();
    }

    public void ActivateRotation()
    {

        Debug.Log("ActivateRotation for ID: " + pV.ViewID);
        if (tile != null)
        {
            if (tile.transform.GetChild(0).gameObject.transform.childCount==0)
            {
                SelectLocation();
            }
            else
            {
                tile.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            StopCoroutine("RotateBack");
            StartCoroutine("Rotate");
        }
    }

    public void ActivateRotationBack()
    {

        Debug.Log("ActivateRotationBack for ID: " + pV.ViewID);
        if (tile != null && beams==0)
        {
            StopCoroutine("Rotate");
            StartCoroutine("RotateBack");
        }
    }

    private IEnumerator Rotate()
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

    private IEnumerator RotateBack()
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
            tile.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    private void SelectLocation()
    {
        if (location.Length == 0)
        {
            //Debug.LogError("No objects no locations.");
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
    

    public void Flock()
    {
        Debug.Log("Flock Good");
        for (int i = 0; i < adjacentIslands.Length; i++)
        {
            Debug.Log("Flock for Good");
            if (adjacentIslands[i] != null)
            {
                Debug.Log("Flock for i= " + i + "good");
                adjacentIslands[i].GetComponent<PuckReactor>().ActivateRotation();
            }

        }
    }

    public void NoFlock()
    {

        Debug.Log("NoFlock Good");
        for (int i = 0; i < adjacentIslands.Length; i++)
        {
            Debug.Log("Flock for loop Good");
            if (adjacentIslands[i] != null)
            {
                Debug.Log("NoFlock for i= " + i + "good");
                adjacentIslands[i].GetComponent<PuckReactor>().ActivateRotationBack();
            }

        }
    }

    public void PreFlockBroadcast()
    {
        // Call an RPC to handle the trigger activation on all clients
        pV.RPC("FlockBraodcast", RpcTarget.All);
    }


    public void PreNoFlockBroadcast()
    {
        // Call an RPC to handle the trigger activation on all clients
        pV.RPC("NoFlockBraodcast", RpcTarget.All);
    }

    [PunRPC]
    private void FlockBroadcast()
    {
        //Debug.Log("Flock Braodcast good");
        MH_GameManager.HandlePuckTriggerActivation(pV.ViewID);
    }
    [PunRPC]
    private void NoFlockBroadcast()
    {
        //Debug.Log("NoFlock Braodcast good");
        MH_GameManager.HandlePuckTriggerDectivation(pV.ViewID);
    }
}
