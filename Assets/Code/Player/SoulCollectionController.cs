using System.Collections;
using UnityEngine;

namespace Purgatory.Player
{
    public class SoulCollectionController : MonoBehaviour
    {
        public static SoulCollectionController instance;

        public SoulCollectionController()
        {
            if (instance == null)
                instance = this;
        }

        [SerializeField]
        public int Souls = 0;
        [SerializeField]
        public float CollectionRadius = 10f;

        public void AddSoul(int amount)
        {
            HudController.instance.SoulCollected();
            Souls += amount;
        }

        public void RemoveSoul(int amount)
        {
            if (Souls > amount)
                Souls -= amount;
            else
                Souls = 0;
        }

		#if UNITY_EDITOR
		private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(GetComponent<Transform>().position - Vector3.down, Vector3.up, CollectionRadius);
        }
		#endif
    }
}