using ExpertWaves.Audio;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Data;
using ExpertWaves.Log;
using ExpertWaves.Utility;
using ExpertWaves.Vibration;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Toggle;

namespace ExpertWaves {
	namespace Setting {
		public class SettingController : MonoBehaviour {
			#region Public Variables
			// log
			public LogController log;

			// volume controller
			public Slider voiceVolumeSlider;
			public Slider effectsVolumeSlider;
			public Slider musicVolumeSlider;

			// audio controller
			public AudioController audioController;

			// vibration controller
			public VibrationController vibrationController;
			public Toggle vibrationToggle;
			public TextMeshProUGUI vibrationSwitchLabelText;
			#endregion

			#region Private Variables
			private DataStore<SettingData> settingDataStore;
			#endregion

			#region Public Variables Properties
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
			}

			private void Start() {
				settingDataStore.LoadData();

				vibrationController.VibrationEnabled = settingDataStore.data.VibrationState;
				vibrationToggle.isOn = settingDataStore.data.VibrationState;

				audioController.voiceTrack.source.volume = settingDataStore.data.VoiceVolume;
				voiceVolumeSlider.value = settingDataStore.data.VoiceVolume;

				audioController.musicTrack.source.volume = settingDataStore.data.MusicVolume;
				musicVolumeSlider.value = settingDataStore.data.MusicVolume;

				audioController.effectTrack.source.volume = settingDataStore.data.EffectVolume;
				effectsVolumeSlider.value = settingDataStore.data.EffectVolume;
			}

			private void OnDestroy() {
				settingDataStore.SaveData();
			}
			#endregion

			#region Public Functions
			#endregion

			#region Private Functions
			private void Configure() {
				settingDataStore = new DataStore<SettingData>(new SettingData(), log, "Setting.in");

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
						settingDataStore.data.VoiceVolume = volume;
						settingDataStore.SaveData();
					});
					voiceVolumeSlider.onValueChanged = sliderEvent;
				}

				if (musicVolumeSlider) {
					Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
					sliderEvent.AddListener((float volume) => {
						audioController.musicTrack.source.volume = volume;
						audioController.PlayMusic(IMusicType.Launch1);
						settingDataStore.data.MusicVolume = volume;
						settingDataStore.SaveData();
					});
					musicVolumeSlider.onValueChanged = sliderEvent;
				}

				if (effectsVolumeSlider) {
					Slider.SliderEvent sliderEvent = new Slider.SliderEvent();
					sliderEvent.AddListener((float volume) => {
						audioController.effectTrack.source.volume = volume;
						audioController.PlayEffect(IEffectType.Success);
						settingDataStore.data.EffectVolume = volume;
						settingDataStore.SaveData();
					});
					effectsVolumeSlider.onValueChanged = sliderEvent;
				}

				if (vibrationToggle) {
					ToggleEvent toggleEvent =  new ToggleEvent();
					toggleEvent.AddListener((bool state) => {
						vibrationController.VibrationEnabled = state;
						settingDataStore.data.VibrationState = state;
						settingDataStore.SaveData();
					});
					vibrationToggle.onValueChanged = toggleEvent;
				}
			}
			#endregion
		}
	}
}