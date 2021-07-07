using UnityEngine;
using log4net;
using System;
using ExpertWaves.Enum;
using System.Reflection;
using log4net.Repository.Hierarchy;
using log4net.Config;
using log4net.Core;

namespace ExpertWaves {
	namespace Log {
		public class LogController : MonoBehaviour {
			#region Public Variables
			public static LogController instance;
			public ILogLevel logLevel = ILogLevel.All;
			public string fileName = "K53EyeTest.log";
			public string filePath = "E:\\UnitySpace\\K53 Eye Test\\Logs";
			public bool fileLoggerEnable = true;
			public bool consoleLoggerEnable = true;
			public bool sqlServerLoggerEnable = true;
			#endregion

			#region Private Variables
			private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
			private SQLServerConfig sqlServerConfig;
			private FileConsoleConfig fileConsoleConfig;
			private Hierarchy hierarchy = (Hierarchy) LogManager.GetRepository();
			private string pattern = $"%utcdate [%-5level] [Thread%thread] %logger '%message' %exception %newline";
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

				// define template to define the log4net
				XmlConfigurator.Configure();
				fileConsoleConfig = new FileConsoleConfig($"{filePath}\\{fileName}");
				sqlServerConfig = new SQLServerConfig();

				// filter logs logging by log levels
				hierarchy.Threshold = logLevel.ToString() switch {
					"Off" => Level.Off,
					"Fatal" => Level.Fatal,
					"Error" => Level.Error,
					"Warn" => Level.Warn,
					"Info" => Level.Info,
					"Debug" => Level.Debug,
					_ => Level.All,
				};

				// config log appenders
				if (fileLoggerEnable) {
					fileConsoleConfig.ConfigConsoleAppender(pattern);
					hierarchy.Root.AddAppender(fileConsoleConfig.ConsoleAppender);
				}
				if (consoleLoggerEnable) {
					fileConsoleConfig.ConfigFileAppender(pattern);
					hierarchy.Root.AddAppender(fileConsoleConfig.FileAppender);
				}
				if (sqlServerLoggerEnable) {
					sqlServerConfig.ConfigureAdoNetAppender();
					hierarchy.Root.AddAppender(sqlServerConfig.AdoNetAppender);
				}

				// defining the level and finalizing the configuration
				hierarchy.Root.Level = Level.All;
				hierarchy.Configured = true;
			}

			private void LoggingFileConfigure(FileConsoleConfig config) {
				fileConsoleConfig = config;
				consoleLoggerEnable = true;
			}

			public void LoggingSQLServerConfigure(SQLServerConfig config) {
				sqlServerConfig = config;
				sqlServerLoggerEnable = true;
			}

			private void Log(string _message, ILogLevel _level, string _classType = null, string _classMethod = null, Exception _exception = null) {
				string message = _message;
				string exception = _exception != null ? $"| Expectation:%newline\t{_exception.Source}%newline\t{_exception.Message}%newline\t{_exception.StackTrace}" : "";
				string classType = _classType != null ? $"[{_classType}]" : "";
				string classMethod = _classMethod != null ? $"[{_classMethod}] " : "";

				//update layout pattern
				string newPattern = pattern.Replace("%logger", $"{classType}{classMethod}");
				fileConsoleConfig.ChangePatternLayout(newPattern);

				// change console and file appenders pattern
				if (fileLoggerEnable) {
					hierarchy.Root.AddAppender(fileConsoleConfig.ConsoleAppender);
				}
				if (consoleLoggerEnable) {
					hierarchy.Root.AddAppender(fileConsoleConfig.FileAppender);
				}

				// defining the level and finalizing the configuration
				hierarchy.Root.Level = Level.All;
				hierarchy.Configured = true;

				string consoleException = _exception == null ? "" : $"\n{_exception.Source}\n\t{_exception.Message}\n\t{_exception.StackTrace}";
				string consoleMessage = $"{classType}{classMethod} {message}";

				switch (_level.ToString()) {
					case "Fatal":
						log.Fatal(message);
						if (consoleLoggerEnable)
							Debug.LogError(consoleMessage);
						break;
					case "Error":
						log.Error(message);
						if (consoleLoggerEnable)
							Debug.LogError(consoleMessage);
						break;
					case "Warn":
						log.Warn(message);
						if (consoleLoggerEnable)
							Debug.LogWarning(consoleMessage);
						break;
					case "Info":
						log.Info(message);
						if (consoleLoggerEnable)
							Debug.Log(consoleMessage);
						break;
					case "Debug":
						log.Debug(message);
						if (consoleLoggerEnable)
							Debug.Log(consoleMessage);
						break;
					default:
						break;
				}
			}

			#endregion

			#region public Function
			public static LogController getInstance() {
				return instance;
			}

			public void LogFatal(string message, string classType = null, string classMethod = null, Exception exception = null) {
				Log(message, ILogLevel.Fatal, classType, classMethod, exception);
			}

			public void LogError(string message, string classType = null, string classMethod = null, Exception exception = null) {
				Log(message, ILogLevel.Error, classType, classMethod, exception);
			}

			public void LogWarn(string message, string classType = null, string classMethod = null, Exception exception = null) {
				Log(message, ILogLevel.Warn, classType, classMethod, exception);
			}
			public void LogInfo(string message, string classType = null, string classMethod = null, Exception exception = null) {
				Log(message, ILogLevel.Info, classType, classMethod, exception);
			}

			public void LogDebug(string message, string classType = null, string classMethod = null, Exception exception = null) {
				Log(message, ILogLevel.Debug, classType, classMethod, exception);
			}
			#endregion
		}
	}
}