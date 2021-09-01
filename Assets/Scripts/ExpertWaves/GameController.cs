using ExpertWaves.Audio;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Log;
using ExpertWaves.Page.Enum;
using ExpertWaves.Scene.Enum;
using ExpertWaves.Setting;
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
		public enum ITextReader {
			Help,
			Privacy,
			Terms
		}

		public class GameController : MonoBehaviour {
			#region Public Variables
			public static GameController instance;
			public PageController pageController;
			public SceneController sceneController;
			public TextAsset helpTextAsset;
			public int helpContentLength = 8000;
			public TextAsset privacyTextAsset;
			public int privacyContentLength = 5000;
			public TextAsset termsTextAsset;
			public int termsContentLength = 4000;

			// setting page UI components
			public Slider voiceVolumeSlider;
			public Slider effectsVolumeSlider;
			public Slider backgroundVolumeSlider;
			public Toggle vibrationToggle;
			#endregion

			#region Private Variables
			private LogController log;
			private AudioController audioController;
			private VibrationController vibrationController;
			private SettingController settingController;
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
				sceneController.SubscribeOnSceneLoaded(OnSceneLoaded);
			}

			private void Start() {
#if UNITY_EDITOR
				log.LogWarn(
					message: $"Android is required to run the script.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
#else
				InitializePlugin();
#endif
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
				voiceVolumeSlider.value = settingController.VoiceVolume;
				if (voiceVolumeSlider) {
					Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
					sliderEvent.AddListener((float volume) => {
						log.LogInfo(
							message: $"Slider event for voice track. Volume: {volume}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
						if (voiceVolumeSlider.value != settingController.VoiceVolume) {
							audioController.PlayVoice(volume <= 0 ? IVoiceType.VoiceOff : IVoiceType.VoiceOn);
						}
						audioController.voiceTrack.source.volume = volume;
						settingController.VoiceVolume = volume;
						settingController.SaveData();
					});
					voiceVolumeSlider.onValueChanged = sliderEvent;
				}

				backgroundVolumeSlider.value = settingController.MusicVolume;
				if (backgroundVolumeSlider) {
					Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
					sliderEvent.AddListener((float volume) => {
						log.LogInfo(
							message: $"Slider event for background track. Volume: {volume}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
						audioController.backgroundTrack.source.volume = volume;
						if (backgroundVolumeSlider.value != settingController.MusicVolume) {
							audioController.PlayBackgroundMusic(IBackgroundType.Test);
						}
						settingController.MusicVolume = volume;
						settingController.SaveData();
					});
					backgroundVolumeSlider.onValueChanged = sliderEvent;
				}

				effectsVolumeSlider.value = settingController.EffectVolume;
				if (effectsVolumeSlider) {
					Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
					sliderEvent.AddListener((float volume) => {
						log.LogInfo(
							message: $"Slider event for effect track. Volume: {volume}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
						audioController.effectTrack.source.volume = volume;
						if (effectsVolumeSlider.value != settingController.EffectVolume) {
							audioController.PlayEffect(IEffectType.Test);
						}
						settingController.EffectVolume = volume;
						settingController.SaveData();
					});
					effectsVolumeSlider.onValueChanged = sliderEvent;
				}

				vibrationToggle.isOn = settingController.VibrationState;
				if (vibrationToggle) {
					ToggleEvent toggleEvent =  new ToggleEvent();
					toggleEvent.AddListener((bool state) => {
						log.LogInfo(
							message: $"Toggle event for vibration. State: {state}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						vibrationController.VibrationEnabled = state;
						settingController.VibrationState = state;
						settingController.SaveData();

						if (state) {
							int ms = 500;
							vibrationController.Vibrate(ms);
						}
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
						// play sound effect for item select
						audioController.PlayEffect(IEffectType.Select);

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
								// play sound effect for item select
								audioController.PlayEffect(IEffectType.Select);

								log.LogInfo(
									message: $"Setting menu button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);

								// set menu page active to allow access of game objects
								pageController.SetPageActiveState(IPageType.Menu);
								pageController.SetPageActiveState(IPageType.Setting, false);

								// switch to menu page
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
						// play sound effect for item select
						audioController.PlayEffect(IEffectType.Select);

						log.LogInfo(
							message: $"Rate button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// vibrate the device
						long [] waves = new long[] {100, 100, 100, 100, 100};
						int [] amplitudes = new int[] {125, 0, 250, 0, 125};
						vibrationController.Vibrate(waves, amplitudes);

						// open URL for app link
						Application.OpenURL(Constant.AppLink);
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
						// play sound effect for item select
						audioController.PlayEffect(IEffectType.Select);

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
								// play sound effect for item select
								audioController.PlayEffect(IEffectType.Select);

								log.LogInfo(
									message: $"About menu button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);

								// set menu page active to allow access of game objects
								pageController.SetPageActiveState(IPageType.Menu);
								pageController.SetPageActiveState(IPageType.About, false);

								// switch to menu page
								pageController.SwitchPage(IPageType.Menu);
							});
							aboutMenuButton.onClick = callback;
						}

						// Privacy Button
						Button privacyButton = GameObject.Find("privacyButton").GetComponent<Button>();
						if (privacyButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => CallbackTextReaderMenu(ITextReader.Privacy));
							privacyButton.onClick = callback;
						}

						// Terms Button
						Button termsButton = GameObject.Find("termsButton").GetComponent<Button>();
						if (termsButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => CallbackTextReaderMenu(ITextReader.Terms));
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

						// play sound effect for item select
						audioController.PlayEffect(IEffectType.Select);

						// native share intent
						Share();
					});
					shareButton.onClick = callback;
				}

				// Help Button
				Button helpButton = GameObject.Find("helpButton").GetComponent<Button>();
				if (helpButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => CallbackTextReaderMenu(ITextReader.Help));
					helpButton.onClick = callback;
				}

				// Distance Test Button
				Button distanceTestButton = GameObject.Find("distanceTestButton").GetComponent<Button>();
				if (distanceTestButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => {
						// play sound effect for item select
						audioController.PlayEffect(IEffectType.Select);

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
						// play sound effect for item select
						audioController.PlayEffect(IEffectType.Select);

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

			public void CallbackTextReaderMenu(ITextReader text) {
				// play sound effect for item select
				audioController.PlayEffect(IEffectType.Select);

				log.LogInfo(
					message: $"TextReader button event of type {text}.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
				pageController.LoadPage(IPageType.TextReader);

				// content text
				TextMeshProUGUI title = GameObject.Find("titleTextReader").GetComponent<TextMeshProUGUI>();
				TextMeshProUGUI content = GameObject.Find("contentTextReader").GetComponent<TextMeshProUGUI>();

				Vector2 size = content.rectTransform.sizeDelta;
				float length = 0;

				switch (text) {
					case ITextReader.Help:
						content.text = helpTextAsset.text;
						length = helpContentLength;
						title.text = "Help";
						break;
					case ITextReader.Privacy:
						content.text = privacyTextAsset.text;
						length = privacyContentLength;
						title.text = "Privacy";
						break;
					case ITextReader.Terms:
						content.text = termsTextAsset.text;
						length = termsContentLength;
						title.text = "Terms";
						break;
					default:
						break;
				}

				size.y = length;
				content.rectTransform.sizeDelta = size;

				// Menu Button
				Button menuButton = GameObject.Find("menuButtonTextReader").GetComponent<Button>();
				if (menuButton) {
					ButtonClickedEvent callback = new ButtonClickedEvent();
					callback.AddListener(() => {
						// play sound effect for item select
						audioController.PlayEffect(IEffectType.Select);

						log.LogInfo(
							message: $"Text reader menu button is clicked.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// set menu page active to allow access of game objects
						pageController.SetPageActiveState(IPageType.Menu);
						pageController.SetPageActiveState(IPageType.TextReader, false);

						// switch to menu page
						pageController.SwitchPage(IPageType.Menu);
					});
					menuButton.onClick = callback;
				}
			}
			#endregion

			#region Callback Functions
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

			private void InitializePlugin() {
				log.LogWarn(
					message: $"Android platform is initializing plugin. Name: {androidPluginName}",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);

				try {
					AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject context = activity.Call<AndroidJavaObject>("getApplicationContext");
					androidPlugin = new AndroidJavaObject(androidPluginName);

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
						message: $"Failed to initialize Android plugin.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name,
						exception: e
					);
				}
			}

			public void Toast(string msg) {
				if (androidPlugin != null) {
					androidPlugin.Call("Toast", msg);
					log.LogInfo(
						message: $"Toast is done using android plugin.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				else {
					// TODO - Toast fall-back plan
					log.LogWarn(
						message: $"Toast is done using fall-back plan for android plugin.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
			}

			public void Share() {
				string info =  $"Hey \nHere is an app to help you pass K53 eye test on a GO. Download {Application.productName} App on {Constant.AppLink}. \nThanks";

				if (androidPlugin != null) {
					androidPlugin.Call("Share", info);
					log.LogInfo(
						message: $"Share is done using android plugin.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				else {
					//Application.OpenURL($"mailto:?Subject=Share K53 Eye Test App");
					log.LogWarn(
						message: $"Share is done using fall-back plan for android plugin.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
					InitializePlugin();
				}
			}
			#endregion
		}
	}
}