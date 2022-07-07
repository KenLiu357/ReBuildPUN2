using Ken.PhotonTest.Units;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ken.PhotonTest.Client
{
    public class GameManager : MonoBehaviourPunCallbacks
    {

        public static GameManager Instance;

        public GameObject _playerPrefab;

        #region Photon Callbacks
        /// <summary>
        /// Called when the local player left the room and load Launcher Scene
        /// </summary>
        public override void OnLeftRoom() 
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);

            LoadArena();
        }
        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}",other.NickName);
            if (PhotonNetwork.IsMasterClient) 
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}",PhotonNetwork.IsMasterClient);
                LoadArena();
            }
        }


        #endregion


        #region MonoBehaviour Callbacks
        private void Start()
        {
            Instance = this;
            if (_playerPrefab != null)
            {
                if (PlayerManager.LocalPlayerInstance == null)
                {
                    //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    PhotonNetwork.Instantiate(this._playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else 
                {
                    //Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
            

        }


        #endregion

        #region Public Methods

        public void LeaveRoom() 
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion

        #region Private Methods
        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);


            //Loadlevel only called by master client
            PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
        }

        #endregion

    }
}