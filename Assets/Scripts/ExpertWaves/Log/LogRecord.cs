using ExpertWaves.Log.Enum;
using ExpertWaves.Utility;
using System;
using System.Reflection;

namespace ExpertWaves {
	namespace Log {
		public class LogRecord {

			//string consoleMessage = ;
			public string dateTimeUctNow;
			public string epochDateTimeUctNow;
			public string level;
			public string classType;
			public string classMethod;
			public string message;
			public string exception;

			public LogRecord(string message, ILogLevel level = ILogLevel.Info, string classType = null, string classMethod = null, Exception exception = null) {
				this.message = message;
				this.level = level.ToString();
				this.classType = classType ?? GetType().Name;
				this.classMethod = classMethod ?? MethodBase.GetCurrentMethod().Name;
				this.exception = exception == null ? "" : $"{exception.Source}\n\t{exception.Message}\n\t{exception.StackTrace}";
				this.dateTimeUctNow = Common.DateTimeEpochTtcNow().ToString();
				this.epochDateTimeUctNow = Common.EpochDateTimeNow().ToString();
			}
		}
	}
}