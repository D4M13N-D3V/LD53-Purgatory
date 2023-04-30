using System.Linq;
using UnityEngine;

namespace Purgatory.Levels.Data
{
	public class LevelSegment : MonoBehaviour
	{
		public SegmentType Type;
		public Environment Environment;

		public Vector3[] EnemySpawnPositions = new Vector3[0];
		public int MinimumEnemies = 0;
		public int MaximumEnemies = 0;

		public int GetEnemySpawns(ref Vector3[] positionBuffer)
		{
			int count = Random.Range(MinimumEnemies, MaximumEnemies);
			// Add count enemy positions to positionBuffer, do not allow duplicates
			for (int i = 0; i < count; i++)
			{
				var pos = EnemySpawnPositions[Random.Range(0, EnemySpawnPositions.Length)];
				if (positionBuffer.Any(t => t == pos))
				{
					i--;
					continue;
				}

				positionBuffer[i] = pos;
			}

			return count;
		}

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Vector3 waterPlaneSize = new(30f, 0.1f, 40f);
			
			// Check if we're in a prefab isolation mode, if so adjust waterPlaneSize
			if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
				waterPlaneSize = new(60f, 0.1f, 70f);


			for (int i = 0; i < EnemySpawnPositions.Length; i++)
			{
				var pos = transform.position + EnemySpawnPositions[i];
				Gizmos.DrawWireSphere(pos, 0.5f);
				UnityEditor.Handles.Label(pos, i.ToString());
			}
			
			Gizmos.DrawWireCube(transform.position, new Vector3(30, 1, 40));
			var c = Gizmos.color;
			Gizmos.color = new Color(0f, 0f, 1f, 0.33f);
			Gizmos.DrawCube(transform.position, waterPlaneSize);
			Gizmos.color = c;
			UnityEditor.Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(new Vector3(0, 0, 1), Vector3.up), 10f, EventType.Repaint);
		}
#endif
	}
}