using ExpertWaves.Log;
using System.Collections;
using UnityEngine;

namespace ExpertWaves {
	namespace Vibration {
		public class VibrationController : MonoBehaviour {
			#region Public Variables
			public static VibrationController instance;
			public LogController log;
			#endregion

			#region Private Variables
			#endregion

			#region Variables Properties
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
			}
			#endregion

			#region Public Functions
			public void Vibrate() {
				Handheld.Vibrate();
				log.LogInfo(
					message: "Device is vibrating.",
					classType: GetType().Name,
					classMethod: System.Reflection.MethodBase.GetCurrentMethod().Name
				);
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