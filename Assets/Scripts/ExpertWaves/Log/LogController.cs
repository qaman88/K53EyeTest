using ExpertWaves.Log.Enum;
using ExpertWaves.Utility;
using System.Reflection;
using System;
using UnityEngine;

namespace ExpertWaves {
	namespace Log {
		public class LogController : MonoBehaviour {
			#region Public Variables
			public static LogController instance;
			public ILogLevel logLevel = ILogLevel.All;
			public bool consoleLoggerEnable = true;
			public bool fileLoggerEnable = true;
			public int fileBufferSize = 2500;
			#endregion

			#region Private Variables
			private FileLogger fileLogger;
			#endregion

			#region Unity Function
			public void Awake() {
				Configure();
				LogInfo(
					message: "LogController is Awake.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);
			}

			private void Update() {
			}

			private void OnDestroy() {
				fileLogger.SaveAllBuffer();
			}
			#endregion

			#region Private Function
			private void Configure() {
				// ensure instance is defined
				if (!instance) {
					instance = this;
					DontDestroyOnLoad(gameObject);
				}
				else {
					Destroy(gameObject);
				}

				// config file logger
				string filePath = $"{Application.persistentDataPath}\\Logs";
				string fileName = $"{Application.productName.Replace(" ", "")}";
				fileLogger = new FileLogger(filePath, fileName, fileBufferSize);
			}

			private void ConsoleLog(string _message, ILogLevel _level, string _classType = null, string _classMethod = null, Exception _exception = null) {
				string message = _message;
				string classType = _classType != null ? _classType : MethodBase.GetCurrentMethod().Name;
				string classMethod = _classMethod != null ? _classMethod : GetType().Name;
				string consoleException = _exception == null ? "" : $"{_exception.Source}\n\t{_exception.Message}\n\t{_exception.StackTrace}";
				string consoleMessage = $"[{_level}] [{classType}.{classMethod}] {message} \n {consoleException}";
				switch (_level.ToString()) {
					case "Fatal":
						if (consoleLoggerEnable && ( logLevel == ILogLevel.All || (int) logLevel <= (int) ILogLevel.Fatal ))
							Debug.LogError(consoleMessage);
						break;
					case "Error":
						if (consoleLoggerEnable && ( logLevel == ILogLevel.All || (int) logLevel <= (int) ILogLevel.Error ))
							Debug.LogError(consoleMessage);
						break;
					case "Warn":
						if (consoleLoggerEnable && ( logLevel == ILogLevel.All || (int) logLevel <= (int) ILogLevel.Warn ))
							Debug.LogWarning(consoleMessage);
						break;
					case "Info":
						if (consoleLoggerEnable && ( logLevel == ILogLevel.All || (int) logLevel <= (int) ILogLevel.Info ))
							Debug.Log(consoleMessage);
						break;
					case "Debug":
						if (consoleLoggerEnable && ( logLevel == ILogLevel.All || (int) logLevel <= (int) ILogLevel.Debug ))
							Debug.Log(consoleMessage);
						break;
					default:
						break;
				}
			}

			private void FileLog(string message, ILogLevel level, string classType = null, string classMethod = null, Exception exception = null) {
				fileLogger.Log(new LogRecord(
					message,
					level,
					classType,
					classMethod,
					exception
				));
			}
			#endregion

			#region public Function
			public static LogController getInstance() {
				return instance;
			}

			public void LogFatal(string message, string classType = null, string classMethod = null, Exception exception = null) {
				ConsoleLog(message, ILogLevel.Fatal, classType, classMethod, exception);
				if (fileLoggerEnable) {
					FileLog(message, ILogLevel.Fatal, classType, classMethod, exception);
				}
			}

			public void LogError(string message, string classType = null, string classMethod = null, Exception exception = null) {
				ConsoleLog(message, ILogLevel.Error, classType, classMethod, exception);
				if (fileLoggerEnable) {
					FileLog(message, ILogLevel.Error, classType, classMethod, exception);
				}
			}

			public void LogWarn(string message, string classType = null, string classMethod = null, Exception exception = null) {
				ConsoleLog(message, ILogLevel.Warn, classType, classMethod, exception);
				if (fileLoggerEnable) {

					FileLog(message, ILogLevel.Warn, classType, classMethod, exception);
				}
			}

			public void LogInfo(string message, string classType = null, string classMethod = null, Exception exception = null) {
				ConsoleLog(message, ILogLevel.Info, classType, classMethod, exception);
				if (fileLoggerEnable) {
					FileLog(message, ILogLevel.Info, classType, classMethod, exception);
				}
			}

			public void LogDebug(string message, string classType = null, string classMethod = null, Exception exception = null) {
				ConsoleLog(message, ILogLevel.Debug, classType, classMethod, exception);
				if (fileLoggerEnable) {
					FileLog(message, ILogLevel.Debug, classType, classMethod, exception);
				}
			}
			#endregion
		}
	}
}