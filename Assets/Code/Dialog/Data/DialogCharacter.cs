using System;
using System.Collections.Generic;
using UnityEngine;

namespace Purgatory.Dialog
{
	[CreateAssetMenu(fileName = "New Dialog Character", menuName = "Purgatory/Dialog/Character")]
	public class DialogCharacter : ScriptableObject
	{
		public List<DialogEmotion> Emotions = new();
		public List<VoiceLine> VoiceLines = new();

		[Serializable]
		public class DialogEmotion
		{
			public Emotion Emotion;
			public Sprite Sprite;
		}

		[Serializable]
		public class VoiceLine
		{
			public int MaximumWords = 3;
			public AudioClip Clip;
		}
	}
}