using ExpertWaves.Enum;
using ExpertWaves.Log;
using ExpertWaves.Utility;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ExpertWaves {
	namespace Scene {
		public delegate void CallbackOnSceneLoaded(UnityEngine.SceneManagement.Scene scene);

		public class SceneController : MonoBehaviour {
			#region Public Variables
			public static SceneController instance;
			public PageController pageController;
			public LogController log;
			public CallbackOnSceneLoaded callbackOnSceneLoaded;

			#endregion

			#region Private Variables
			private bool isLoadingScene = Constant.Enable;
			UnityEngine.SceneManagement.Scene currentScene;
			#endregion

			#region Variables Properties
			private PageController PageManager {
				get {
					if (pageController == null) {
						pageController = PageController.instance;
						log.LogWarn(
							message: "PageController instance is not define.",
							classType: "SceneController",
							classMethod: "PageManager.get"
						);
					}
					return pageController;
				}
			}

			public bool IsLoadingScene {
				get => this.isLoadingScene;
				set => this.isLoadingScene = value;
			}
			#endregion

			#region Unity Functions	
			private void Awake() {
				Configure();
			}
			private void OnDisable() {
			}
			#endregion

			#region Public Functions
			public void Configure() {
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
			}

			public void LoadSceneOnPage(ISceneType scene, IPageType page) {
				log.LogInfo(
					message: $"Page type is {page}, Scene type is {scene}.",
					classType: "SceneController",
					classMethod: "LoadSceneOnPage"
				);

				// check if page is valid
				if (page == IPageType.None) {
					log.LogWarn(
						message: $"Page type is {page}, cannot be loaded.",
						classType: "SceneController",
						classMethod: "LoadSceneOnPage"
					);
					return;
				}

				// check if page controller is valid
				if (PageManager == null) {
					log.LogWarn(
						message: "PageController is not defined, cannot be loaded.",
						classType: "SceneController",
						classMethod: "LoadSceneOnPage"
					);
					return;
				}

				// check if scene is valid
				if (scene == ISceneType.None) {
					log.LogWarn(
						message: "Scene is none, cannot be loaded.",
						classType: "SceneController",
						classMethod: "LoadSceneOnPage"
					);
					return;
				}

				// scene cannot be the current scene
				if (scene.ToString() == currentScene.name) {
					log.LogWarn(
						message: "Scene cannot be current scene.",
						classType: "SceneController",
						classMethod: "LoadSceneOnPage"
					);
					return;
				}

				// scene cannot load, another scene is busy loading.
				if (IsLoadingScene) {
					log.LogWarn(
						message: "scene cannot load, another scene is busy loading.",
						classType: "SceneController",
						classMethod: "LoadSceneOnPage"
					);
					return;
				}

				// get ready to load a scene
				IsLoadingScene = Constant.Enable;
				string task = "AwaitLoadSceneOnPage";

				// load a scene asynchronous
				StopCoroutine(task);
				StartCoroutine(AwaitLoadSceneOnPage(scene, page));
			}
			#endregion

			#region Private Functions
			private IEnumerator AwaitLoadSceneOnPage(ISceneType scene, IPageType page) {
				// load a page
				PageManager.LoadPage(page);

				// load a scene on the current page
				string name = scene.ToString();
				SceneManager.LoadScene(scene.ToString());

				log.LogDebug($"Waiting until {name} scene finish to load on {page} page.",
					classType: "SceneController",
					classMethod: "AwaitLoadSceneOnPage"
				);

				// await PageManager current page to be this page
				while (SceneManager.GetActiveScene().name != scene.ToString()) {
					yield return null;
				}

				log.LogDebug($"Completed loading {name} scene on {page} page.",
					classType: "SceneController",
					classMethod: "AwaitLoadSceneOnPage"
				);

			}
			#endregion

			#region Callback Functions
			public void SubscribeOnSceneLoaded(CallbackOnSceneLoaded callback) {
				callbackOnSceneLoaded += callback;
				SceneManager.sceneLoaded += OnSceneLoaded;
			}

			public void UnsubscribeOnSceneLoaded(CallbackOnSceneLoaded callback) {
				callbackOnSceneLoaded -= callback;
				SceneManager.sceneLoaded -= OnSceneLoaded;
			}

			private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode) {
				log.LogDebug($"UnityEvent.SceneManger.onSceneLoaded event fired, loaded {scene.name} scene using {mode.ToString()} mode. Invoked OnSceneLoaded callbacks.",
					classType: "SceneController",
					classMethod: "OnSceneLoaded"
				);
				currentScene = scene;
				callbackOnSceneLoaded.Invoke(scene);
				IsLoadingScene = Constant.Disable;
			}
			#endregion
		}
	}
}