using ExpertWaves.Enum;
using ExpertWaves.Log;
using System;
using System.Reflection;
using UnityEngine;

namespace ExpertWaves {
	namespace UserInput {
		namespace Touch {

			public class TouchInputEvent :MonoBehaviour {
				public IDirection Direction {
					set; get;
				}

				public TouchInputEvent(IDirection direction) {
					Direction = direction;
				}
			}

			public class TouchInputController :MonoBehaviour {
				#region Public Variables
				public static TouchInputController instance;
				public LogController log;
				public double swipeRange = 50;
				public double tapRange = 10;
				public event EventHandler<TouchInputEvent> RaiseTouchInputEvent;
				#endregion

				#region Private Variables
				private Vector2 startPosition;
				private Vector2 currentPosition;
				private Vector2 endPosition;
				private bool stopTouch = false;
				#endregion

				#region Public Functions
				// virtual to allow derived classes to override the event invocation behavior.
				protected virtual void onRaiseTouchInputEvent(TouchInputEvent inputEvent) {
					// copy to avoid race condition between subscriber and unsubscribes.
					EventHandler<TouchInputEvent> eventRaiser = RaiseTouchInputEvent;

					// event raiser is null if no subscribers
					if (eventRaiser != null) {
						eventRaiser(this, inputEvent);
					}
				}
				#endregion

				#region Unity Functions
				private void Awake() {
					Configure();
					Debug.Log("InputController is Awake.");
				}

				private void Update() {
					// listen swipe input
					SwipeListener();
				}
				#endregion


				#region Private Functions
				private void Configure() {
					// ensure instance is defined
					if (!instance) {
						instance = this;
						DontDestroyOnLoad(gameObject);
					}

					// ensure log controller is defined
					if (!log) {
						log = LogController.instance;
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

							if (swipe != IDirection.None) {
								onRaiseTouchInputEvent(new TouchInputEvent(swipe));
								/*
								log.LogInfo(
									message: $"Swipe: {swipe}.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								*/
							}
						}
					}
					if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
						endPosition = Input.GetTouch(0).position;
						Vector2 distance = endPosition - startPosition;
						stopTouch = false;

						if (Mathf.Abs(distance.x) < tapRange && Mathf.Abs(distance.y) < tapRange) {
							/*
							log.LogInfo(
								message: $"Single Tap Detected",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);
							*/
						}
					}
				}

				#endregion
			}
		}
	}
}