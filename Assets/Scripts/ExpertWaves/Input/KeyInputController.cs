using ExpertWaves.Enum;
using ExpertWaves.Log;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ExpertWaves {
	namespace UserInput {
		namespace Key {
			public delegate void CallbackKeyPress(KeyCode key);

			public class KeyInputController :MonoBehaviour {
				#region Public Variables
				public static KeyInputController instance;
				public LogController log;
				public List<KeyCode> userInputKeys = new List<KeyCode>(){KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
				#endregion

				#region Private Variables
				private CallbackKeyPress callbackKeyPress;
				#endregion

				#region Unity Functions
				private void Awake() {
					Configure();
					Debug.Log("InputController is Awake.");
				}

				public void Update() {
					// listen to key press
					if (userInputKeys != null && callbackKeyPress != null) {
						KeyPressListener();
					}
				}

				private void OnDestroy() {
					log.LogInfo(
						message: "InputController is destroyed.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				#endregion

				#region Public Functions
				public void PushKey(KeyCode key) {
					if (userInputKeys.Contains(key)) {
						userInputKeys.Append(key);
					}
				}

				public void PopKey(KeyCode key) {
					if (userInputKeys.Contains(key)) {
						userInputKeys.Remove(key);
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

				private void KeyPressListener() {
					foreach (KeyCode key in userInputKeys) {
						if (Input.GetKeyUp(key)) {
							callbackKeyPress(key);
							// log
							log.LogDebug(
								message: $"Key press event, key: {key}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);
						}
					}
				}

				#endregion

				#region Callback Functions
				public void SubscribeKeyPressListener(CallbackKeyPress callback, List<KeyCode> keys = null) {
					callbackKeyPress += callback;
					if (keys != null) {
						userInputKeys = (List<KeyCode>) userInputKeys.Concat(keys);
					}
					// log
					log.LogDebug(
						message: $"Subscription to onKeyPress event detected.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				public void UnsubscribeKeyPressListener(CallbackKeyPress callback) {
					callbackKeyPress -= callback;
					// log
					log.LogDebug(
						message: $"Unsubscription to onKeyPress event detected.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				#endregion
			}
		}
	}
}