using ExpertWaves.Audio;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Enum;
using ExpertWaves.Log;
using ExpertWaves.Page.Enum;
using ExpertWaves.Scene.Enum;
using ExpertWaves.Scene.Screening.Enum;
using ExpertWaves.UserInput.Key;
using ExpertWaves.UserInput.Touch;
using ExpertWaves.Utility;
using ExpertWaves.Vibration;
using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace ExpertWaves {
	namespace Scene {
		namespace Screening {
			namespace Distance {
				public class DistanceScreeningController : MonoBehaviour {
					#region Public Variables
					public Image gameOptotypeImage;
					public TextMeshProUGUI gameOverScore;
					public PageController pageController;
					public SceneController sceneController;
					public Engine engine;
					public Button restartButton;
					public Button menuButton;
					public Slider timerBar;
					#endregion

					#region Private Variables
					private LogController log;
					private KeyInputController keyInput;
					private TouchInputController touchInput;
					private AudioController audioController;
					private VibrationController vibrationController;
					private IGameOverReason  gameOverReason = IGameOverReason.None;
					private Coroutine awaitTimeoutCoroutine;
					private float timeout = 2.0f;
					#endregion

					#region Public Functions
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
							message: $"Game states. Gameover: {engine.GameOver}. Engine direction: {engine.Direction}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}

					private void OnDestroy() {
						// user inputs event unsubscription
						touchInput.RaiseTouchInputEvent -= OnSwipe;
						keyInput.UnsubscribeKeyPressListener(OnKeyPress);

						// scene manager onSceneLoaded unsubscription
						sceneController.UnsubscribeOnSceneLoaded(OnSceneLoaded);
					}
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

						// init game engine
						engine = new Engine();

						// config menu button
						if (menuButton) {
							ButtonClickedEvent callback = new ButtonClickedEvent();
							callback.AddListener(() => {
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
								StartCoroutine(OnGameRestart());
							});
							restartButton.onClick = callback;
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
							log.LogInfo(
								message: $"Gameover by timeout, Level {engine.Level}, " +
													$"Engine Answer {engine.Direction}, GameOver reason {gameOverReason}.",
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

					private void OnKeyPress(KeyCode key) {
						// log
						log.LogDebug(
							message: $"Key press {key}",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						switch (key) {
							case KeyCode.W:
								OnGameUpdate(IDirection.Up);
								break;

							case KeyCode.A:
								OnGameUpdate(IDirection.Left);
								break;

							case KeyCode.D:
								OnGameUpdate(IDirection.Right);
								break;

							case KeyCode.S:
								OnGameUpdate(IDirection.Down);
								break;

							default:
								break;
						}
					}

					private void OnSwipe(object sender, TouchInputEvent e) {
						OnGameUpdate(e.Direction);
					}

					private void OnGameUpdate(IDirection direction) {
						if (engine.GameOver == false && direction == engine.Direction) {
							// log
							log.LogDebug(
								message: $"Move to next level scope. Engine Direction: {engine.Direction}. Swipe: {direction}",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// move to the next level
							OnGameNextLevel();
						}

						else if (direction != engine.Direction) {
							// log
							log.LogInfo(
								message: $"Gameover by incorrect answer, Level {engine.Level}, " +
													$"Engine Answer {engine.Direction}, Choice: {direction}, GameOver {engine.GameOver}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// set engine game over
							engine.GameOver = Constant.Positive;
							gameOverReason = IGameOverReason.IncorrectChoice;
							OnGameOver();
						}

						else if (engine.Level == engine.MaxLevel) {
							// log
							log.LogInfo(
								message: $"Gameover by completed levels, Level {engine.Level}, " +
													$"Engine Answer {engine.Direction}, Choice: {direction}, GameOver {engine.GameOver}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// set engine game over
							engine.GameOver = Constant.Positive;
							gameOverReason = IGameOverReason.LevelsComplete;
							OnGameOver();
						}

						else {
							// log
							log.LogInfo(
								message: $"Gameover by unknown reason, Level {engine.Level}, " +
													$"Engine Answer {engine.Direction}, Choice: {direction}, GameOverResason {gameOverReason}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// set engine game over
							engine.GameOver = Constant.Positive;
							gameOverReason = IGameOverReason.None;
							OnGameOver();
						}
					}


					private IEnumerator OnGameRestart() {
						try {
							// restart game engine
							engine.Restart();

							// reset optotype letter
							gameOptotypeImage.transform.localEulerAngles = new Vector3(0f, 0f, engine.Angle);
							gameOptotypeImage.transform.localScale = new Vector3(engine.OptotypeScale, engine.OptotypeScale, 1.0f);

							// log
							log.LogInfo(
								message: $"Game restarted. EngineGameOver: {engine.GameOver}. Optotype Direction: {engine.Direction}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// stop previous task
							if (awaitTimeoutCoroutine != null) {
								StopCoroutine(awaitTimeoutCoroutine);
							}

							// update inverted timer bar
							timerBar.value = 0.0f;

							// log
							log.LogInfo(
								message: $"Switch to {IPageType.Game} page.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// switch the page to game page
							pageController.SwitchPage(IPageType.Game);

							// play audio
							audioController.PlayVoice(IVoiceType.NewGame);
						}
						catch (Exception error) {
							log.LogError(
								message: $"Error occurred while restarting the game.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name,
								exception: error
							);
						}
						yield return null;
					}

					private void OnGameNextLevel() {
						// play sound effect for correct answer
						audioController.PlayEffect(IEffectType.Success);

						// game manager move to next level
						engine.NextLevel();

						// update inverted timer bar
						timerBar.value = 0.0f;

						// change angles
						gameOptotypeImage.transform.localEulerAngles = new Vector3(0f, 0f, engine.Angle);

						// change size
						gameOptotypeImage.transform.localScale = new Vector3(engine.OptotypeScale, engine.OptotypeScale, 1.0f);

						// start timeout coroutine
						awaitTimeoutCoroutine = StartCoroutine(AwaitTimout(engine.Level));

						// log
						log.LogInfo(
							message: $"Game next level. Score: {engine.Score}, Level: {engine.Level}, Optotype Direction: {engine.Direction}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						if (engine.Level == engine.MaxLevel) {
							// log
							log.LogInfo(
								message: $"Gameover by completed levels, Level {engine.Level}, " +
													$"Engine Answer {engine.Direction}.  GameOver {engine.GameOver}.",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// set engine game over
							engine.GameOver = Constant.Positive;
							gameOverReason = IGameOverReason.LevelsComplete;
							OnGameOver();
						}
					}

					private void OnGameOver() {
						// log
						log.LogInfo(
							message: $"Game over. Score: {engine.Score}, Level: {engine.Level}, Optotype Direction: {engine.Direction}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						vibrationController.Vibrate();

						// game over on incorrect answer or end of levels
						engine.GameOver = true;

						// set the score for game over page
						gameOverScore.text = $"{engine.Score}%";

						// switch to game over page
						pageController.LoadPage(IPageType.GameOver);

						// play voice sound
						float score =  engine.Score;
						if (score >= 50 && score < 60) {
							audioController.PlayVoice(IVoiceType.Good);
						}
						else if (score >= 60 && score < 70) {
							audioController.PlayVoice(IVoiceType.Wow);
						}
						else if (score >= 70 && score < 80) {
							audioController.PlayVoice(IVoiceType.Amazing);
						}
						else if (score >= 80 && score < 90) {
							audioController.PlayVoice(IVoiceType.Awesome);
						}
						else if (score >= 90) {
							audioController.PlayVoice(IVoiceType.Incredible);
						}
						else {
							audioController.PlayVoice(IVoiceType.GameOver);
						}

						// log
						log.LogInfo(
							message: $"Switch to {IPageType.GameOver} page.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}

					public void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene) {
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