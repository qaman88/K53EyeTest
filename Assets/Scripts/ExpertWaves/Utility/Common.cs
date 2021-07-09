using System;
using System.IO;
using UnityEngine;

namespace ExpertWaves {
	namespace Utility {
		public static class Common {
			public static bool CreateDirectory(string path) {
				bool status = Constant.Negative;
				try {
					DirectoryInfo info = Directory.CreateDirectory(path);
					Debug.Log($"{DateTime.UtcNow} [Info] [Common.CreateDirectory] Created a directory, path {path}.");
					status = Constant.Positive;
				}
				catch (Exception error) {
					Debug.LogError($"{DateTime.UtcNow} [Info] [Common.CreateDirectory] Error occurred while creating a directory, path {path}.\n {error}");
				}
				return status;
			}

			public static bool CreateFile(string path, string file) {
				bool status = Constant.Negative;

				try {
					string fileFullPath = $"{path}\\{file}";
					// create path
					if (!Directory.Exists(path)) {
						CreateDirectory(path);
					}

					// create file
					if (!File.Exists(fileFullPath)) {
						using FileStream stream = File.Create(fileFullPath, 0, FileOptions.WriteThrough);
						stream.Close();
						status = Constant.Positive;
						Debug.Log($"{DateTimeEpochTtcNow()} [Info] [Common.CreateFile] Created a file, path {path}\\{file}.");
					}
				}
				catch (Exception error) {
					Debug.LogError($"{DateTimeEpochTtcNow()} [Error] [Common.CreateFile] Error occurred while creating a file, path {path}\\{file}. \n {error}");
				}

				return status;
			}

			public static DateTime DateTimeEpochNow() {
				return DateTime.Now;
			}
			public static DateTime DateTimeEpochTtcNow() {
				return DateTime.UtcNow;
			}

			public static DateTime DateTimeEpochStart() {
					return new DateTime(1970, 1, 1, 0, 0, 0, 0);
			}
			public static int EpochDateTimeZero() {
				return 0;
			}

			public static int EpochDateTimeNow() {
				TimeSpan timeSpanEpochNow = DateTime.Now - DateTimeEpochStart();
				return (int) timeSpanEpochNow.TotalSeconds;
			}
			public static int EpochDateTimeUtcNow() {
				TimeSpan timeSpanEpochUtcNow = DateTime.UtcNow - DateTimeEpochStart();
				return (int) timeSpanEpochUtcNow.TotalSeconds;
			}

			public static int DateTimeToEpoch(DateTime dateTime) {
				TimeSpan TimeSpanEpoch = dateTime - DateTimeEpochStart();
				return (int) TimeSpanEpoch.TotalSeconds;
			}

			public static DateTime EpochToDateTime(int epoch) {
				return DateTimeEpochStart().AddSeconds(epoch);
			}
		}
	}
}
