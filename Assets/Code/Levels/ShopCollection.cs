using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Environment = Purgatory.Levels.Data.Environment;

namespace Purgatory.Levels
{
	[Serializable]
	public class ShopData
	{
		public Environment Environment;
		public GameObject Prefab;
	}

	public class ShopCollection
	{
		private Dictionary<Environment, List<ShopData>> shopLookup;
		public ShopCollection(ShopData[] enemies)
		{
			shopLookup = new Dictionary<Environment, List<ShopData>>();
			foreach (var enemy in enemies)
			{
				if (!shopLookup.ContainsKey(enemy.Environment))
					shopLookup.Add(enemy.Environment, new List<ShopData>() { enemy });

				shopLookup[enemy.Environment].Add(enemy);
			}
		}

		public  ShopData GetShop(Environment environment)
		{
			if (!shopLookup.ContainsKey(environment))
				return shopLookup[environment].FirstOrDefault();

			return shopLookup[environment].First();
		}
	}
}