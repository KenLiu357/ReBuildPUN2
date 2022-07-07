using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ken.PhotonTest.Units
{

    public class PlayerUI : MonoBehaviour
    {
        #region Private Fields
        [SerializeField]
        private Text _playerNameText;
        [SerializeField]
        private Slider _playerHealthSlider;

        private PlayerManager _playerTarget;

        [SerializeField]
        private Vector3 _screenOffset = new Vector3(0f,30f, 0f);

        private float _characterControllerHeight = 0f;
        private Transform _targetTrasform;
        private Renderer _targetRenderer;
        private CanvasGroup _canvasGroup;
        private Vector3 _targetPosition;

        #endregion

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            this.transform.SetParent(GameObject.Find("Canvas").GetComponent<Transform>(),false);
            _canvasGroup = GetComponent<CanvasGroup>();

            _playerHealthSlider = GetComponent<Slider>();
        }
        private void Update()
        {

            if (_playerHealthSlider != null) 
            {
                
                _playerHealthSlider.value = _playerTarget.Health;
            }
            if (_playerTarget == null)
            {
                Destroy(this.gameObject);
                return;
            }

        }
        private void LateUpdate()
        {
            if (_targetRenderer != null)
                this._canvasGroup.alpha = _targetRenderer.isVisible ? 1f : 0f;
            if (_targetTrasform != null)
            {
                _targetPosition = _targetTrasform.position;
                _targetPosition.y += _characterControllerHeight;
                this.transform.position = Camera.main.WorldToScreenPoint(_targetPosition)+_screenOffset;

            }

        }

        #endregion


        #region Public Methods
        private void SetTarget(PlayerManager target)
        {
            if (target == null)
                return;
            _playerTarget = target;
            if (_playerNameText != null) 
            {
                

                _playerNameText.text = target.photonView.Owner.NickName;
            }
            _targetTrasform = this._playerTarget.GetComponent<Transform>();
            _targetRenderer = this._playerTarget.GetComponent<Renderer>();
            CharacterController cc = _playerTarget.GetComponent<CharacterController>();

            if (cc != null)
                _characterControllerHeight = cc.height;
        }

        #endregion

    }
}