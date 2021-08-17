using ExpertWaves.Audio;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Enum;
using ExpertWaves.Log;
using ExpertWaves.Page.Enum;
using ExpertWaves.Scene.Enum;
using ExpertWaves.Setting;
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
using static UnityEngine.UI.Toggle;

namespace ExpertWaves {
	namespace Scene {
		public class GameController : MonoBehaviour {
			#region Public Variables
			public static GameController instance;
			public PageController pageController;
			public SceneController sceneController;
			public TextAsset helpTextAsset;
			public TextAsset privacyTextAsset;
			public TextAsset termsTextAsset;
			public SettingController settingController;

			// setting page UI components
			public Slider voiceVolumeSlider;
			public Slider effectsVolumeSlider;
			public Slider musicVolumeSlider;
			public Toggle vibrationToggle;
			#endregion

			#region Private Variables
			private LogController log;
			private AudioController audioController;
			private VibrationController vibrationController;
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
				sceneController.SubscribeOnSceneLoaded(OnSceneLoaded);
			}

			private void Start() {
				InitializePlugin(androidPluginName);

				voiceVolumeSlider.value = settingController.VoiceVolume;
				vibrationToggle.isOn = settingController.VibrationState;
				musicVolumeSlider.value = settingController.MusicVolume;
				effectsVolumeSlider.value = settingController.EffectVolume;
			}

			private void OnDestroy() {
				sceneController.UnsubscribeOnSceneLoaded(OnSceneLoaded);
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

				// ensure setting controller is defined
				if (!settingController) {
					settingController = SettingController.instance;
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

				InitSettingComponents();
			}

			private void InitSettingComponents() {

				if (voiceVolumeSlider) {
					Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
					sliderEvent.AddListener((float volume) => {
						log.LogInfo(
							message: $"Slider event for voice track.  Volume: {volume}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
						audioController.voiceTrack.source.volume = volume;
						audioController.PlayVoice(IVoiceType.Success1);
						settingController.VoiceVolume = volume;
						settingController.SaveData();
					});
					voiceVolumeSlider.onValueChanged = sliderEvent;
				}

				if (musicVolumeSlider) {
					Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
					sliderEvent.AddListener((float volume) => {
						audioController.backgroundTrack.source.volume = volume;
						audioController.PlayBackgroundMusic(IBackgroundType.Launch1);
						settingController.MusicVolume = volume;
						settingController.SaveData();
					});
					musicVolumeSlider.onValueChanged = sliderEvent;
				}

				if (effectsVolumeSlider) {
					Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
					sliderEvent.AddListener((float volume) => {
						audioController.effectTrack.source.volume = volume;
						audioController.PlayEffect(IEffectType.Success);
						settingController.EffectVolume = volume;
						settingController.SaveData();
					});
					effectsVolumeSlider.onValueChanged = sliderEvent;
				}

				if (vibrationToggle) {
					ToggleEvent toggleEvent =  new ToggleEvent();
					toggleEvent.AddListener((bool state) => {
						vibrationController.VibrationEnabled = state;
						settingController.VibrationState = state;
						settingController.SaveData();
					});
					vibrationToggle.onValueChanged = toggleEvent;
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

								// content text
								TextMeshProUGUI privacyContent = GameObject.Find("privacyContent").GetComponent<TextMeshProUGUI>();
								if (privacyContent) {
									privacyContent.text = privacyTextAsset.text;
								}

								// Return Button
								Button privacyReturnButton = GameObject.Find("privacyReturnButton").GetComponent<Button>();
								if (privacyReturnButton) {
									ButtonClickedEvent callback = new ButtonClickedEvent();
									callback.AddListener(() => {
										log.LogInfo(
											message: $"Privacy Return button is clicked.",
											classType: GetType().Name,
											classMethod: MethodBase.GetCurrentMethod().Name
										);
										pageController.SwitchPage(IPageType.About);
									});
									privacyReturnButton.onClick = callback;
								}
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

								// content text
								TextMeshProUGUI termsContent = GameObject.Find("termsContent").GetComponent<TextMeshProUGUI>();
								if (termsContent) {
									termsContent.text = termsTextAsset.text;
								}

								// Return Button
								Button termsReturnButton = GameObject.Find("termsReturnButton").GetComponent<Button>();
								if (termsReturnButton) {
									ButtonClickedEvent callback = new ButtonClickedEvent();
									callback.AddListener(() => {
										log.LogInfo(
											message: $"Terms return button is clicked.",
											classType: GetType().Name,
											classMethod: MethodBase.GetCurrentMethod().Name
										);
										pageController.SwitchPage(IPageType.About);
									});
									termsReturnButton.onClick = callback;
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

						// native share intent
						Share();
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

						// content text
						TextMeshProUGUI helpContent = GameObject.Find("helpContent").GetComponent<TextMeshProUGUI>();
						if (helpContent) {
							helpContent.text = helpTextAsset.text;
						}

						// Menu Button
						Button helpMenuButton = GameObject.Find("helpMenuButton").GetComponent<Button>();
						if (helpMenuButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"Help menu button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								pageController.SwitchPage(IPageType.Menu);
							});
							helpMenuButton.onClick = callback;
						}
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

			#region Android Library
			private string androidPluginName = "za.co.expertwaves.k53eyetest.unityk53javaplugin.NativeShare";
			private AndroidJavaObject androidPlugin;
			private void InitializePlugin(string plugin) {
#if UNITY_EDITOR
				log.LogWarn(
					message: $"Android is required to run the script.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
#else
				try {
					AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
					androidPlugin = new AndroidJavaObject(plugin);

					if (androidPlugin == null) {
						log.LogError(
							message: $"Failed initialize Android plugin, pluginInstance is null.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}
					else {
						androidPlugin.CallStatic("SetUnityActivity", activity);
						log.LogInfo(
							message: $"Android plugin initiated and activity setup completed.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}
				}
				catch (System.Exception e) {
					log.LogError(
						message: $"Failed initialize Android plugin.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name,
						exception: e
					);
				}
#endif
			}

			public void Toast(string msg) {
#if UNITY_EDITOR
				log.LogWarn(
					message: $"Android is required to run the script.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
#else
				if (androidPlugin != null) {
					androidPlugin.Call("Toast", msg);
				}
				Share();
#endif
			}

			public void Share() {
#if UNITY_EDITOR
				log.LogWarn(
					message: $"Android is required to run the script.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
#else
				string info =  $"Hey \nHere is an app to help you pass K53 eye test on a GO. Download {Application.productName} App on {Constant.Website}. \nThanks";
				if (androidPlugin != null) {
					androidPlugin.Call("Share", info);
				}
#endif
			}
			#endregion
		}
	}
}