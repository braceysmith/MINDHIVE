// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Launcher.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Networking Demos
// </copyright>
// <summary>
//  Used in "PUN Basic tutorial" to handle typical game management requirements
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
#pragma warning disable 649

    /// <summary>
    /// Game manager.
    /// Connects and watch Photon Status, Instantiate Player
    /// Deals with quiting the room and the game
    /// Deals with level loading (outside the in room synchronization)
    /// </summary>
    public class MH_GameManager : MonoBehaviourPunCallbacks
    {

        #region Public Fields

        static public MH_GameManager Instance;
        public string scene = "Murmuration";
        public string loader = "Murmuration_Launcher";
        public GameObject[] activeTiles;
        #endregion

        #region Private Fields

        private GameObject instance;

        [Tooltip("The prefab to use for representing the player")]
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private GameObject foodPrefab;

        [SerializeField]
        private GameObject deathPrefab;

        private int level;

        MH_PlayerManager player;
        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            Instance = this;

            // in case we started this demo with the wrong scene being active, simply load the menu scene
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene(loader);

                return;
            }

            if (playerPrefab == null)
            { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

                Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {


                if (PlayerManager.LocalPlayerInstance == null)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 1f, 0f), Quaternion.identity, 0);
                }
                else
                {

                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }


            }

            player = GameObject.Find("Pers(Clone)").GetComponent<MH_PlayerManager>();
            AddFood();
        }
        
        //next season
        public void NextLevel()
        {
            player.Heal();
            level++;
            for(int i = 0; i*2<level; i++)
            {
                AddFood();
            }

            for(int j = 0; j<level; j++)
            {
                AddDeath();
            }
        }

        public void Death()
        {
            LeaveRoom();
        }

        private void AddFood()
        {
            if (foodPrefab != null)
            {
                ChooseEmptyTile(foodPrefab);
            }
        }

        private void AddDeath()
        {
            if (foodPrefab != null)
            {
                ChooseEmptyTile(deathPrefab);
            }

        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            // "back" button of phone equals "Escape". quit app if that's pressed
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitApplication();
            }
        }

        #endregion

        #region Photon Callbacks

        /// <summary>
        /// Called when a Photon Player got connected. We need to then load a bigger scene.
        /// </summary>
        /// <param name="other">Other.</param>
        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        /// <summary>
        /// Called when a Photon Player got disconnected. We need to load a smaller scene.
        /// </summary>
        /// <param name="other">Other.</param>
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

                LoadArena();
            }
        }

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(loader);
        }

        #endregion

        #region Public Methods

        public bool LeaveRoom()
        {
            return PhotonNetwork.LeaveRoom();
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        // This function will handle the trigger activation
        public static void HandleTriggerActivation(int viewID)
        {
            // Use PhotonView.Find to obtain the interactive object across the network
            PhotonView interactiveObjectView = PhotonView.Find(viewID);

            // Call a function on the interactive object to handle the activation
            interactiveObjectView.GetComponent<InteractiveObject>().HandleActivation();
        }

        // This function will be called by the interactive object to handle the activation
        public static void InteractiveObjectActivated()
        {
            Debug.Log("Interactive object activated!");
        }

        public static void HandlePuckTriggerActivation(int viewID)
        {
            // Use PhotonView.Find to obtain the interactive object across the network
            PhotonView interactiveObjectView = PhotonView.Find(viewID);

            // Call a function on the interactive object to handle the activation
            interactiveObjectView.GetComponent<PuckReactor>().Flock();
        }

        public static void HandlePuckTriggerDectivation(int viewID)
        {
            // Use PhotonView.Find to obtain the interactive object across the network
            PhotonView interactiveObjectView = PhotonView.Find(viewID);

            // Call a function on the interactive object to handle the activation
            interactiveObjectView.GetComponent<PuckReactor>().NoFlock();
        }

        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }

            Debug.LogFormat(scene);


            PhotonNetwork.LoadLevel(scene);
        }

        private void ChooseEmptyTile(GameObject prefab)
        {
            // Choose a random game object from the array
            int randomIndex = Random.Range(0, activeTiles.Length);
            AddInteractive(randomIndex, prefab);
        }

        public void AddInteractive(int tileNum, GameObject prefab)
        {
            if (activeTiles[tileNum].transform.childCount > 1)
            {
                ChooseEmptyTile(prefab);
                return;
            }
            else
            {
                GameObject interactive = Instantiate(prefab);
                interactive.transform.SetParent(activeTiles[tileNum].transform.GetChild(0).transform.GetChild(0).transform);
                interactive.transform.localPosition = new Vector3(1.5f, 0, 0);
                interactive.transform.localScale = new Vector3(1,.2f,1);
                interactive.transform.localEulerAngles = new Vector3(0, 0, -90);
            }
        }

        #endregion

    }

}