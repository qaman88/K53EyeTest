using ExpertWaves.Log;
using ExpertWaves.Utility;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace ExpertWaves {
	namespace Data {
		public class DataStore<T> where T : IBaseData {

			#region Public Variables
			public T data;
			#endregion

			#region Private Variables
			public LogController log;
			#endregion

			#region Variables Properties
			public T Data { get => this.data; set => this.data = value; }

			public string LogFile { get; set; }

			public string LogFolder { get; set; }

			public string PersistentPath { get => Application.persistentDataPath; }

			public string LogFolderPath { get => CombinePath(PersistentPath, LogFolder); }

			public string LogFileFullPath { get => CombinePath(LogFolderPath, LogFile); }

			#endregion

			#region Public Functions
			public DataStore(T source, LogController logController, string fileName, string subFolder = null) {
				LogFile = fileName;
				LogFolder = subFolder == null ? Constant.LogFolder : subFolder;
				data = source;
				log = logController;
			}

			public void SaveData() {
				try {
					// notify before saving data
					data.onBeforeSave();

					if (!File.Exists(LogFileFullPath)) {
						CreateFile();
					}
					// 

					// save data to file
					string json = JsonConvert.SerializeObject(data);
					byte[] byteData = Encoding.ASCII.GetBytes(json);
					File.WriteAllBytes(LogFileFullPath, byteData);

					// log
					log.LogInfo(
						message: $"Data stored in {LogFileFullPath}.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name
					);

					// notify after saving data
					data.onAfterSave();
				}
				catch (Exception error) {
					// log
					log.LogWarn(
						message: $"Failed to save data, path: {LogFileFullPath}.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name,
						exception: error
					);
				}
			}

			public void LoadData() {

				// log
				log.LogInfo(
					message: $"Data load started.",
					classType: GetType().Name,
					classMethod: MethodBase.GetCurrentMethod().Name
				);

				// load data from json
				try {
					// notify before loading data
					data.onBeforeLoad();

					if (File.Exists(LogFileFullPath)) {
						// load data from file
						byte[] byteData = File.ReadAllBytes(LogFileFullPath);
						string deciphred = Encoding.ASCII.GetString(byteData);
						T loadedData = JsonConvert.DeserializeObject<T>(deciphred);

						// override T data with file data
						if (loadedData != null) {
							data = loadedData;
						}

						// log
						log.LogInfo(
							message: $"Data restored from {LogFileFullPath}.",
							classType: GetType().Name,
							classMethod: MethodBase.GetCurrentMethod().Name
						);
					}

					else {
						// notify load file not found
						data.onLoadFileNotFound();
					}

					// notify after loading data
					data.onAfterLoad();
				}
				catch (Exception error) {
					// log
					log.LogWarn(
						message: $"Failed to load data, filename {LogFileFullPath}",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name,
						exception: error
					);
				}
			}
			#endregion

			#region Private Functions			
			private string GetPersistentDataPath() {
//#if UNITY_EDITOR_WIN
				// Windows-specific
				string winPath = Application.persistentDataPath.Replace("/", "\\");
				return winPath;
/*#else
				// Unix-specific
				string unixPath = Application.persistentDataPath;
				return unixPath;
#endif*/
			}

			private string CombinePath(string left, string right) {
//#if UNITY_EDITOR_WIN
				// do Windows-specific stuff
				return $"{left}\\{right}";
/*#else
				// do Unix-specific stuff
				return $"{left}/{right}";
#endif*/
			}

			private void CreateDirectory() {
				try {
					DirectoryInfo info = Directory.CreateDirectory(LogFolderPath);
				}
				catch (Exception error) {
					// log
					log.LogWarn(
						message: $"Error occurred while creating a directory, path {LogFolderPath}.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name,
						exception: error
					);
				}
			}

			private void CreateFile() {
				try {
					// create path
					if (!Directory.Exists(LogFolderPath)) {
						CreateDirectory();
					}

					// create file
					if (!File.Exists(LogFolderPath)) {
						using FileStream stream = File.Create(LogFolderPath, 0, FileOptions.WriteThrough);
						stream.Close();
					}
				}
				catch (Exception error) {
					log.LogWarn(
						message: $"Error occurred while creating a file, path {LogFolderPath}.",
						classType: GetType().Name,
						classMethod: MethodBase.GetCurrentMethod().Name,
						exception: error
					);
				}
			}

			#endregion
		}
	}
}