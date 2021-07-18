using ExpertWaves.Audio;
using ExpertWaves.Log;
using ExpertWaves.UserInput.Key;
using ExpertWaves.UserInput.Touch;
using ExpertWaves.Vibration;
using System.Reflection;
using UnityEngine;

namespace ExpertWaves {
	namespace Scene {
		namespace Screening {
			namespace Color {
				public class ColorScreeningController :MonoBehaviour {
					#region Public Variables
					public PageController pageController;
					public SceneController sceneController;
					#endregion

					#region Private Variables
					private LogController log;
					private KeyInputController keyInput;
					private TouchInputController touchInput;
					private AudioController audioController;
					private VibrationController vibrationController;
					#endregion

					#region Variables Properties
					#endregion

					#region Unity Functions
					private void Awake() {
						Configure();
					}

					private void Start() {
						// user inputs event subscription
						touchInput.RaiseTouchInputEvent += OnSwipe;
						keyInput.SubscribeKeyPressListener(OnKeyPress);

						// scene manager onSceneLoaded subscription
						sceneController.SubscribeOnSceneLoaded(OnSceneLoaded);

						// log
						log.LogDebug(
							message: $"On Start.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}

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

					private void OnDestroy() {
						// user inputs event unsubscription
						touchInput.RaiseTouchInputEvent -= OnSwipe;
						keyInput.UnsubscribeKeyPressListener(OnKeyPress);

						// scene manager onSceneLoaded unsubscription
						sceneController.UnsubscribeOnSceneLoaded(OnSceneLoaded);
					}
					#endregion

					#region Public Functions
					#endregion

					#region Private Functions		
					private void OnSwipe(object sender, TouchInputEvent e) {

					}

					private void OnKeyPress(KeyCode key) {
						// log
						log.LogDebug(
							message: $"Key press {key}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						switch (key) {
							case KeyCode.W:
								break;

							case KeyCode.A:
								break;

							case KeyCode.D:
								break;

							case KeyCode.S:
								break;

							default:
								break;
						}
					}

					public void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene) {
						log.LogInfo(
							message: $"Scene loaded event, scene: {scene.name}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}
					#endregion
				}
			}
		}
	}
}