using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ken.PhotonTest.Client
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields
        [SerializeField]
        private byte maxPlayersPerRoom = 4;

        #endregion



        #region Private Fields

        string gameVersion = "1";
        
        [Tooltip("The Ui Panel to let the user enter name, connect and play")]
        [SerializeField]
        private GameObject controlPanel;
        [Tooltip("The UI Label to inform the user that the connection is in progress")]
        [SerializeField]
        private GameObject progressLabel;

        private bool IsConnecting;


        #endregion



        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {

            //Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            //PhotonNetwork.CreateRoom(null, new RoomOptions());
            //PhotonNetwork.JoinRandomRoom();
            Debug.Log(IsConnecting);
            if (IsConnecting)
            {
                PhotonNetwork.JoinOrCreateRoom("default", new RoomOptions() { MaxPlayers = maxPlayersPerRoom },TypedLobby.Default);
                progressLabel.SetActive(false);
                controlPanel.SetActive(true);
                IsConnecting = false;
            }    
            
        }
        #endregion
        public override void OnDisconnected(DisconnectCause cause)
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
            //Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            base.OnDisconnected(cause);
        }
        public override void OnJoinedRoom()
        {
            // Only Load as fisrt player, other player rely on PhotonNetwork.AutomaticallySyncScene to sync scene
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1) 
            {
                Debug.Log("Load Room for 1");

                PhotonNetwork.LoadLevel("Room for 1");
            }
            //Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            PhotonNetwork.CreateRoom(null, new RoomOptions());

        }


        #region MonoBehaviour Callbacks

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;

        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        #endregion



        #region Public Mothods
        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {

                PhotonNetwork.JoinOrCreateRoom("default", new RoomOptions() { MaxPlayers = maxPlayersPerRoom }, TypedLobby.Default);
            }
            else
            {
                //Connect to server
                IsConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }

        }

        #endregion
    }
}