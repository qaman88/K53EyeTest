using ExpertWaves.Enum;
using ExpertWaves.Log;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace ExpertWaves {
	namespace UserInput {

		public delegate void CallbackKeyPress(KeyCode key);
		public delegate void CallbackSwipe(IDirection type);
		public delegate void CallbackSingleTab();
		public delegate void onSwipe(IDirection key);

		public class InputController : MonoBehaviour {
			#region Public Variables
			public static InputController instance;
			public LogController log;
			public KeyCode[] userInputKeys;
			public double swipeRange = 50;
			public double tapRange = 10;
			#endregion

			#region Private Variables
			private CallbackKeyPress callbackKeyPress;
			private CallbackSwipe callbackSwipe;
			private CallbackSingleTab callbackSingleTab;
			private Vector2 startPosition;
			private Vector2 currentPosition;
			private Vector2 endPosition;
			private bool stopTouch = false;
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

				// listen swipe input
				if (callbackSwipe != null) {
					SwipeListener();
				}

				// listen to single tab
				if (callbackSingleTab != null) {
					TabListener();
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

			private void SwipeListener() {
				if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
					startPosition = Input.GetTouch(0).position;
				}
				else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
					currentPosition = Input.GetTouch(0).position;
					Vector2 distance = currentPosition - startPosition;

					// identify direction of swipe
					if (!stopTouch) {
						IDirection swipe = IDirection.None;
						if (distance.x < -swipeRange) {
							stopTouch = true;
							swipe = IDirection.Left;
						}
						else if (distance.x > swipeRange) {
							stopTouch = true;
							swipe = IDirection.Right;
						}
						else if (distance.y < -swipeRange) {
							stopTouch = true;
							swipe = IDirection.Down;
						}
						else if (distance.y > swipeRange) {
							stopTouch = true;
							swipe = IDirection.Up;
						}

						// fire swipe event through subscribed callbacks
						callbackSwipe.Invoke(swipe);

						// log
						log.LogDebug(
							message: $"Touch event, swipe {swipe}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}
				}
			}

			private void TabListener() {
				if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
					endPosition = Input.GetTouch(0).position;
					Vector2 distance = endPosition - startPosition;
					stopTouch = false;

					if (Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange) {
						callbackSingleTab.Invoke();
						log.LogDebug(
							message: $"Touch event, single tab.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}
				}
			}
			#endregion

			#region Callback Functions
			public void SubscribeKeyPressListener(CallbackKeyPress callback, KeyCode[] keys = null) {
				callbackKeyPress += callback;
				if (keys != null) {
					userInputKeys = (KeyCode[]) userInputKeys.Concat(keys).Distinct();
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

			public void SubscribeSwipeListener(CallbackSwipe callback) {
				callbackSwipe += callback;
				// log
				log.LogDebug(
					message: $"Subscription to onSwipe event detected.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
			}

			public void UnsubscribeSwipeListener(CallbackSwipe callback) {
				callbackSwipe -= callback;
				// log
				log.LogDebug(
					message: $"Unsubscription to onSwipe event detected.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
			}
			public void RegisterSingleTabListener(CallbackSingleTab callback) {
				callbackSingleTab += callback;
				// log
				log.LogDebug(
					message: $"Subscription to onSingleTab event detected.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
			}

			public void UnregisterSingleTabListener(CallbackSingleTab callback) {
				callbackSingleTab -= callback;
				// log
				log.LogDebug(
					message: $"Unsubscription to onSingleTab event detected.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
			}
			#endregion
		}
	}
}