using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Photon.Pun.Demo.PunBasics
{
    public class MH_FoundFood : MonoBehaviour
    {
       MH_GameManager manager;

        private void Start()
        {
            manager = GameObject.Find("MH_Game Manager").GetComponent<MH_GameManager>();
        }
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.name == "MainBeam")
            {
                manager.NextLevel();
                Destroy(this.gameObject);
            }
        }
    }
}
