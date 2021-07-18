using ExpertWaves.Audio;
using ExpertWaves.Audio.Enum;
using ExpertWaves.Enum;
using ExpertWaves.Log;
using ExpertWaves.Page.Enum;
using ExpertWaves.Scene.Enum;
using ExpertWaves.Scene.Screening.Distance;
using ExpertWaves.UserInput.Key;
using ExpertWaves.UserInput.Touch;
using ExpertWaves.Vibration;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

namespace ExpertWaves {
	namespace Scene {
		namespace Screening {
			namespace Distance {
				public class DistanceScreeningController :MonoBehaviour {
					#region Public Variables
					public Image gameOptotypeImage;
					public TextMeshProUGUI gameOverScore;
					public PageController pageController;
					public SceneController sceneController;
					public Engine engine;
					public Button restartButton;
					public Button menuButton;
					public Slider progressSlider;
					#endregion

					#region Private Variables
					private LogController log;
					private KeyInputController keyInput;
					private TouchInputController touchInput;
					private AudioController audioController;
					private VibrationController vibrationController;
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

						// restart game
						OnGameRestart();

						// log
						log.LogDebug(
							message: $"Game states. Gameover: {engine.Gameover}. Engine direction: {engine.Direction}.",
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

								// restart the game
								OnGameRestart();
							});
							restartButton.onClick = callback;
						}

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
						if (engine.Gameover == false && direction == engine.Direction) {
							// log
							log.LogDebug(
								message: $"Move to next level scope. Engine Direction: {engine.Direction}. Swipe: {direction}",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// move to the next level
							OnGameNextLevel();
						}
						else {
							// log
							log.LogDebug(
								message: $"Game over. EngineGameOver: {engine.Gameover}. Optotype Direction: {engine.Direction}. Swipe: {direction}",
								classType: GetType().Name,
								classMethod: MethodBase.GetCurrentMethod().Name
							);

							// switch to game over page
							OnGameOver();
						}
					}

					private void OnGameRestart() {
						// restart game engine
						engine.Restart();

						// reset optotype letter
						gameOptotypeImage.transform.localEulerAngles = new Vector3(0f, 0f, engine.Angle);
						gameOptotypeImage.transform.localScale = new Vector3(engine.OptotypeScale, engine.OptotypeScale, 1.0f);

						// set progress slider
						progressSlider.value = engine.Score / 100;

						// log
						log.LogInfo(
							message: $"Game restarted. EngineGameOver: {engine.Gameover}. Optotype Direction: {engine.Direction}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// switch the page to game page
						pageController.SwitchPage(IPageType.Game);

						// log
						log.LogInfo(
							message: $"Switch to {IPageType.Game} page.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// play sound effect
						audioController.PlayEffect(IEffectType.Update);
					}

					private void OnGameNextLevel() {
						// play sound effect for correct answer
						audioController.PlayEffect(IEffectType.Success);

						// game manager move to next level
						engine.NextLevel();

						// change angles
						gameOptotypeImage.transform.localEulerAngles = new Vector3(0f, 0f, engine.Angle);

						// change size
						gameOptotypeImage.transform.localScale = new Vector3(engine.OptotypeScale, engine.OptotypeScale, 1.0f);

						// set progress slider
						progressSlider.value = engine.Score / 100;

						// log
						log.LogInfo(
							message: $"Game next level. Score: {engine.Score}, Level: {engine.Level}, Optotype Direction: {engine.Direction}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}

					private void OnGameOver() {
						// log
						log.LogInfo(
							message: $"Game over. Score: {engine.Score}, Level: {engine.Level}, Optotype Direction: {engine.Direction}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);

						// play sound effect
						audioController.PlayEffect(IEffectType.Failure);

						// game over on incorrect answer or end of levels
						engine.Gameover = true;

						// set the score for game over page
						gameOverScore.text = $"{engine.Score}%";

						// switch to game over page
						pageController.LoadPage(IPageType.GameOver);

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