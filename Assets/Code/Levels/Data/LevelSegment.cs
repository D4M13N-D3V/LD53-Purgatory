using System.Threading.Tasks;
using OperationResult;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static OperationResult.Helpers;

namespace Purgatory.Levels.Data
{
	[CreateAssetMenu(fileName = "LevelSegment", menuName = "Purgatory/LevelSegment")]
	public class LevelSegment : ScriptableObject
	{
		public SegmentType Type;
		public Environment Environment;
		public AssetReferenceGameObject SegmentPrefab;

		private AsyncOperationHandle<GameObject> handle;

		public async Task<Result<GameObject>> Load()
		{
			if (!handle.IsValid())
			{
				handle = SegmentPrefab.LoadAssetAsync();
			}

			await handle.Task;

			if (!handle.IsDone || handle.Status != AsyncOperationStatus.Succeeded)
				return Error();
			
			return Ok(handle.Result);
		}

		public void Unload()
		{
			if (!handle.IsValid())
				return;
			Addressables.Release(handle);
			handle = default;
		}
	}
}