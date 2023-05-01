using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Purgatory.Player
{
    public class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        public CameraController()
        {
            instance = this;

        }

        [SerializeField]
        private float _damageShakeForce = 0.2f;
        [SerializeField]
        private float _damageShakeDuration = 1.0f;
        [SerializeField]
        private CinemachineVirtualCamera _camera;
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        internal void CameraShake(CinemachineImpulseSource impulseSource)
        {
            impulseSource.GenerateImpulseWithForce(_damageShakeForce);
        }
    }
}