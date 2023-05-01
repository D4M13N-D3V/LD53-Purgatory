using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Purgatory.Levels.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Environment = Purgatory.Levels.Data.Environment;
using Random = UnityEngine.Random;

namespace Purgatory.Levels
{
	public class LevelController : MonoBehaviour
	{
		public Action<Environment, Environment> EnvironmentChanged;

		[SerializeField] private Transform segmentParent;
		[SerializeField] private float levelStartOffset = 4f;
		[SerializeField] private float levelScrollSpeed = 5f;
		[SerializeField] private float segmentSize = 20f;

		[SerializeField] private int cinematicSegmentsCount = 2;
		[SerializeField] private int lookAheadCount = 2;
		[SerializeField] private Environment[] environments;
		[SerializeField] private int[] environmentLengths;
		[SerializeField] private EnemyData[] enemies;
		[SerializeField] private List<SegmentWeight> segmentWeights;
		[SerializeField] private LevelSegment[] segments;
		[SerializeField] private LevelSegment fallbackSegment;
		
		public int currentEnvironment = 0;
		private int currentEnvironmentSegment = 0;
		private LevelSegment[] segmentInstances;
		private float scrollDelta = 0f;
		private Vector3[] enemySpawnBuffer = new Vector3[16];
		private LevelSegmentCollection segmentCollection;
		private EnemyCollection enemyCollection;

		private async void Start()
		{
			scrollDelta -= levelStartOffset;
			segmentInstances = new LevelSegment[2 + lookAheadCount];
			currentEnvironment = GameManager.instance.CurrentLevel;
			segmentCollection = new LevelSegmentCollection(segments);
			enemyCollection = new EnemyCollection(enemies.ToArray());
			for (int i = 0; i < 2 + lookAheadCount; i++)
			{
				await CreateNextSegment();
			}
		}

		private async void Update()
		{
			scrollDelta += Time.deltaTime * levelScrollSpeed;

			if (scrollDelta > 1f)
			{
				await CreateNextSegment();
				scrollDelta = 0f;
			}
			
			int activeIdx = 1;
			for (int i = 0; i < segmentInstances.Length; i++)
			{
				if (!segmentInstances[i])
					continue;
				
				float zPos = 0f;
				if (i == 0)
				{
					zPos = (scrollDelta + 1f) * segmentSize;
				}else if (i == activeIdx)
				{
					zPos = scrollDelta * segmentSize;
				}
				else
				{
					zPos = (scrollDelta - (i - activeIdx)) * segmentSize;
				}

				segmentInstances[i].transform.position = new Vector3(0f, 0f, zPos);
			}
		}

		private SegmentType GetNextSegmentType()
		{

			// If this is the last segment in this environment, return a transition segment
			if (currentEnvironmentSegment == environmentLengths[currentEnvironment] - 1)
			{
				if(environments.Length == currentEnvironment)
					return SegmentType.End;
				else
					return SegmentType.Transition;
			}
			
			// If this is one of the first two segments in this environment, return a cinematic segment
			if (currentEnvironmentSegment <= cinematicSegmentsCount - 1)
			{
				return SegmentType.Cinematic;
			}
			
			// Otherwise, return a random weighted segment type
			float totalChance = segmentWeights.Sum(segmentWeight => segmentWeight.Weight);
			float random = UnityEngine.Random.Range(0, totalChance);
			float currentChance = 0;
			foreach (var segmentWeight in segmentWeights)
			{
				currentChance += segmentWeight.Weight;
				if (random <= currentChance)
				{
					return segmentWeight.Type;
				}
			}

			// This shouldn't be reached, but if it is, return a generic segment
			return SegmentType.Generic;
		}
		
		private LevelSegment GetNextSegment()
		{
			var type = GetNextSegmentType();

			var segmentsResult = segmentCollection.GetSegments(environments[currentEnvironment], type);
			if (segmentsResult.IsError || segmentsResult.Value.Count == 0)
				return fallbackSegment;

			var list = segmentsResult.Value;
			return list[Random.Range(0, list.Count - 1)];
		}

		private async Task CreateNextSegment()
		{
			var nextSegment = GetNextSegment();

			var prefab = nextSegment.gameObject;
			float zPos = (2 + lookAheadCount) * segmentSize;
			var instance = Instantiate(prefab, new Vector3(0f, 0f, zPos), Quaternion.identity);
			instance.transform.parent = segmentParent;
			
			// Move everything backwards in the segmentInstances buffer
			if (segmentInstances[0])
				Destroy(segmentInstances[0]);

			for (int i = 1; i < segmentInstances.Length; i++)
			{
				segmentInstances[i - 1] = segmentInstances[i];
			}

			segmentInstances[^1] = instance.GetComponent<LevelSegment>();

			SpawnEnemiesForSegment(segmentInstances[^1]);
			
			currentEnvironmentSegment++;
			if (currentEnvironmentSegment > environmentLengths[currentEnvironment])
			{
				currentEnvironmentSegment = 0;
			}
		}

		private void SpawnEnemiesForSegment(LevelSegment instance)
		{
			if (!instance || instance.MaximumEnemies == 0)
				return;
			
			var enemyList = enemyCollection.GetEnemies(instance.Environment);
			if (enemyList.Count == 0)
				return;
			int count = instance.GetEnemySpawns(ref enemySpawnBuffer);
			for (int i = 0; i < count; i++)
			{
				var enemy = enemyList[Random.Range(0, enemyList.Count - 1)];
				var prefab = enemy.Prefab;
				var pos = enemySpawnBuffer[i];
				
				var enemyInstance = Instantiate(prefab, instance.transform.position + pos, Quaternion.identity, instance.transform);
				
			}
		}
		
		[Serializable]
		private class SegmentWeight
		{
			public SegmentType Type;
			public float Weight;
		}
	}
}