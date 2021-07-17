using ExpertWaves.Log;
using System.Collections;
using System.Reflection;
using ExpertWaves.Page.Enum;
using UnityEngine;

namespace ExpertWaves {
	namespace Enum {
		public delegate void CallbackPageLoaded(Page page);

		public class Page : MonoBehaviour {

			#region Public Variables
			public bool animationEnable;
			public IPageType type;
			#endregion

			#region Private Variables
			private LogController log;
			private bool active;
			private Animator animator;
			ISwitch targetState = ISwitch.None;
			CallbackPageLoaded callbackPageLoaded;
			#endregion

			#region Variables Properties			
			public bool Active {
				get {
					return this.active;
				}
				set {
					gameObject.SetActive(value);
					this.active = value;
				}
			}
			public bool AnimationEnable { get => this.animationEnable; set => this.animationEnable = value; }
			public ISwitch TargetState { get => this.targetState; set => this.targetState = value; }
			public IPageType Type { get => this.type; set => this.type = value; }

			#endregion

			#region Unity Functions
			private void Awake() {
				// ensure log controller is defined
				if (!log) {
					log = LogController.instance;
				}
			}
			#endregion

			#region Public Functions
			public void RegisterPageLoadedCallback(CallbackPageLoaded callback) {
				callbackPageLoaded += callback;
			}
			public void UnregisterPageLoadedCallback(CallbackPageLoaded callback) {
				callbackPageLoaded -= callback;
			}

			public void Animate(bool state) {
				string task = "AwaitAnimation";
				// animation start
				if (animationEnable) {
					// activate animation
					animator.SetBool("on", state);

					// async animation on a thread to run task AwaitAnimation()
					StopCoroutine(task);
					StartCoroutine(task, state);
					log.LogInfo(
						message: $"{Type} Page animation started.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}

				// animation end
				else {
					Active = false;
				}
			}

			#endregion

			#region Private Functions
			private IEnumerator AwaitAnimation(bool state) {
				// set target state
				targetState = state ? ISwitch.On : ISwitch.Off;

				// wait until animation move to next stage
				while (animator.GetCurrentAnimatorStateInfo(0).IsName(TargetState.ToString())) {
					yield return null;
				}

				// wait until animation is complete at normalized time >= Max
				while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
					yield return null;
				}

				// enabling the page
				if (state) {
					Active = true;
				}
				else {
					Active = false;
				}


				if (Active == false && state == true) {
					// log
					log.LogInfo(
						message: $"Page {Type} failed to turn off, page is not active.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}
				else {
					// fire page loaded event
					callbackPageLoaded.Invoke(this);

					// log
					log.LogInfo(
						message: $"{Type} Page animation completed.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);
				}

				// reset target state
				TargetState = ISwitch.None;
			}

			public void CheckAnimatorIntegrity() {
				if (animationEnable) {

					animator = GetComponent<Animator>();

					if (!animator) {
						log.LogError(
							message: $"Animator integrity failed, animator is not in gameobject.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}
				}
			}
			#endregion
		}
	}
}