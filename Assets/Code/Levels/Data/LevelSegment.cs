using UnityEngine;

namespace Purgatory.Levels.Data
{
	[CreateAssetMenu(fileName = "LevelSegment", menuName = "Purgatory/LevelSegment")]
	public class LevelSegment : MonoBehaviour
	{
		public SegmentType Type;
		public Environment Environment;
	}
}