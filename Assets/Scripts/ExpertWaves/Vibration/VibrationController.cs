using ExpertWaves.Log;
using ExpertWaves.Utility;
using System.Collections;
using System.Reflection;
using UnityEngine;

namespace ExpertWaves {
	namespace Vibration {
		/**
		 * Vibration controller vibrates the device. 
		 * First attempt to use android plugin library to trigger waveforms vibration.
		 * If plugin failed to load, it fall back to Unity hand held vibration.
		*/
		public class VibrationController : MonoBehaviour {
			#region Public Variables
			public static VibrationController instance;
			public LogController log;
			public bool vibrationEnabled = Constant.SwitchOn;
			#endregion

			#region Private Variables
			#endregion

			#region Variables Properties
			public bool VibrationEnabled {
				get => vibrationEnabled;
				set => vibrationEnabled = value;
			}
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
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
			#endregion

			#region Public Functions
			public void Vibrate(long ms) {
				if (VibrationEnabled) {
					StartCoroutine(AsyncVibrate(ms));
				}
				else {
					log.LogInfo(
						message: "Vibration is disabled.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
			}

			public IEnumerator AsyncVibrate(long ms) {
				// android plugin waveform vibration
				if (androidPlugin != null) {
					NativeVibrate(ms);

					log.LogInfo(
						message: "Device is vibrating using android plugin.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				// unity vibration
				else {
					Handheld.Vibrate();

					log.LogInfo(
						message: "Device is vibrating using Unity hand held.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}

				yield return null;
			}

			public void Vibrate(long[] wavelengths, int[] implitudes) {
				if (VibrationEnabled) {
					StartCoroutine(AsyncVibrate(wavelengths, implitudes));
				}
				else {
					log.LogInfo(
						message: "Vibration is disabled.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
			}

			public IEnumerator AsyncVibrate(long[] wavelengths, int[] implitudes) {
				// android plugin waveform vibration
				if (androidPlugin != null) {
					NativeVibrate(wavelengths, implitudes);

					log.LogInfo(
						message: "Device is vibrating using android plugin.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				// unity vibration
				else {
					Handheld.Vibrate();

					log.LogInfo(
						message: "Device is vibrating using Unity hand held.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}

				yield return null;
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
			}
			#endregion

			#region Android Plugin
			private string androidPluginName = "za.co.expertwaves.k53eyetest.unityk53javaplugin.NativeVibration";
			private AndroidJavaObject androidPlugin;

			private void InitializePlugin() {
				androidPlugin = new AndroidJavaObject(androidPluginName);

				if (androidPlugin != null) {
					log.LogInfo(
						message: $"Android plugin initiated and activity setup completed.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}

				else {
					log.LogError(
						message: $"Failed initialize Android plugin, pluginInstance is null.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
			}

			/***
			 * The function to vibrate the device provided amplitude and wavelength.
			 * @param wavelength - duration in ms to vibrate the device.
			 * @param amplitude - amplitude of wavelength of vibration (0 - 255).
			 */
			public void NativeVibrate(long ms, int amplitude = 255) {
				if (androidPlugin == null) {
					InitializePlugin();
				}
				androidPlugin.CallStatic("Vibrate", ms, amplitude);
			}

			/***
			 * The function to vibrate the device using waveforms with amplitudes. Option to repeat is included.
			 * The maximum amplitude value is 255.
			 * @param wavelengths - array of wavelength patterns,
			 * @param amplitudes - array of wavelengths amplitudes, maximum amplitude value is 255.
			 * @param repeat - number of playbacks, it should be -1 for a single shot.
			 */
			public void NativeVibrate(long[] waves, int[] amplitudes, int repeat = -1) {
				if (androidPlugin != null) {
					InitializePlugin();
				}
				androidPlugin.CallStatic("Vibrate", waves, amplitudes, -1);
			}
			#endregion
		}
	}
}