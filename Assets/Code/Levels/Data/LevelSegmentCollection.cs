using System.Collections.Generic;
using OperationResult;
using static OperationResult.Helpers;

namespace Purgatory.Levels.Data
{
	/// <summary>
	/// Container for a collection of level segments, allowing querying based on segment type and environment.
	/// </summary>
	public class LevelSegmentCollection
	{
		public Dictionary<Environment, Dictionary<SegmentType, List<LevelSegment>>> Segments { get; } = new();

		public LevelSegmentCollection(LevelSegment[] segments)
		{
			foreach(var segment in segments)
			{
				if(!Segments.TryGetValue(segment.Environment, out var bySegmentType))
					bySegmentType = Segments[segment.Environment] = new Dictionary<SegmentType, List<LevelSegment>>();
				if(!bySegmentType.TryGetValue(segment.Type, out var byEnvironment))
					byEnvironment = bySegmentType[segment.Type] = new List<LevelSegment>();
				byEnvironment.Add(segment);
			}
		}

		public Result<List<LevelSegment>> GetSegments(Environment environment, SegmentType type)
		{
			if (!Segments.TryGetValue(environment, out var bySegmentType))
				return Error();
			if (!bySegmentType.TryGetValue(type, out var segmentList))
				return Error();
			return segmentList;
		}
	}
}