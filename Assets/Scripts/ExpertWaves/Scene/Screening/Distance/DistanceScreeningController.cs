using ExpertWaves.UserInput;
using ExpertWaves.Scene.Screening.Distance;
using UnityEngine;
using UnityEngine.UI;
using ExpertWaves.Enum;
using ExpertWaves.Page.Enum;
using ExpertWaves.Scene.Enum;
using ExpertWaves.Log;

namespace ExpertWaves {
	namespace Scene {
		public class DistanceScreeningController : MonoBehaviour {
			#region Public Variables
			public Image imageOptotype;
			public Text status;
			public DistanceScreening distanceScene;
			public InputController inputController;
			public LogController log;
			public SceneController sceneController;
			#endregion

			#region Public Functions
			void Start() {
				this.status.text = "K53EyeTest.Distance";
				inputController.SubscribeKeyPressListener(onKeyPress);
				inputController.SubscribeSwipeListener(onSwipe);
			}

			private void OnDestroy() {
				inputController.UnsubscribeKeyPressListener(onKeyPress);
				inputController.UnsubscribeSwipeListener(onSwipe);
			}
			#endregion

			#region Private Functions
			private void onKeyPress(KeyCode key) {
				Debug.Log($"Key Press Event, Key: {key}");

				switch (key) {
					case KeyCode.W:
						this.NextLevel(IDirection.Up);
						break;

					case KeyCode.A:
						this.NextLevel(IDirection.Left);
						break;

					case KeyCode.D:
						this.NextLevel(IDirection.Right);
						break;

					case KeyCode.S:
						this.NextLevel(IDirection.Down);
						break;

					case KeyCode.Z:
						sceneController.LoadSceneOnPage(ISceneType.Menu, IPageType.Loading);
						break;

					default:
						break;
				}
			}

			private void onSwipe(IDirection type) {
				Debug.Log($"### Swipe Event from InputController. Swipe: {type}");
				this.NextLevel(type);
			}

			private void NextLevel(IDirection direction) {
				if (distanceScene.getGameOver() == false && direction == distanceScene.Direction) {
					// game manager move to next level
					distanceScene.NextLevel();

					// change angles
					this.imageOptotype.transform.localEulerAngles = new Vector3(0f, 0f, distanceScene.Angle);

					// change size
					this.imageOptotype.transform.localScale = new Vector3(distanceScene.OptotypeScale, distanceScene.OptotypeScale, 1.0f);
					;

					// score 
					this.status.text = $"Score: {distanceScene.Score} %";
				}
				else {
					// game over on incorrect answer or end of levels
					distanceScene.setGameOver(true);
					// status text
					this.status.text = $"Score: {distanceScene.Score} %";
				}
			}
			#endregion
		}
	}
}