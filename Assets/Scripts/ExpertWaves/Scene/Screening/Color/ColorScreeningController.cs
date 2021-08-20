using ExpertWaves.Audio;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Log;
using ExpertWaves.Page.Enum;
using ExpertWaves.Scene.Enum;
using ExpertWaves.Scene.Screening.Enum;
using ExpertWaves.UserInput.Key;
using ExpertWaves.UserInput.Touch;
using ExpertWaves.Utility;
using ExpertWaves.Vibration;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace ExpertWaves {
	namespace Scene {
		namespace Screening {
			namespace Color {
				public class ColorScreeningController : MonoBehaviour {
					#region Public Variables
					public PageController pageController;
					public SceneController sceneController;
					public Button redButton;
					public Button greenButton;
					public Button blueButton;
					public Image carImage;
					public Image clockImage;
					public Slider timerBar;
					public TextMeshProUGUI gameOverScore;
					public TextMeshProUGUI gameOverRedScore;
					public TextMeshProUGUI gameOverGreenScore;
					public TextMeshProUGUI gameOverBlueScore;
					public Button restartButton;
					public Button menuButton;
					public float timeout = 2.0f;
					#endregion

					#region Private Variables
					private LogController log;
					private KeyInputController keyInput;
					private TouchInputController touchInput;
					private AudioController audioController;
					private VibrationController vibrationController;
					private ColorScreeningEngine engine;
					private IGameOverReason  gameOverReason = IGameOverReason.None;
					private Coroutine awaitTimeoutCoroutine;
					#endregion

					#region Variables Properties
					#endregion

					#region Unity Functions
					private void Awake() {
						Configure();
					}

					private void Start() {
						// user inputs event subscription
						touchInput.RaiseTouchInputEvent += OnSwipe;
						keyInput.SubscribeKeyPressListener(OnKeyPress);

						// scene manager onSceneLoaded subscription
						sceneController.SubscribeOnSceneLoaded(OnSceneLoaded);

						// log
						log.LogDebug(
							message: $"On Start.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}

					private void Update() {
					}

					private void OnDestroy() {
						// user inputs event unsubscription
						touchInput.RaiseTouchInputEvent -= OnSwipe;
						keyInput.UnsubscribeKeyPressListener(OnKeyPress);

						// scene manager onSceneLoaded unsubscription
						sceneController.UnsubscribeOnSceneLoaded(OnSceneLoaded);
					}
					#endregion

					#region Public Functions
					#endregion

					#region Private Functions		
					private void Configure() {
						// ensure log controller is defined
						if (!log) {
							log = LogController.instance;
						}
						// ensure key input controller is defined
						if (!keyInput) {
							keyInput = KeyInputController.instance;
						}

						// ensure touch input controller is defined
						if (!touchInput) {
							touchInput = TouchInputController.instance;
						}

						// ensure vibration controller is defined
						if (!vibrationController) {
							vibrationController = VibrationController.instance;
						}

						// ensure scene controller is defined
						if (!sceneController) {
							sceneController = SceneController.instance;
							sceneController.Log = log;

							if (!pageController) {
								pageController.Log = log;
								sceneController.pageController = pageController;
							}
						}

						// ensure audio controller is defined
						if (!audioController) {
							audioController = AudioController.instance;
						}

						// init engine
						engine = new ColorScreeningEngine();

						// config red button
						if (redButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"Red button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								OnGameNextLevel(IColorChoice.Red);
							});
							redButton.onClick = callback;
						}

						// config green button
						if (greenButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"Green button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								OnGameNextLevel(IColorChoice.Green);
							});
							greenButton.onClick = callback;
						}

						// config red button
						if (blueButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"Blue button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);
								OnGameNextLevel(IColorChoice.Blue);
							});
							blueButton.onClick = callback;
						}

						// config menu button
						if (menuButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								// play sound effect for item select
								audioController.PlayEffect(IEffectType.Select);

								log.LogInfo(
									message: $"Menu button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);

								// set game page active to allow access of game objects
								pageController.SetPageActiveState(IPageType.Game, true);
								pageController.SetPageActiveState(IPageType.GameOver, false);

								// load menu scene
								sceneController.IsLoadingScene = false;
								sceneController.LoadSceneOnPage(ISceneType.Menu, IPageType.Game);
							});
							menuButton.onClick = callback;
						}

						// config restart button
						if (restartButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
								log.LogInfo(
									message: $"Restart button is clicked.",
									classType: GetType().Name,
									classMethod: MethodBase.GetCurrentMethod().Name
								);

								// set game page active to allow access of game objects
								pageController.SetPageActiveState(IPageType.Game, true);
								pageController.SetPageActiveState(IPageType.GameOver, false);

								// restart the game
								OnGameRestart();
							});
							restartButton.onClick = callback;
						}
					}

					private void OnGameOver() {
						// log
						log.LogInfo(
							message: $"Game over. Score: {engine.Score}, Level: {engine.Level}, GameOverReason: {gameOverReason}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// game over on incorrect answer or end of levels
						engine.GameOver = true;

						// set the score for game over page
						gameOverScore.text = $"{engine.Score}%";
						gameOverRedScore.text = $"{engine.RedScore}%";
						gameOverGreenScore.text = $"{engine.GreenScore}%";
						gameOverBlueScore.text = $"{engine.BlueScore}%";

						// switch to game over page
						pageController.LoadPage(IPageType.GameOver);

						// log
						log.LogInfo(
							message: $"Switch to {IPageType.GameOver} page.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}

					private void OnGameRestart() {
						// log
						log.LogInfo(
							message: $"Switch to {IPageType.Game} page.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// switch the page to game page
						pageController.SwitchPage(IPageType.Game);

						// restart game engine
						engine.Restart();
						gameOverReason = IGameOverReason.None;

						// change car color
						carImage.color = engine.CurrentColor;

						// set game over score
						gameOverScore.text = $"{engine.Score}%";

						// stop previous task
						if (awaitTimeoutCoroutine != null) {
							StopCoroutine(awaitTimeoutCoroutine);
						}

						// update inverted timer bar
						timerBar.value = 0.0f;

						// log
						log.LogInfo(
							message: $"Game restarted. EngineGameOver: {engine.GameOver}. GameOverReason: {gameOverReason}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

					}

					private void OnGameNextLevel(IColorChoice colorChoice) {
						if (engine.GameOver == Constant.Negative) {
							// play audio effect
							audioController.PlayEffect(engine.Answer == colorChoice ? IEffectType.Success : IEffectType.Failure);

							// move engine to next level
							engine.NextLevel(colorChoice);

							// change car color
							carImage.color = engine.CurrentColor;

							// set game over score
							gameOverScore.text = $"{engine.Score}%";

							// start timeout coroutine
							awaitTimeoutCoroutine = StartCoroutine(AwaitTimout(engine.Level));

							// log
							log.LogInfo(
								message: $"Next Level {engine.Level}, Engine Answer {engine.Answer}, User Choice: {colorChoice}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);
						}

						else if (engine.Level == engine.MaxLevel) {
							// play sound effect for game over in completed levels event
							audioController.PlayEffect(IEffectType.Success);

							// move engine to next level
							engine.NextLevel(colorChoice);

							// log
							log.LogInfo(
								message: $"Gameover by completed levels, Level {engine.Level}, " +
													$"Engine Answer {engine.Answer}, Choice: {colorChoice}, GameOver {engine.GameOver}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// set engine game over
							gameOverReason = IGameOverReason.LevelsComplete;
							OnGameOver();
						}

						else {
							// play sound effect for game over
							audioController.PlayEffect(IEffectType.Failure);

							// log
							log.LogInfo(
								message: $"Gameover by unknown reason, Level {engine.Level}, " +
													$"Engine Answer {engine.Answer}, Choice: {colorChoice}, GameOverResason {gameOverReason}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// set engine game over
							engine.GameOver = Constant.Positive;
							gameOverReason = IGameOverReason.None;
							OnGameOver();
						}
					}

					public IEnumerator AwaitTimout(int level) {
						// stop previous task
						if (awaitTimeoutCoroutine != null) {
							StopCoroutine(awaitTimeoutCoroutine);
						}

						int N = 100;
						float step = timeout / N;
						for (int i = 0 ; i < N && engine.Level == level ; i++) {
							yield return new WaitForSeconds(step);
							timerBar.value = 100.0f * step * i / timeout;
						}

						if (engine.Level == level && engine.GameOver == Constant.Negative) {
							// play sound effect for game over in timeout event
							audioController.PlayEffect(IEffectType.Warning);

							// log
							log.LogInfo(
								message: $"Gameover by timeout, Level {engine.Level}, " +
													$"Engine Answer {engine.Answer}, GameOverReasom {gameOverReason}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// set engine game over
							engine.GameOver = Constant.Positive;
							gameOverReason = IGameOverReason.Timeout;

							// update inverted timer bar
							timerBar.value = 100.0f;
							OnGameOver();
						}

						yield return null;
					}

					private void OnSwipe(object sender, TouchInputEvent e) {

					}

					private void OnKeyPress(KeyCode key) {
						// log
						log.LogDebug(
							message: $"Key press {key}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);


						switch (key) {
							case KeyCode.W:
								break;

							case KeyCode.A:
								break;

							case KeyCode.D:
								break;

							case KeyCode.S:
								break;

							default:
								break;
						}
					}

					private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene) {
						log.LogInfo(
							message: $"Scene loaded event, scene: {scene.name}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}
					#endregion
				}
			}
		}
	}
}