
using ExpertWaves.Graphic.Enum;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExpertWaves {
	namespace Graphic {
		public enum IExpertPanel {
			UpperPanel,
			MiddlePanel,
			LowerPanel,
			BackGroundPanel,
			FrontGroundPanel
		}

		public class ExpertPanel : MonoBehaviour {
			#region Public Variables
			#endregion

			#region Private Variables
			RectTransform shape;
			CanvasRenderer canvas;
			#endregion

			#region Public Variables Properties
			public IExpertPanel type;
			public float animationStepMilliSecond = 500.0f;
			public Texture texture;
			public Material material;

			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
			}

			private void Start() {
			}

			private void Update() {
			}

			private void OnDestroy() {
			}
			#endregion

			#region Public Functions
			#endregion

			#region Private Functions
			private void Configure() {
				shape = GetComponent<RectTransform>();
				canvas = GetComponent<CanvasRenderer>();

				ConfigureShape();
				ConfigureCanvas();
			}

			void ConfigureShape() {
			}

			void ConfigureCanvas() {
				switch (type) {
					case IExpertPanel.UpperPanel:
						canvas.SetColor(Color.cyan);
						break;
					case IExpertPanel.MiddlePanel:
						canvas.SetColor(Color.gray);
						break;
					case IExpertPanel.LowerPanel:
						canvas.SetMaterial(material, texture);
						break;
					case IExpertPanel.BackGroundPanel:
						StartCoroutine(AsyncRandomColor());
						break;
					case IExpertPanel.FrontGroundPanel:
						canvas.SetColor(new Color(0.2f, 0.5f, 0.6f, 0.25f));
						break;
					default:
						break;
				};
			}

			IEnumerator AsyncRandomColor() {
				while (true) {
					canvas.SetColor(new Color(
						Random.Range(0, 255) / 255.0f,
						Random.Range(0, 255) / 255.0f,
						Random.Range(0, 255) / 255.0f,
						Random.Range(200, 255) / 255.0f)
					);
					yield return new WaitForSeconds(animationStepMilliSecond / 1000);
				}
			}
		}
		#endregion
	}
}
