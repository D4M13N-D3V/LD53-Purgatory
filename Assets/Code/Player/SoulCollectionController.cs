using UnityEngine;

namespace Purgatory.Player
{
    public class SoulCollectionController : MonoBehaviour
    {
        [SerializeField]
        private int _soulCount = 0;
        [SerializeField]
        private float _collectionRadius = 10f;
		

        public void AddSoul(int amount)
        {
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