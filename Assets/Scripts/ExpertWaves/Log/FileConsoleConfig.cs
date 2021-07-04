using log4net.Appender;
using log4net.Layout;
using static log4net.Appender.RollingFileAppender;

namespace ExpertWaves {
	namespace Log {
		public class FileConsoleConfig {
			#region Private Variables
			private ConsoleAppender consoleAppender;
			private RollingFileAppender fileAppender;
			private string rollingFilename;
		
			#endregion

			#region Public Variables
			#endregion

			#region Variables Properties
			public ConsoleAppender ConsoleAppender { get => this.consoleAppender; set => this.consoleAppender = value; }

			public RollingFileAppender FileAppender { get => this.fileAppender; set => this.fileAppender = value; }
			#endregion


			#region Public Functions
			public FileConsoleConfig(string filename) {
				rollingFilename = filename;
			}

			public void ConfigConsoleAppender(string pattern) {
				// defining the pattern layout
				PatternLayout patternLayout = new PatternLayout();
				patternLayout.ConversionPattern = pattern;
				patternLayout.ActivateOptions();

				// creating the regular console appender
				consoleAppender = new ConsoleAppender();
				consoleAppender.Layout = patternLayout;
				consoleAppender.ActivateOptions();
			}
			public void ConfigFileAppender(string pattern) {
				// defining the pattern layout
				PatternLayout patternLayout = new PatternLayout();
				patternLayout.ConversionPattern = pattern;
				patternLayout.ActivateOptions();

				// creating the RollingLogFileAppender appender
				fileAppender = new RollingFileAppender();
				fileAppender.File = rollingFilename;
				fileAppender.Layout = patternLayout;
				fileAppender.AppendToFile = true;
				fileAppender.RollingStyle = RollingMode.Date;
				fileAppender.PreserveLogFileNameExtension = true;
				fileAppender.ActivateOptions();
			}

			public void ChangePatternLayout(string conversionPattern) {
				// defining the pattern layout
				PatternLayout patternLayout = new PatternLayout();
				patternLayout.ConversionPattern = conversionPattern;
				patternLayout.ActivateOptions();
				if (consoleAppender != null) {
					consoleAppender.Layout = patternLayout;
					consoleAppender.ActivateOptions();
				}

				if (fileAppender != null) {
					fileAppender.Layout = patternLayout;
					fileAppender.ActivateOptions();
				}
			}
			#endregion
		}
	}
}