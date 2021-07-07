using ExpertWaves.UserInput;
using UnityEngine;
using ExpertWaves.Enum;
using ExpertWaves.Log;
using System.Reflection;
using ExpertWaves.Scene.Enum;
using ExpertWaves.Page.Enum;

namespace ExpertWaves {
	namespace Scene {
		public class GameController : MonoBehaviour {
			#region Public Variables
			public static GameController instance;
			public LogController log;
			public InputController inputController;
			public PageController pageController;
			public SceneController sceneController;
			#endregion

			#region Public Functions
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
				log.LogInfo(
					message: "GameController is Awake.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
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

				// subscribe to controllers events
				inputController.SubscribeKeyPressListener(onKeyPress);
				inputController.SubscribeSwipeListener(onSwipe);
				sceneController.SubscribeOnSceneLoaded(OnSceneLoaded);
			}
			#endregion

			#region Callback Functions
			private void onKeyPress(KeyCode key) {
				log.LogInfo(
					message: $"Key Press Event, Key: {key}",
					classType: MethodBase.GetCurrentMethod().Name,
					classMethod: GetType().Name
				);

				switch (key) {
					case KeyCode.W:
						pageController.SwichPage(IPageType.Loading);
						break;

					case KeyCode.A:
						pageController.SwichPage(IPageType.About);
						break;

					case KeyCode.D:
						pageController.SwichPage(IPageType.Menu);
						break;

					case KeyCode.S:
						pageController.SwichPage(IPageType.Privacy);
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