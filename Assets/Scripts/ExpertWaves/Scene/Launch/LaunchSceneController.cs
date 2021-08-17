using ExpertWaves.Audio;
using ExpertWaves.Log;
using ExpertWaves.Page.Enum;
using ExpertWaves.Scene.Enum;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ExpertWaves {
	namespace Scene {
		public class LaunchSceneController : MonoBehaviour {
			#region Public Variables
			public Image imageCompanyIcon;
			public SceneController sceneController;
			public LogController log;
			public AudioController audioController;
			#endregion

			#region Private Variables
			#endregion

			#region Public Variables Properties
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
			}

			private void Start() {
				sceneController.IsLoadingScene = false;
				StartCoroutine(AsyncLoadMenu());
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
				// ensure log controller is defined
				if (!log) {
					log = LogController.instance;
				}

				// ensure scene controller is defined
				if (!audioController) {
					audioController = AudioController.instance;
				}
			}

			private IEnumerator AsyncLoadMenu() {
				if (audioController) {
					audioController.PlayMusic(Audio.Enum.IMusicType.Launch1);
				}

				// wait for page to finish loading
				yield return new WaitForSeconds(5);

				// wait until loading scene finish loading
				while (sceneController.IsLoadingScene) {
					yield return new WaitForSeconds(1);
				}

				// load menu scene
				sceneController.LoadSceneOnPage(ISceneType.Menu, IPageType.Loading);

				yield return null;
			}
			#endregion
		}
	}
}