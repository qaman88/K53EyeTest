using ExpertWaves.Utility;
using ExpertWaves.Enum;
using ExpertWaves.Page.Enum;
using ExpertWaves.Log;
using System.Collections;
using UnityEngine;
using System.Reflection;

public class PageController : MonoBehaviour {
	#region Public Variables
	public static PageController instance;
	public IPageType entryPage;
	public Page[] pages;
	#endregion

	#region Private Variables
	private LogController log;
	private Hashtable register;
	private Page currentPage;

	public Page CurrentPage {
		get => currentPage != null ? currentPage : GetPage(IPageType.None);
		set => currentPage = value;
	}

	public LogController Log {
		set => log = value;
	}

	#endregion

	#region Variables Properties
	#endregion

	#region Unity Functions
	private void Awake() {
		// configure instance
		Configure();
	}

	private void Start() {
		if (!log) {
			log = LogController.instance;
		}

		register = new Hashtable();
		RegisterAllPages();
		currentPage = GetPage(IPageType.None);

		// load entry page is not none
		if (entryPage != IPageType.None) {
			LoadPage(entryPage);
		}

		log.LogInfo(
			message: "PageController is started.",
			classType: GetType().Name,
			classMethod: MethodBase.GetCurrentMethod().Name
		);
	}
	#endregion

	#region Event Delegate Functions
	public void onPageLoaded(Page page) {
		if (page != CurrentPage) {
			// deactivate current page
			EnableOrDisablePage(CurrentPage, Constant.SwitchOff);

			// update current page
			CurrentPage = page;
		}

		// log
		log.LogDebug(
			message: $"{page.Type} page is now current page.",
			classType: GetType().Name,
			classMethod: MethodBase.GetCurrentMethod().Name
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
			EnableOrDisablePage(page, Constant.SwitchOn);

			// register page loaded event
			page.RegisterPageLoadedCallback(onPageLoaded);

			// check page integrity
			page.CheckAnimatorIntegrity();

			// animate the page
			page.Animate(Constant.SwitchOn);

			// log
			log.LogDebug(
				message: $"Attempting to load {targetPage} page.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
			);
		}
		else {
			log.LogWarn(
				message: $"Failed to load {targetPage} page, it is none or not registered.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
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
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
			}
		}
		else {
			log.LogWarn(
				message: $"Failed to unload {targetPage} page, it is none or not registered.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
			);
		}
	}

	public void SwitchPage(IPageType onPageType) {
		IPageType offPageType = currentPage.Type;

		if (currentPage.Type == onPageType) {
			log.LogInfo(
				message: $"Cannot switch loaded page, from {offPageType} to {onPageType} page.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
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
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
			);
		}
		else {
			log.LogWarn(
				message: $"Failed to switch from {offPageType} to {onPageType} page, cannot not switch to none or not registered.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
			);
		}
	}

	public Page GetPage(IPageType page) {
		if (register.Contains(page)) {
			return (Page) register[page];
		}

		log.LogWarn(
			message: $"Failed to get {page} page, it is not registered.",
			classType: GetType().Name,
			classMethod: MethodBase.GetCurrentMethod().Name
		);

		return null;
	}
	#endregion

	#region Private Functions
	private void Configure() {
		// ensure log controller is defined
		if (!log) {
			log = LogController.instance;
		}

		// ensure instance is defined
		if (!instance) {
			instance = this;
		}
		else {
			Destroy(gameObject);
		}
	}

	private void EnableOrDisablePage(Page page, bool state) {
		// return if the gameObject is desired state
		if (page == null) {
			log.LogWarn($"Failed to enable or disable null page.",
			classType: GetType().Name,
			classMethod: MethodBase.GetCurrentMethod().Name
			);
			return;
		}

		// deactivate game object if is active
		if (page.Active) {
			// deactivate the game object of the page
			page.Active = state;

			// log
			log.LogDebug(
				message: $"{page.Type} page gameobject is active, target state is {state}. Deactivating {page.Type} page.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
			);
		}

		// activate game object if is inactive
		else {
			// activate the game object of the page
			page.Active = state;

			// log
			log.LogDebug(
				message: $"{page.Type} page gameobject is inactive, target state is {state}. Activating {page.Type} page.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
			);
		}
	}

	private void RegisterPage(Page page) {
		if (!register.Contains(page)) {
			register.Add(page.type, page);
			log.LogInfo(
				message: $"Registered new page called {page.Type} page.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
			);
		}
		else {
			log.LogWarn(
				message: $"Failed to register {page.Type} page, it is already registered.",
				classType: GetType().Name,
				classMethod: MethodBase.GetCurrentMethod().Name
			);
		}
	}

	private void RegisterAllPages() {
		log.LogDebug(
			message: $"Registering all pages...",
			classType: GetType().Name,
			classMethod: MethodBase.GetCurrentMethod().Name
		);

		foreach (Page page in pages) {
			RegisterPage(page);
		}
	}

	private IEnumerator AwaitPageSwitch(Page offPage, Page onPage) {
		// wait for off page to complete unload.
		while (offPage.TargetState != ISwitch.None) {
			yield return null;
		}

		// load onPage
		LoadPage(onPage.Type);

		log.LogInfo(
			message: $"Succeeded switching from {offPage.Type} to {onPage.Type} page.",
			classType: GetType().Name,
			classMethod: MethodBase.GetCurrentMethod().Name
		);
	}
	#endregion
}
