using ExpertWaves.Utility;
using ExpertWaves.Enum;
using ExpertWaves.Log;
using ExpertWaves.UserInput;
using System.Collections;
using UnityEngine;

public class PageController : MonoBehaviour {
	#region Public Variables
	public static PageController instance;
	public LogController log;
	public InputController inputController;
	public IPageType entryPage;
	public Page[] pages;
	#endregion

	#region Private Variables
	private Hashtable register;
	private Page currentPage;

	#endregion

	#region Variables Properties
	#endregion

	#region Unity Functions
	private void Awake() {
		Debug.Log("Page Controller is awake!");

		// configure instance
		Configure();

		// load entry page is not none
		if (entryPage != IPageType.None) {
			LoadPage(entryPage);
		}

		currentPage = GetPage(IPageType.None);
		// register user input listeners
#if UNITY_EDITOR
		inputController.RegisterKeyPressListener(onKeyPress);
#endif
		inputController.RegisterSwipeListener(onSwipe);
	}
	private void OnDestroy() {
#if UNITY_EDITOR
		inputController.UnregisterKeyPressListener(onKeyPress);
#endif
		inputController.UnregisterSwipeListener(onSwipe);
	}
	#endregion

	#region Event Delegate Functions
	private void onKeyPress(KeyCode key) {
		Debug.Log($"Key Press Event, Key: {key.ToString()}");
		log.LogDebug(
			message: $"Key press event, key code: {key}.",
			classType: "PageController",
			classMethod: "onKeyPress"
		);

#if UNITY_EDITOR
		switch (key) {
			case KeyCode.W:
				Debug.Log("$$$ Keypress W");
				SwichPage(currentPage.Type, IPageType.Loading);
				break;

			case KeyCode.A:
				Debug.Log("$$$ Keypress A");
				SwichPage(currentPage.Type, IPageType.About);
				break;

			case KeyCode.D:
				Debug.Log("$$$ Keypress D");
				SwichPage(currentPage.Type, IPageType.Menu);
				break;

			case KeyCode.S:
				Debug.Log("$$$ Keypress S");
				SwichPage(currentPage.Type, IPageType.Privacy);
				break;

			default:
				break;
		}
#endif
	}

	private void onSwipe(IDirection type) {
		Debug.Log($"### Swipe Event from InputController. Swipe: {type.ToString()}");
	}

	public void onPageLoaded(Page page) {
		if (page != currentPage) {
			// TODO deactivate current page
			EnableOrDisableGameObject(currentPage, Constant.SwitchOff);

			// update current page
			currentPage = page;

			// unregister onPageLoaded event
			page.UnregisterPageLoadedCallback(onPageLoaded);
		}

		// log
		log.LogDebug(
			message: $"{page.Type} page is now current page. Callback onPageLoaded is unregistered.",
			classType: "PageController",
			classMethod: "onPageLoaded"
		);
	}
	#endregion


	#region Public Functions
	public PageController GetInstance() {
		return this;
	}


	public void LoadPage(IPageType targetPage) {
		if (targetPage != IPageType.None && register.Contains(targetPage)) {
			// get page to load
			Page page = GetPage(targetPage);

			// activate page game object
			EnableOrDisableGameObject(page, Constant.SwitchOn);

			// register page loaded event
			page.RegisterPageLoadedCallback(onPageLoaded);

			// check page integrity
			page.CheckAnimatorIntegrity();

			// animate the page
			page.Animate(Constant.SwitchOn);

			// log
			log.LogDebug(
				message: $"Attempting to load {targetPage} page.",
				classType: "PageController",
				classMethod: "LoadPage"
			);
		}
		else {
			log.LogWarn(
				message: $"Failed to load {targetPage} page, it is none or not registered.",
				classType: "PageController",
				classMethod: "LoadPage"
			);
		}
	}

