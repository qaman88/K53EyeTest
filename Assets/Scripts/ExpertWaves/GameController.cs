using ExpertWaves.UserInput;
using ExpertWaves.Scene.Screening.Distance;
using UnityEngine;
using UnityEngine.UI;
using ExpertWaves.Enum;

namespace ExpertWaves {
	namespace Scene {
		public class GameController : MonoBehaviour {
			#region Public Variables
			public Image imageOptotype;
			public Text status;
			public Camera sceneCamera;
			public DistanceScreening distanceEyeTest;
			public InputController inputController;
			#endregion

			#region Public Functions
			void Start() {
				this.status.text = "K53 Eye Test";
				inputController.RegisterKeyPressListener(onKeyPress);
				inputController.RegisterSwipeListener(onSwipe);
			}

			private void OnDestroy() {
				inputController.UnregisterKeyPressListener(onKeyPress);
				inputController.UnregisterSwipeListener(onSwipe);
			}
			#endregion

			#region Private Functions
			private void onKeyPress(KeyCode key) {
				Debug.Log($"Key Press Event, Key: {key.ToString()}");

				switch (key) {
					case KeyCode.W:
						Debug.Log("$$$ Keypress W");
						this.NextLevel(IDirection.UP);
						break;

					case KeyCode.A:
						Debug.Log("$$$ Keypress A");
						this.NextLevel(IDirection.LEFT);
						break;

					case KeyCode.D:
						Debug.Log("$$$ Keypress D");
						this.NextLevel(IDirection.UP);
						break;

					case KeyCode.S:
						Debug.Log("$$$ Keypress S");
						this.NextLevel(IDirection.DOWN);
						break;

					default:
						break;
				}
			}

			private void onSwipe(IDirection type) {
				Debug.Log($"### Swipe Event from InputController. Swipe: {type.ToString()}");
				this.NextLevel(type);
			}
			private void NextLevel(IDirection direction) {
				if (distanceEyeTest.getGameOver() == false && direction == distanceEyeTest.Direction) {
					// game manager move to next level
					distanceEyeTest.NextLevel();

					// change angless
					this.imageOptotype.transform.localEulerAngles = new Vector3(0f, 0f, distanceEyeTest.Angle);

					// change size
					this.imageOptotype.transform.localScale = new Vector3(distanceEyeTest.OptotypeScale, distanceEyeTest.OptotypeScale, 1.0f);
					;

					// score 
					this.status.text = $"Score: {distanceEyeTest.Score} %";
				}
				else {
					// game over on incorrect answer or end of levels
					distanceEyeTest.setGameOver(true);
					// status text
					this.status.text = $"Score: {distanceEyeTest.Score} %";
				}
			}
			#endregion
		}
	}
}