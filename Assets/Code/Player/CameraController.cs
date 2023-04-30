using System.Collections;
using UnityEngine;
using Cinemachine;

namespace Purgatory.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float _damageShakeIntensity = 0.2f;
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
        public void DamageShake(int amount)
        {
            StartCoroutine(Shake(_damageShakeDuration, _damageShakeIntensity));
        }

        public IEnumerator Shake(float duration, float magnitude)
        {
            Noise(1, magnitude);
            yield return new WaitForSeconds(duration);
            Noise(0, 0);
        }


        public void Noise(float amplitudeGain, float frequencyGain)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitudeGain;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequencyGain;
        }

    }
}