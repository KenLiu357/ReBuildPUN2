using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Ken.PhotonTest.Units
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {
        #region Private Field
        private Animator _animator;
        [SerializeField]
        private float directionDampTime = 0.25f;

        private AnimatorStateInfo _stateInfo;
        
        #endregion
        
        
        
        #region MonoBehaviour Callbacks

        private void Start()
        {
            _animator = GetComponent<Animator>();

        }

        
        private void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
                return;

            if (!_animator)
                return;

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            v =   v < 0 ? v = 0:v ;

            _animator.SetFloat("Speed", h*h + v*v);
            _animator.SetFloat("Direction",h,directionDampTime,Time.deltaTime);

            _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (_stateInfo.IsName("Base Layer.Run"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _animator.SetTrigger("Jump");
                }
            }
            

        }

        #endregion
    }
}