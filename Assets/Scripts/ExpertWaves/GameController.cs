using ExpertWaves.UserInput;
using UnityEngine;
using ExpertWaves.Enum;
using ExpertWaves.Log;
using System.Reflection;
using ExpertWaves.Scene.Enum;
using ExpertWaves.Page.Enum;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Audio;
using ExpertWaves.Vibration;

namespace ExpertWaves {
	namespace Scene {
		public class GameController : MonoBehaviour {
			#region Public Variables
			public static GameController instance;
			public LogController log;
			public InputController inputController;
			public PageController pageController;
			public SceneController sceneController;
			public AudioController audioController;
			public VibrationController vibrationController; 
			#endregion

			#region Public Functions
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
				vibrationController.Vibrate();
				vibrationController.Vibrate();
				vibrationController.Vibrate();
				vibrationController.Vibrate();
				vibrationController.Vibrate();
			}

			private void OnDestroy() {
				inputController.UnsubscribeKeyPressListener(onKeyPress);
				inputController.UnsubscribeSwipeListener(onSwipe);
				sceneController.UnsubscribeOnSceneLoaded(OnSceneLoaded);
			}
			#endregion

			#region Private Functions		
			private void Configure() {
				// ensure instance is defined
				if (!instance) {
					instance = this;
					DontDestroyOnLoad(gameObject);
				}
				else {
					Destroy(gameObject);
				}

				// ensure log controller is defined
				if (!log) {
					log = LogController.instance;
				}

				// ensure page controller is defined
				if (!pageController) {
					pageController = PageController.instance;
				}

				// ensure input controller is defined
				if (!inputController) {
					inputController = InputController.instance;
				}

				// ensure scene controller is defined
				if (!sceneController) {
					sceneController = SceneController.instance;
				}

				// ensure scene controller is defined
				if (!audioController) {
					audioController = AudioController.instance;
				}

				// ensure scene controller is defined
				if (!vibrationController) {
					vibrationController = VibrationController.instance;
				}

				// subscribe to controllers events
				inputController.SubscribeKeyPressListener(onKeyPress);
				inputController.SubscribeSwipeListener(onSwipe);
				sceneController.SubscribeOnSceneLoaded(OnSceneLoaded);
			}
			#endregion

			#region Callback Functions
			private void onKeyPress(KeyCode key) {
				switch (key) {
					case KeyCode.W:
						audioController.PlaySound(ISoundType.Background1);
						audioController.PlayEffect(IEffectType.Warinig);
						audioController.PlayVoice(IVoiceType.Warning1);
						break;

					case KeyCode.A:
						audioController.PlayEffect(IEffectType.Swipe);
						break;

					case KeyCode.D:
						audioController.PlayVoice(IVoiceType.Success1);
						break;

					case KeyCode.S:
						pageController.SwichPage(IPageType.Privacy);
						vibrationController.Vibrate();
						break;

					case KeyCode.Z:
						sceneController.LoadSceneOnPage(ISceneType.Menu, IPageType.Loading);
						break;

					case KeyCode.X:
						sceneController.LoadSceneOnPage(ISceneType.DistanceScreening, IPageType.Loading);
						break;

					default:
						break;
				}
			}

			private void onSwipe(IDirection swipe) {
				audioController.PlayEffect(IEffectType.Swipe);
				vibrationController.Vibrate();
				log.LogInfo(
					message: $"Touch event, swipe {swipe}.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
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