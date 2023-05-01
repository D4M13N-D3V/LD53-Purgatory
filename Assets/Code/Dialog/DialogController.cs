﻿using Purgatory.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using SpeakerSide = Purgatory.Dialog.Conversation.DialogEntry.SpeakerSide;

namespace Purgatory.Dialog
{
	public class DialogController : MonoBehaviour
	{
		public Conversation TEMP_CONVO;
		
		[Header("Visuals")]
		[SerializeField] private Color activeCharColor = Color.white;
		[SerializeField] private Color inactiveCharColor = new (0.8f, 0.8f, 0.8f, 1f);
		[SerializeField] private int textRevealSpeedMs = 50;
		[SerializeField] private List<TextBackgroundSprite> backgroundSprites = new();

		[Header("References")]
		[SerializeField] private Image leftCharacter;
		[SerializeField] private Image rightCharacter;
		[SerializeField] private Image textBackground;
		[SerializeField] private TextMeshProUGUI text;
		[SerializeField] private List<TextEffectReference> textEffects = new();

		[SerializeField] private bool loadScene = false;
		[SerializeField] private string sceneToLoad;

		private async void Start()
		{
			GameManager.instance.SetGameState(EnumGameState.DIALOGUE);
			await BeginConversation(TEMP_CONVO);
		}


		public async Task BeginConversation(Conversation conversation)
		{
			int idx = 0;
			while (idx < conversation.Entries.Count)
			{
				await DoEntry(conversation, conversation.Entries[idx]);
				idx++;
			}
			SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
		}

		public async Task DoEntry(Conversation conversation, Conversation.DialogEntry entry)
		{
			textBackground.sprite = backgroundSprites.First(t => t.Side == entry.Speaker).Sprite;

			var leftColor = entry.Speaker == SpeakerSide.Left || entry.Speaker == SpeakerSide.Both ? activeCharColor : inactiveCharColor;
			var rightColor = entry.Speaker == SpeakerSide.Right || entry.Speaker == SpeakerSide.Both ? activeCharColor : inactiveCharColor;
			leftCharacter.color = leftColor;
			rightCharacter.color = rightColor;
			
			leftCharacter.sprite = conversation.LeftCharacter.Emotions.First(t => t.Emotion == entry.LeftEmotion).Sprite;
			rightCharacter.sprite = conversation.RightCharacter.Emotions.First(t => t.Emotion == entry.RightEmotion).Sprite;

			// Set the text to our filtered text
			text.text = entry.Text;

			foreach (var effectRef in textEffects)
			{
				effectRef.Component.enabled = entry.Effects?.Any(t => t == effectRef.Effect) ?? false;
			}

			// Reveal the text
			await RevealText();
			await Task.Delay(entry.NextEntryDelayMS);
		}

		private async Task RevealText()
		{
			text.ForceMeshUpdate();

			var textInfo = text.textInfo;
			
			int totalVisibleCharacters = text.textInfo.characterCount;
			int counter = 0;

			while (true)
			{
				var visibleCount = counter % (totalVisibleCharacters + 1);
				
				for (int i = 0; i < totalVisibleCharacters; i++)
				{
					var cInfo = textInfo.characterInfo[i];

					for (int j = 0; j < 4; j++)
					{
						textInfo.meshInfo[cInfo.materialReferenceIndex].colors32[cInfo.vertexIndex + j] = visibleCount >= i ? new Color(1f,1f,1f,1f) : new Color(1f,1f,1f,0f);
					}
				}

				// Manually set the first char to be visible. I don't know why I need to do this. I'm tired.
				textInfo.meshInfo[0].colors32[0] = new Color(1f,1f,1f,1f);
				textInfo.meshInfo[0].colors32[1] = new Color(1f,1f,1f,1f);
				textInfo.meshInfo[0].colors32[2] = new Color(1f,1f,1f,1f);
				textInfo.meshInfo[0].colors32[3] = new Color(1f,1f,1f,1f);
				
				textInfo.meshInfo[0].mesh.vertices = textInfo.meshInfo[0].vertices;
				text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
				
				if (visibleCount >= totalVisibleCharacters)
					break;
				
				counter += 1;
				await Task.Delay(textRevealSpeedMs);
			}
		}

		[Serializable]
		private class TextBackgroundSprite
		{
			public SpeakerSide Side;
			public Sprite Sprite;
		}

		[Serializable]
		private class TextEffectReference
		{
			public TextEffect Effect;
			public MonoBehaviour Component;
		}
	}
}