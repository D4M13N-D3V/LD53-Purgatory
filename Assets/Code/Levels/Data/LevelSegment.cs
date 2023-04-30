using UnityEngine;

namespace Purgatory.Levels.Data
{
	[CreateAssetMenu(fileName = "LevelSegment", menuName = "Purgatory/LevelSegment")]
	public class LevelSegment : MonoBehaviour
	{
		public SegmentType Type;
		public Environment Environment;
		
		#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Gizmos.DrawWireCube(transform.position, new Vector3(30, 1, 40));
		}
#endif
	}
}