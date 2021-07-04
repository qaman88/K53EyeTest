using ExpertWaves.Enum;
using UnityEngine;

namespace ExpertWaves {
	namespace UserInput {

		public delegate void CallbackKeyPress(KeyCode key);
		public delegate void CallbackSwipe(IDirection type);
		public delegate void CallbackSingleTab();

		public delegate void onSwipe(IDirection key);

		public class InputController : MonoBehaviour {
			#region Public Variables
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
			private void Update() {
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
			#endregion

			#region Private Functions
			private void KeyPressListener() {
				foreach (KeyCode key in userInputKeys) {
					if (Input.GetKeyUp(key)) {
						callbackKeyPress(key);
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

					if (!stopTouch) {
						if (distance.x < -swipeRange) {
							stopTouch = true;
							callbackSwipe.Invoke(IDirection.LEFT);
						}
						else if (distance.x > swipeRange) {
							stopTouch = true;
							callbackSwipe.Invoke(IDirection.RIGHT);
						}
						else if (distance.y < -swipeRange) {
							stopTouch = true;
							callbackSwipe.Invoke(IDirection.DOWN);
						}
						else if (distance.y > swipeRange) {
							stopTouch = true;
							callbackSwipe.Invoke(IDirection.UP);
						}
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
					}
				}
			}
			#endregion

			#region Public Functions
			public void RegisterKeyPressListener(CallbackKeyPress callback, KeyCode[] keys = null) {
				callbackKeyPress += callback;
				if (keys != null) {
					userInputKeys = keys;
				}
			}
			public void UnregisterKeyPressListener(CallbackKeyPress callback) {
				callbackKeyPress -= callback;
			}

			public void RegisterSwipeListener(CallbackSwipe callback) {
				callbackSwipe += callback;
			}

			public void UnregisterSwipeListener(CallbackSwipe callback) {
				callbackSwipe -= callback;
			}
			public void RegisterSingleTabListener(CallbackSingleTab callback) {
				callbackSingleTab += callback;
			}

			public void UnregisterSingleTabListener(CallbackSingleTab callback) {
				callbackSingleTab -= callback;
			}
			#endregion
		}
	}
}