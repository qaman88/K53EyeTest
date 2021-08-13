using ExpertWaves.Log;
using ExpertWaves.Utility;
using UnityEngine;

namespace ExpertWaves {
	namespace Vibration {
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
			#endregion

			#region Public Functions
			public void Vibrate() {
				if (VibrationEnabled) {
					Handheld.Vibrate();
					log.LogInfo(
						message: "Device is vibrating.",
						classType: GetType().Name,
						classMethod: System.Reflection.MethodBase.GetCurrentMethod().Name
					);

				}
				else {
					log.LogInfo(
						message: "Vibration is disabled.",
						classType: GetType().Name,
						classMethod: System.Reflection.MethodBase.GetCurrentMethod().Name
					);
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
			}
			#endregion
		}
	}
}