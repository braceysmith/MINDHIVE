using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Photon.Pun.Demo.PunBasics
{
    public class MH_FoundDeath : MonoBehaviour
    {
        MH_GameManager manager;

        private void Start()
        {
            manager = FindObjectOfType<MH_GameManager>();
        }

        public void OnTriggerEnter(Collider other)
        {
            manager.Death();
            Destroy(this);
        }
    }
}
