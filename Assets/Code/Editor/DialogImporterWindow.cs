using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Purgatory.Dialog;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
	public class DialogImporterWindow : EditorWindow
	{
		[MenuItem("Tools/Dialog/Import Dialog")]
		static void Init()
		{
			GetWindow<DialogImporterWindow>().Show();
		}

		private DialogCharacter leftCharacter, rightCharacter;
		private string dialogName;
		private string dialogString;
		private string folder;
		private void OnGUI()
		{
			leftCharacter = EditorGUILayout.ObjectField("Left Character", leftCharacter, typeof(DialogCharacter), false) as DialogCharacter;
			rightCharacter = EditorGUILayout.ObjectField("Right Character", rightCharacter, typeof(DialogCharacter), false) as DialogCharacter;
			dialogName = EditorGUILayout.TextField("Dialog Name", dialogName);
			dialogString = EditorGUILayout.TextArea(dialogString);
			folder = EditorGUILayout.TextField("Folder", folder);
			if (GUILayout.Button("Import"))
			{
				var entries = GetEntries(dialogString);
				var conversation = CreateInstance<Conversation>();
				conversation.Entries = entries;
				conversation.LeftCharacter = leftCharacter;
				conversation.RightCharacter = rightCharacter;
				AssetDatabase.CreateAsset(conversation, $"Assets/Gameplay/Dialog/Conversations/{folder}/{dialogName}.asset");
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		private List<Conversation.DialogEntry> GetEntries(string dialog)
		{
			var entries = new List<Conversation.DialogEntry>();
			string[] lines = dialog.Split('\n');
			var regex = new Regex(@"\{(\d+)\}\s\(([^,]+),\s*([^)]+)\)\s\[(\w+)\]\s:\s""([^""]+)""", RegexOptions.Compiled);
			
			foreach (string line in lines)
			{
				var match = regex.Match(line);
				if (!match.Success)
					continue;
				
				var entry = new Conversation.DialogEntry();
				entry.Text = match.Groups[5].Value;
				entry.Speaker = match.Groups[4].Value switch
				{
					"Rokuro" => Conversation.DialogEntry.SpeakerSide.Left,
					"Yuki" => Conversation.DialogEntry.SpeakerSide.Right,
					_ => throw new ArgumentOutOfRangeException()
				};

				entry.LeftEmotion = Enum.Parse<Emotion>(match.Groups[2].Value);
				entry.RightEmotion = Enum.Parse<Emotion>(match.Groups[3].Value);
				entries.Add(entry);
			}

			return entries;
		}
	}
}