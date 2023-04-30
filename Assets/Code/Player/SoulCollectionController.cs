using System.Collections;
using UnityEngine;

namespace Purgatory.Player
{
    public class SoulCollectionController : MonoBehaviour
    {
        [SerializeField]
        private int _soulCount = 0;
        [SerializeField]
        private float _collectionRadius = 10f;

        private void Start()
        {
            StartCoroutine(UpdateHud());
        }


        private IEnumerator UpdateHud()
        {
            HudController.instance.UpdateSoulStats(_soulCount);
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(UpdateHud());
        }

        public void AddSoul(int amount)
        {
            HudController.instance.SoulCollected();
            _soulCount += amount;
        }

        public void RemoveSoul(int amount)
        {
            if (_soulCount > amount)
                _soulCount -= amount;
            else
                _soulCount = 0;
        }

		#if UNITY_EDITOR
		private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(GetComponent<Transform>().position - Vector3.down, Vector3.up, _collectionRadius);
        }
		#endif
    }
}