
using ExpertWaves.Graphic.Enum;
using System.Collections;
using TMPro;
using UnityEngine;

namespace ExpertWaves {
	namespace Graphic {
		public class ExpertTextStyle : MonoBehaviour {
			#region Public Variables
			public float animationStepMilliSecond = 500.0f;
			public delegate IEnumerator Transformer(int indexFirst, int indexLast);
			#endregion

			#region Private Variables
			private TMP_Text text;
			private TMP_TextInfo textInfo;
			#endregion

			#region Public Variables Properties
			public TMP_Text Text {
				get => text;
				set => text = value;
			}

			public float FontSize {
				get => Text.fontSize;
				set => Text.fontSize = value;
			}

			public Color FontColor {
				get => Text.color;
				set => Text.color = value;
			}

			public TMP_TextInfo TextInfo {
				get => textInfo;
				set => textInfo = value;
			}
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure(IExpertText.Title);
			}

			private void Start() {
			}

			private void Update() {
			}

			private void OnDestroy() {
			}
			#endregion

			#region Public Functions
			public void Configure(IExpertText type = IExpertText.Normal) {
				Text = GetComponentInChildren<TextMeshProUGUI>();
				Text.ForceMeshUpdate();
				TextInfo = Text.textInfo;

				switch (type) {
					case IExpertText.Title:
						StartCoroutine(TransformText(RandomVertexColor));
						break;
					case IExpertText.Heading:
						break;
					case IExpertText.Normal:
						break;
					default:
						break;
				}
			}
			#endregion

			#region Private Functions
			private IEnumerator TransformText(Transformer transformer) {
				FontSize = (int) IExpertFontSize.XLarge;

				int totalCharacters = textInfo.characterCount;

				while (true) {
					StartCoroutine(transformer.Invoke(0, totalCharacters - 1));
					yield return new WaitForSeconds(animationStepMilliSecond / 1000);
				}
			}

			private IEnumerator RandomVertexColor(int indexCharacterFirst, int indexCharacterLast) {
				for (int currentCharacter = indexCharacterFirst ; currentCharacter <= indexCharacterLast ; currentCharacter++) {
					// Get the index of the material used by the current character.
					int materialIndex = textInfo.characterInfo[currentCharacter].materialReferenceIndex;

					// Get the index of the first vertex used by this text element.
					int vertexIndex = textInfo.characterInfo[currentCharacter].vertexIndex;

					// Only change the vertex color if the text element is visible.
					if (textInfo.characterInfo[currentCharacter].isVisible) {
						Color randomColor = new Color32(
							(byte) Random.Range(0, 255),
							(byte) Random.Range(0, 255),
							(byte) Random.Range(0, 255),
							255
						);

						// Update the vertex colors of the mesh used by this text element (character or sprite).
						textInfo.meshInfo[materialIndex].colors32[vertexIndex] = randomColor;
						textInfo.meshInfo[materialIndex].colors32[vertexIndex + 1] = randomColor;
						textInfo.meshInfo[materialIndex].colors32[vertexIndex + 2] = randomColor;
						textInfo.meshInfo[materialIndex].colors32[vertexIndex + 3] = randomColor;
					}
				}

				// force mesh update
				text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

				yield return null;
			}
			#endregion
		}
	}
}
