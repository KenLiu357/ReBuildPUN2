using Ken.PhotonTest.Client;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Ken.PhotonTest.Units
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        #region IPunObservable implementation


        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            //Send the others our local data
            if (stream.IsWriting)
            {
                stream.SendNext(IsFiring);
                stream.SendNext(Health);
            }
            else
            {
                // For reading from netwok(other clients)
                this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
            }

        }


        #endregion
        #region Private Fields
        [SerializeField]
        private GameObject beams;
        private bool IsFiring;
        [SerializeField]
        public float Health = 1f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        
        [SerializeField]
        private GameObject _playerUIPrefab;


#if UNITY_5_4_OR_NEWER
        private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
#endif


        #endregion

        #region MonoBehaviour Callbacks
        private void Awake()
        {
            if (beams != null)
            {
                beams.SetActive(false);
            }
            if (photonView.IsMine)
                PlayerManager.LocalPlayerInstance = this.gameObject;

            // we flag as don't destroy on load so that instance survives level synchronization,
            // thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);

        }

        private void Start()
        {
            //Add Camera
            CameraWork _cameraWork = this.gameObject.GetComponent<CameraWork>();
            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                    _cameraWork.OnStartFollowing();
            }
            else
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
            }

            //Add PlayerUI
            if (_playerUIPrefab != null)
            {
                GameObject uiGo = Instantiate(_playerUIPrefab);
                uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }

#if UNITY_5_4_OR_NEWER

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

        private void Update()
        {
            if (photonView.IsMine)
            {
                ProcessInputs();
                if (Health <= 0f)
                {
                    Ken.PhotonTest.Client.GameManager.Instance.LeaveRoom();
                }
            }
            if (beams != null && IsFiring != beams.activeInHierarchy)
            {
                beams.SetActive(IsFiring);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (!photonView.IsMine)
                return;
            if (!other.name.Contains("Left Beam"))
                return;
           
            Health -= 0.1f;

        }
        private void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
                return;
            if (!other.name.Contains("Left Beam"))
                return;

            
            Health -= 0.1f * Time.deltaTime;

        }
#if UNITY_5_4_OR_NEWER

        void OnLeveleWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif
        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(this._playerUIPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);

        }
#if UNITY_5_4_OR_NEWER
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }
#endif
        #endregion


        private void ProcessInputs()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (!IsFiring)
                    IsFiring = true;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (IsFiring)
                    IsFiring = false;
            }
        }

    }
}