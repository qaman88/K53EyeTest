using ExpertWaves.Audio.Enum;
using ExpertWaves.Audio.Track;
using ExpertWaves.Log;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ExpertWaves {
	namespace Audio {
		public class AudioController :MonoBehaviour {
			#region Public Variables
			// instance
			public static AudioController instance;

			// log
			public LogController log;

			// audio tracks
			public BackgroundTrack backgroundTrack = new BackgroundTrack();
			public VoiceTrack voiceTrack = new VoiceTrack();
			public EffectTrack effectTrack = new EffectTrack();

			#endregion

			#region Private Variables
			private Dictionary<IEffectType, AudioClip> effectAudioClips;
			private Dictionary<IBackgroundType, AudioClip> backgroundAudioClips;
			private Dictionary<IVoiceType, AudioClip> voiceAudioClips;

			#endregion

			#region Variables Properties
			public Dictionary<IEffectType, AudioClip> EffectAudioClips {
				get => effectAudioClips; set => effectAudioClips = value;
			}
			public Dictionary<IBackgroundType, AudioClip> BackgroundAudioClips {
				get => backgroundAudioClips; set => backgroundAudioClips = value;
			}
			public Dictionary<IVoiceType, AudioClip> VoiceAudioClips {
				get => voiceAudioClips; set => voiceAudioClips = value;
			}
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
			}
			#endregion

			#region Public Functions - Effect Player
			public void PlayEffect(IEffectType type) {
				if (EffectAudioClips.ContainsKey(type)) {
					effectTrack.source.clip = EffectAudioClips[type];
					effectTrack.source.Play();
					log.LogInfo(
						message: $"Playing audio clip: {type}",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				else {
					log.LogWarn(
						message: $"Failed to play audio clip: {type}",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
			}

			public void StopEffect(IEffectType type) {
				if (effectTrack.source.clip == EffectAudioClips[type]) {
					effectTrack.source.Stop();
				}
			}
			#endregion


			#region Public Functions - Background Music Player
			public void PlayBackgroundMusic(IBackgroundType type) {
				if (BackgroundAudioClips.ContainsKey(type)) {
					backgroundTrack.source.clip = BackgroundAudioClips[type];
					backgroundTrack.source.Play();
					log.LogInfo(
						message: $"Playing background track audio clip: {type}",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				else {
					log.LogWarn(
						message: $"Failed to play background track audio clip: {type}",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
			}

			public void StopBackgroundMusic(IBackgroundType type) {
				if (backgroundTrack.source.clip == BackgroundAudioClips[type]) {
					backgroundTrack.source.Stop();
				}
			}

			#endregion

			#region Public Functions - Voice Player
			public void PlayVoice(IVoiceType type) {
				if (VoiceAudioClips.ContainsKey(type)) {
					voiceTrack.source.clip = VoiceAudioClips[type];
					voiceTrack.source.Play();
					log.LogInfo(
						message: $"Playing audio clip: {type}",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				else {
					effectTrack.source.Play();
					log.LogWarn(
						message: $"Failed to play audio clip: {type}",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
			}

			public void StopVoice(IVoiceType type) {
				if (voiceTrack.source.clip == VoiceAudioClips[type]) {
					voiceTrack.source.Stop();
				}
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

				// initialize variables
				EffectAudioClips = new Dictionary<IEffectType, AudioClip>();
				BackgroundAudioClips = new Dictionary<IBackgroundType, AudioClip>();
				VoiceAudioClips = new Dictionary<IVoiceType, AudioClip>();

				// retrieve audio clips
				RetrieveSoundAudioClips();
				RetrieveVoiceAudioClips();
				RetrieveEffectAudioClips();
			}

			private void RetrieveEffectAudioClips() {
				foreach (EffectClip item in effectTrack.clips) {
					if (item.audioClip != null) {
						if (!EffectAudioClips.ContainsKey(item.clipType)) {
							EffectAudioClips.Add(item.clipType, item.audioClip);
							log.LogInfo(
								message: $"Added effect audio clip of {item.clipType} type..",
								classMethod: System.Reflection.MethodBase.GetCurrentMethod().Name,
								classType: GetType().Name
							);
						}
						else {
							log.LogWarn(
								message: $"Already added effect audio clip of {item.clipType} type. No duplicates for same clip type allowed",
								classType: System.Reflection.MethodBase.GetCurrentMethod().Name,
								classMethod: GetType().Name
							);
						}
					}
					else {
						log.LogWarn(
							message: $"Cannot add effect audio clip of {item.clipType} type.",
							classMethod: System.Reflection.MethodBase.GetCurrentMethod().Name,
							classType: GetType().Name
						);
					}
				}
			}

			private void RetrieveSoundAudioClips() {
				foreach (BackgroundClip item in backgroundTrack.clips) {
					if (item.audioClip != null) {
						if (!BackgroundAudioClips.ContainsKey(item.clipType)) {
							BackgroundAudioClips.Add(item.clipType, item.audioClip);
							log.LogInfo(
								message: $"Added sound audio clip of {item.clipType} type.",
								classMethod: System.Reflection.MethodBase.GetCurrentMethod().Name,
								classType: GetType().Name
							);
						}
						else {
							log.LogWarn(
								message: $"Already added sound audio clip of {item.clipType} type. No duplicates for same clip type allowed",
								classType: System.Reflection.MethodBase.GetCurrentMethod().Name,
								classMethod: GetType().Name
							);
						}
					}
					else {
						log.LogWarn(
							message: $"Cannot add sound audio clip of {item.clipType} type without audio clip.",
							classMethod: System.Reflection.MethodBase.GetCurrentMethod().Name,
							classType: GetType().Name
						);
					}
				}
			}

			private void RetrieveVoiceAudioClips() {
				foreach (var item in voiceTrack.clips) {
					if (item.audioClip != null) {
						if (!VoiceAudioClips.ContainsKey(item.clipType)) {
							VoiceAudioClips.Add(item.clipType, item.audioClip);
							log.LogInfo(
								message: $"Added voice audio clip of {item.clipType} type.",
								classType: System.Reflection.MethodBase.GetCurrentMethod().Name,
								classMethod: GetType().Name
							);
						}
						else {
							log.LogWarn(
								message: $"Already added voice audio clip of {item.clipType} type. No duplicates for same clip type allowed",
								classType: System.Reflection.MethodBase.GetCurrentMethod().Name,
								classMethod: GetType().Name
							);
						}
					}
					else {
						log.LogWarn(
							message: $"Cannot add voice audio clip of {item.clipType} type without audio clip.",
							classType: System.Reflection.MethodBase.GetCurrentMethod().Name,
							classMethod: GetType().Name
						);
					}
				}
			}
		}
		#endregion
	}
}
