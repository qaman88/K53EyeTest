using ExpertWaves.Audio;
using ExpertWaves.Data;
using ExpertWaves.Log;
using ExpertWaves.Vibration;
using UnityEngine;

namespace ExpertWaves {
	namespace Setting {
		public class SettingController : MonoBehaviour {
			#region Public Variables
			// log
			public LogController log;

			// audio controller
			public AudioController audioController;

			// setting controller singleton instance
			public static SettingController instance;

			// vibration controller
			public VibrationController vibrationController;
			#endregion

			#region Private Variables
			// data store
			private DataStore<SettingData> settingDataStore;
			#endregion

			#region Public Variables Properties
			public bool VibrationState {
				get => settingDataStore.data.VibrationState;
				set => settingDataStore.data.VibrationState = value;
			}

			public float VoiceVolume {
				get => settingDataStore.data.VoiceVolume;
				set => settingDataStore.data.VoiceVolume = value;
			}

			public float MusicVolume {
				get => settingDataStore.data.MusicVolume;
				set => settingDataStore.data.MusicVolume = value;
			}

			public float EffectVolume {
				get => settingDataStore.data.EffectVolume;
				set => settingDataStore.data.EffectVolume = value;
			}
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
			}

			private void Start() {
				settingDataStore.LoadData();
				vibrationController.VibrationEnabled = settingDataStore.data.VibrationState;
				audioController.voiceTrack.source.volume = settingDataStore.data.VoiceVolume;
				audioController.backgroundTrack.source.volume = settingDataStore.data.MusicVolume;
				audioController.effectTrack.source.volume = settingDataStore.data.EffectVolume;
			}

			private void OnDestroy() {
				settingDataStore.SaveData();
			}
			#endregion

			#region Public Functions
			public void SaveData() {
				settingDataStore.SaveData();
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

				settingDataStore = new DataStore<SettingData>(new SettingData(), log, "Setting.in");
				#endregion
			}
		}
	}
}