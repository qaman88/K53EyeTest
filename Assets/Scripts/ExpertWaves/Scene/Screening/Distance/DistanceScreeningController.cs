using ExpertWaves.Scene.Screening.Distance;
using UnityEngine;
using UnityEngine.UI;
using ExpertWaves.Enum;
using ExpertWaves.Log;
using System.Reflection;
using ExpertWaves.UserInput.Touch;
using ExpertWaves.UserInput.Key;
using ExpertWaves.Audio;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Vibration;

namespace ExpertWaves {
	namespace Scene {
		public class DistanceScreeningController :MonoBehaviour {
			#region Public Variables
			public Image imageOptotype;
			public Text status;
			public PageController pageController;
			public SceneController sceneController;
			public Engine engine;
			public AudioController audioController;
			#endregion

			#region Private Variables
			private LogController log;
			private KeyInputController keyInput;
			private TouchInputController touchInput;
			private VibrationController vibrationController;
			#endregion

			#region Public Functions
			private void Awake() {
				Configure();
			}

			private void Start() {
				status.text = "K53 Eye Test";
				engine = new Engine();
				touchInput.RaiseTouchInputEvent += onSwipe;
				keyInput.SubscribeKeyPressListener(onKeyPress);

				// log
				log.LogDebug(
					message: $"Game states. Gameover: {engine.Gameover}. Engine direction: {engine.Direction}.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
			}

			private void OnDestroy() {
				touchInput.RaiseTouchInputEvent -= onSwipe;
				keyInput.UnsubscribeKeyPressListener(onKeyPress);
			}
			#endregion

			#region Private Functions

			private void Configure() {
				// ensure log controller is defined
				if (!log) {
					log = LogController.instance;
				}
				// ensure key input controller is defined
				if (!keyInput) {
					keyInput = KeyInputController.instance;
				}

				// ensure touch input controller is defined
				if (!touchInput) {
					touchInput = TouchInputController.instance;
				}

				// ensure vibration controller is defined
				if (!vibrationController) {
					vibrationController = VibrationController.instance;
				}

				// ensure scene controller is defined
				if (!sceneController) {
					sceneController = SceneController.instance;
					sceneController.Log = log;

					if (!pageController) {
						pageController.Log = log;
						sceneController.pageController = pageController;
					}
				}

				// ensure audio controller is defined
				if (!audioController) {
					audioController = AudioController.instance;
				}
			}

			private void onKeyPress(KeyCode key) {
				audioController.PlayEffect(IEffectType.Success);
				// log
				log.LogDebug(
					message: $"Key press {key}",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);

				switch (key) {
					case KeyCode.W:
						NextLevel(IDirection.Up);
						break;

					case KeyCode.A:
						NextLevel(IDirection.Left);
						break;

					case KeyCode.D:
						NextLevel(IDirection.Right);
						break;

					case KeyCode.S:
						NextLevel(IDirection.Down);
						break;

					default:
						break;
				}
			}

			private void onSwipe(object sender, TouchInputEvent e) {
				IDirection type = e.Direction;
				NextLevel(type);
			}

			private void NextLevel(IDirection direction) {
				if (engine.Gameover == false && direction == engine.Direction) {
					audioController.PlayEffect(IEffectType.Success);
					// game manager move to next level
					engine.NextLevel();

					// change angles
					imageOptotype.transform.localEulerAngles = new Vector3(0f, 0f, engine.Angle);

					// change size
					imageOptotype.transform.localScale = new Vector3(engine.OptotypeScale, engine.OptotypeScale, 1.0f);

					// score 
					status.text = $"Score: {engine.Score} %";
				}
				else {
					audioController.PlayEffect(IEffectType.Failure);
					// game over on incorrect answer or end of levels
					engine.Gameover = true;

					// status text
					status.text = $"Game Over Score: {engine.Score} %";
				}

				// log
				log.LogDebug(
					message: $"Move to next level scope. Gameover: {engine.Gameover}. Engine Direction: {engine.Direction}. Swipe: {direction}",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
			}
			#endregion
		}
	}
}