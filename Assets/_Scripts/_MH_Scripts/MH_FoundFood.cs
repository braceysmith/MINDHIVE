using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    public class MH_FoundFood : MonoBehaviour
    {
       [SerializeField] private GameObject manager;
       // MH_PlayerManager player;
       
        public void OnTriggerEnter(Collider other)
        {
            manager.GetComponent<MH_GameManager>().NextLevel();
            Destroy(this);
        }
    }
}
