using ExpertWaves.Log;
using UnityEngine;

namespace ExpertWaves {
	namespace Enum {
		public class AudioController : MonoBehaviour {
			#region Public Variables
			public static AudioController instance;
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

			private void Start() {

			}

			private void Update() {

			}

			private void OnDestroy() {

			}
			#endregion

			#region Public Functions
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