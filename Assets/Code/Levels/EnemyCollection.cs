using System;
using System.Collections.Generic;
using UnityEngine;
using Environment = Purgatory.Levels.Data.Environment;

namespace Purgatory.Levels
{
	[Serializable]
	public class EnemyData
	{
		public Environment Environment;
		public GameObject Prefab;
	}
	
	public class EnemyCollection
	{
		private Dictionary<Environment, List<EnemyData>> enemyLookup;
		public EnemyCollection(EnemyData[] enemies)
		{
			enemyLookup = new Dictionary<Environment, List<EnemyData>>();
			foreach (var enemy in enemies)
			{
				if (!enemyLookup.ContainsKey(enemy.Environment))
					enemyLookup.Add(enemy.Environment, new List<EnemyData>() { enemy });
				
				enemyLookup[enemy.Environment].Add(enemy);
			}
		}
		
		public List<EnemyData> GetEnemies(Environment environment)
		{
			if (!enemyLookup.ContainsKey(environment))
				return new List<EnemyData>();
			
			return enemyLookup[environment];
		}
	}
}