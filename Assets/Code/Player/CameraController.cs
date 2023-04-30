using System.Collections;
using UnityEngine;

namespace Purgatory.Player
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private float _damageShakeIntensity = 0.2f;
        [SerializeField]
        private float _damageShakeDuration = 1.0f;

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
            Vector3 orignalPosition = transform.position;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                float x = orignalPosition.x + Random.Range(-1f, 1f) * magnitude;
                float y = orignalPosition.y + Random.Range(-1f, 1f) * magnitude;

                transform.position = new Vector3(x, y, orignalPosition.z);
                elapsed += Time.deltaTime;
                yield return 0;
            }
            transform.position = orignalPosition;
        }
    }
}