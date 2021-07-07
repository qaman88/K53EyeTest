using System;

namespace ExpertWaves {
	namespace Utility {
		public static class Constant {
			// Switch
			public static bool SwitchOn = true;
			public static bool SwitchOff = false;
			public static bool Enable = true;
			public static bool Disable = false;
			public static bool Active = true;
			public static bool Inactive = false;
			public static int LayerUI = 5;
			public static string LogFolder = "Logs";
			public static DateTime DateTimeZero;
			public static DateTime DateTimeNow => DateTime.Now;
			public static DateTime DateTimeEpochStart => new DateTime(1970, 1, 1, 0, 0, 0, 0); 
			public static TimeSpan TimeSpanEpochNow => DateTimeNow - DateTimeEpochStart;
			public static int EpochTimeNow => (int) TimeSpanEpochNow.TotalSeconds;
			public static int EpochTime => EpochTimeNow;
			public static int EpochTimeZero => 0;

			public static int DateTimeToEpoch(DateTime datetime) {
				TimeSpan span = datetime - DateTimeEpochStart;
				return (int) span.TotalSeconds;
			}

			public static DateTime EpochToDateTime(int epoch) {
				return DateTimeEpochStart.AddSeconds(epoch);
			}
		}
	}
}