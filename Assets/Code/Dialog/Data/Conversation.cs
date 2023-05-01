using System;
using System.Collections.Generic;
using UnityEngine;

namespace Purgatory.Dialog
{
	[CreateAssetMenu(fileName = "New Conversation", menuName = "Purgatory/Dialog/Conversation")]
	public class Conversation : ScriptableObject
	{
		public DialogCharacter LeftCharacter, RightCharacter;
		
		public List<DialogEntry> Entries = new();

		[Serializable]
		public class DialogEntry
		{
			public Emotion LeftEmotion, RightEmotion;
			public SpeakerSide Speaker;
			[TextArea] public string Text;
			public int NextEntryDelayMS = 1000;
			public TextEffect[] Effects;

			public enum SpeakerSide
			{
				Left, Right, Both, None
			}
		}
	}
}