	public void UnloadPage(IPageType targetPage) {
		if (targetPage != IPageType.None && register.Contains(targetPage)) {
			Page page = GetPage(targetPage);

			// unload if is active
			if (page.gameObject.activeSelf) {
				page.CheckAnimatorIntegrity();
				page.Animate(Constant.SwitchOff);
				log.LogDebug(
					message: $"Attempting to unload {targetPage} page.",
					classType: "PageController",
					classMethod: "UnloadPage"
				);
			}
		}
		else {
			log.LogWarn(
				message: $"Failed to unload {targetPage} page, it is none or not registered.",
				classType: "PageController",
				classMethod: "UnloadPage"
			);
		}
	}
	public void SwichPage(IPageType offPageType, IPageType onPageType) {
		if (offPageType == onPageType) {
			log.LogInfo(
				message: $"Cannot switch loaded page, from {offPageType} to {onPageType} page.",
				classType: "PageController",
				classMethod: "SwichPage"
			);
			return;
		}

		// switch only if onPage is registered and not none type. 
		if (onPageType != IPageType.None && register.Contains(onPageType)) {
			// unload off page
			UnloadPage(offPageType);

			// async load onPage
			Page offPage = GetPage(onPageType);
			Page onPage = GetPage(onPageType);
			string task = "AwaitPageSwitch";
			StopCoroutine(task);
			StartCoroutine(AwaitPageSwitch(offPage, onPage));

			log.LogInfo(
				message: $"Async switching from {offPageType} to {onPageType} page.",
				classType: "PageController",
				classMethod: "SwichPage"
			);
		}
		else {
			log.LogWarn(
				message: $"Failed to switch from {offPageType} to {onPageType} page, cannot not switch to none or not registered.",
				classType: "PageController",
				classMethod: "SwichPage"
			);
		}
	}

	public Page GetPage(IPageType page) {
		if (register.Contains(page)) {
			return (Page) register[page];
		}

		log.LogWarn(
			message: $"Failed to get {page} page, it is not registered.",
			classType: "PageController",
			classMethod: "GetPage"
		);

		return null;
	}
	#endregion

	#region Private Functions
	private void Configure() {
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
			register = new Hashtable();
			RegisterAllPages();
		}
		else {
			Destroy(gameObject);
		}
	}

	private void EnableOrDisableGameObject(Page page, bool state) {
		// return if the gameObject is desired state
		if (page == null) {
			log.LogWarn($"Failed to enable or disable null page.",
				classType: "PageController",
				classMethod: "EnableOrDisableGameObject"
			);
			return;
		}

		// deactivate game object if is active
		if (page.Active) {
			// deactivate the game object of the page
			page.Active = state;

			// log
			log.LogDebug(
				message: $"{page.Type} page gameobject is active, target state is {state}. Activating gameOject of {page.Type}.",
				classType: "PageController",
				classMethod: "EnableOrDisableGameObject"
			);
		}

		// activate game object if is inactive
		else {
			// activate the game object of the page
			page.Active = state;

			// log
			log.LogDebug(
				message: $"{page.Type} page gameobject is inactive, target state is {state}. Activating gameOject of {page.Type}.",
				classType: "PageController",
				classMethod: "ActivateGameObject"
			);
		}
	}

	private void RegisterPage(Page page) {
		if (!register.Contains(page)) {
			register.Add(page.type, page);
			log.LogInfo(
				message: $"Registered new page called {page.Type} page.",
				classType: "PageController",
				classMethod: "RegisterPage"
			);
		}
		else {
			log.LogWarn(
				message: $"Failed to register {page.Type} page, it is already registered.",
				classType: "PageController",
				classMethod: "RegisterPage"
			);
		}
	}

	private void RegisterAllPages() {
		log.LogDebug(
			message: $"Registering all pages...",
			classType: "PageController",
			classMethod: "RegisterAllPages"
		);

		foreach (Page page in pages) {
			RegisterPage(page);
		}
	}

	private IEnumerator AwaitPageSwitch(Page offPage, Page onPage) {
		// wait for off page to complete unload.
		while (offPage.TargetState != IFlag.None) {
			yield return null;
		}

		// load onPage
		LoadPage(onPage.Type);

		log.LogInfo(
			message: $"Succeeded switching from {offPage.Type} to {onPage.Type} page.",
			classType: "PageController",
			classMethod: "AwaitPageSwitch"
		);
	}
	#endregion
}
