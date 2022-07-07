using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ken.PhotonTest.Client
{
    [RequireComponent(typeof(InputField))]
    public class PlayerName : MonoBehaviour
    {
        #region Private Constants
        [SerializeField]
        private InputField _inputField ;
        [SerializeField]
        private string _lastValue = string.Empty;
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {            
            _inputField = GetComponent<InputField>();           
        }
        private void Update()
        {
            CheckNameChange();
            
        }
        #endregion
        
        #region Methods
        private void CheckNameChange() {            
            if (_inputField.text == _lastValue)
                return;

            
            if(Input.GetKeyDown(KeyCode.Return))
            _lastValue = _inputField.text;

           
            SetPlayerName(_lastValue);
        }
        private void SetPlayerName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            PhotonNetwork.NickName = name;
        }


        #endregion

    }
}