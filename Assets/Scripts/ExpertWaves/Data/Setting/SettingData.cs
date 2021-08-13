namespace ExpertWaves {
	namespace Data {
		public class SettingData : IBaseData {
			#region Private Variables
			private float effectVolume;
			private float musicVolume;
			private float voiceVolume;
			private bool vibrationState;
			#endregion

			#region Public Properties
			public float EffectVolume {
				get => effectVolume;
				set => effectVolume = value;
			}
			public float MusicVolume {
				get => musicVolume;
				set => musicVolume = value;
			}
			public float VoiceVolume {
				get => voiceVolume;
				set => voiceVolume = value;
			}
			public bool VibrationState {
				get => vibrationState;
				set => vibrationState =  value ;
			}
			#endregion

			#region Public Functions
			#endregion

			#region Private Functions
			#endregion
		}
	}
}