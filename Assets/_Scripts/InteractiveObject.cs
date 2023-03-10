using UnityEngine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;

public class InteractiveObject : MonoBehaviourPun
{
    PhotonView pV;
    [SerializeField] private bool hivePuck = true;

    private void Start()
    {
        pV = GetComponent<PhotonView>();
    }
    // This function will be called when the trigger is activated
    public void TriggerActivated()
    {
        // Call an RPC to handle the trigger activation on all clients
        photonView.RPC("HandleActivationRPC", RpcTarget.All);
    }

    // This function will be called on all clients when the RPC is called
    [PunRPC]
    private void HandleActivationRPC()
    {
        // Call a function on the game manager to handle the trigger activation
       // mhGameManger.HandleTriggerActivation(pV.ViewID);
       MH_GameManager.HandleTriggerActivation(pV.ViewID);
    }

    public void HandleActivation() {


    }

  
}
