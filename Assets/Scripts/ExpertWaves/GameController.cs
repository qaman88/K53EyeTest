using ExpertWaves.Audio;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Enum;
using ExpertWaves.Log;
using ExpertWaves.Page.Enum;
using ExpertWaves.Scene.Enum;
using ExpertWaves.UserInput.Key;
using ExpertWaves.UserInput.Touch;
using ExpertWaves.Utility;
using ExpertWaves.Vibration;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace ExpertWaves {
	namespace Scene {
		public class GameController : MonoBehaviour {
			#region Public Variables
			public static GameController instance;
			public LogController log;
			public TouchInputController touchInput;
			public PageController pageController;
			public SceneController sceneController;
			public AudioController audioController;
			public VibrationController vibrationController;
			public KeyInputController keyInput;
			public TextAsset privacyTextAsset;
			public TextAsset termsTextAsset;
			#endregion

			#region Private Variables
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
				sceneController.SubscribeOnSceneLoaded(OnSceneLoaded);
				touchInput.RaiseTouchInputEvent += OnSwipe;
				keyInput.SubscribeKeyPressListener(OnKeyPress);
			}

			private void Start() {
			}
			private void OnDestroy() {
				sceneController.UnsubscribeOnSceneLoaded(OnSceneLoaded);
				touchInput.RaiseTouchInputEvent -= OnSwipe;
				keyInput.UnsubscribeKeyPressListener(OnKeyPress);

				gameObject.SetActive(false);
			}
			#endregion

			#region Private Functions		

			private void Configure() {
				// ensure instance is defined
				if (!instance) {
					instance = this;
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

				// initialize UI components
				if (SceneManager.GetActiveScene().name == ISceneType.Menu.ToString()) {
					InitButtonComponents();
				}
			}

			private void InitButtonComponents() {
				// Setting Button
				Button settingButton = GameObject.Find("settingButton").GetComponent<Button>();
				if (settingButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => {
						log.LogInfo(
							message: $"Setting button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// Switch to setting page
						pageController.LoadPage(IPageType.Setting);

						// Setting Menu Button
						Button settingMenuButton = GameObject.Find("settingMenuButton").GetComponent<Button>();
						if (settingMenuButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"Setting menu button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								pageController.SwitchPage(IPageType.Menu);
							});
							settingMenuButton.onClick = callback;
						}
					});
					settingButton.onClick = callback;
				}

				// Rate Button
				Button rateButton = GameObject.Find("rateButton").GetComponent<Button>();
				if (rateButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => {
						log.LogInfo(
							message: $"Rate button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						Application.OpenURL(Constant.Website);
					});
					rateButton.onClick = callback;
				}

				// About Button
				Button aboutButton = GameObject.Find("aboutButton").GetComponent<Button>();
				if (aboutButton) {
					// button event
					ButtonClickedEvent callback = new ButtonClickedEvent();
					// event listener
					callback.AddListener(() => {
						log.LogInfo(
							message: $"About button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// switch page
						pageController.LoadPage(IPageType.About);

						// about version text
						TextMeshProUGUI aboutVersionText = GameObject.Find("versionText").GetComponent<TextMeshProUGUI>();
						if (aboutVersionText) {
							aboutVersionText.text = $"Version {Application.version}";
						}

						// About Menu Button
						Button aboutMenuButton = GameObject.Find("aboutMenuButton").GetComponent<Button>();
						if (aboutMenuButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"About menu button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								pageController.SwitchPage(IPageType.Menu);
							});
							aboutMenuButton.onClick = callback;
						}

						// Privacy Button
						Button privacyButton = GameObject.Find("privacyButton").GetComponent<Button>();
						if (privacyButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"Privacy button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								pageController.LoadPage(IPageType.Privacy);
							});
							privacyButton.onClick = callback;
						}

						// Terms Button
						Button termsButton = GameObject.Find("termsButton").GetComponent<Button>();
						if (termsButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"Terms button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								pageController.LoadPage(IPageType.TermsConditions);

								// load terms

								// about version text
								TextMeshProUGUI termsText = GameObject.Find("termsText").GetComponent<TextMeshProUGUI>();
								if (termsText) {
									termsText.text = termsTextAsset.text;
								}
							});
							termsButton.onClick = callback;
						}
					});

					aboutButton.onClick = callback;
				}

				// Share Button
				Button shareButton = GameObject.Find("shareButton").GetComponent<Button>();
				if (shareButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => {
						log.LogInfo(
							message: $"Share button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
						pageController.LoadPage(IPageType.Share);
					});
					shareButton.onClick = callback;
				}



				// Help Button
				Button helpButton = GameObject.Find("helpButton").GetComponent<Button>();
				if (helpButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => {
						log.LogInfo(
							message: $"Help button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
						pageController.LoadPage(IPageType.Help);
					});
					helpButton.onClick = callback;
				}

				// Distance Test Button
				Button distanceTestButton = GameObject.Find("distanceTestButton").GetComponent<Button>();
				if (distanceTestButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => {
						log.LogInfo(
							message: $"Distance test button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
						sceneController.LoadSceneOnPage(ISceneType.DistanceScreening, IPageType.Loading);
					});
					distanceTestButton.onClick = callback;
				}

				// Color Test Button
				Button colorTestButton = GameObject.Find("colorTestButton").GetComponent<Button>();
				if (colorTestButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => {
						log.LogInfo(
							message: $"Color test button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
						sceneController.LoadSceneOnPage(ISceneType.ColorScreening, IPageType.Loading);
					});
					colorTestButton.onClick = callback;
				}
			}

			#endregion

			#region Callback Functions
			private void OnKeyPress(KeyCode key) {
				switch (key) {
					case KeyCode.W:
						audioController.PlayEffect(IEffectType.Warinig);
						break;

					case KeyCode.A:
						audioController.PlayVoice(IVoiceType.Warning1);
						break;

					case KeyCode.D:
						audioController.PlayVoice(IVoiceType.Warning1);
						break;

					case KeyCode.S:
						audioController.PlayEffect(IEffectType.Warinig);
						break;

					case KeyCode.M:
						pageController.SwitchPage(IPageType.Menu);
						break;

					case KeyCode.Z:
						break;

					default:
						break;
				}
			}

			private void OnSwipe(object sender, TouchInputEvent e) {
				IDirection swipe = e.Direction;

				log.LogInfo(
					message: $"Touch event, swipe {swipe}.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);

				audioController.PlayEffect(IEffectType.Swipe);
				vibrationController.Vibrate();

				switch (swipe) {
					case IDirection.Down:
						break;
					case IDirection.Up:
						break;
					case IDirection.Left:
						break;
					case IDirection.Right:
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