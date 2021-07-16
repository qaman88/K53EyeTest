using ExpertWaves.Data;
using ExpertWaves.Utility;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace ExpertWaves {
	namespace Log {
		public enum ILogBuffer {
			Buffer1,
			Buffer2
		}

		public class FileLogger {
			#region Private Variables
			private string filePath;
			private string fileName;
			private string filePathName;

			private LogBuffer buffer1;
			private LogBuffer buffer2;
			private ILogBuffer activeBuffer;
			#endregion

			#region Variables Properties
			public string FilePath {
				get => this.filePath; set => this.filePath = value;
			}
			public string FileName {
				get => this.fileName; set => this.fileName = value;
			}
			public string FilePathName {
				get => this.filePathName; set => this.filePathName = value;
			}
			#endregion

			#region Unity Functions

			#endregion

			#region Public Functions
			public FileLogger(string path, string file, int bufferSize) {
				FilePath = path; // $"{Constant.PersistentDataPath}\\Logs";
				FileName = file; // $"{Constant.ApplicationName.Replace(" ", "")}.log";
				if (Application.platform == RuntimePlatform.WindowsPlayer) {
					FilePathName = $"{FilePath}\\{FileName}";
				}
				else if (Application.platform == RuntimePlatform.Android) {
					FilePathName = $"{FilePath}/{FileName}";
				}

				if (!Directory.Exists(FilePath)) {
					Common.CreateDirectory(FilePath);
				}

				// crate log buffers and select active
				buffer1 = new LogBuffer(FilePath, FileName, bufferSize, 1);
				buffer1.Active = Constant.Positive;
				buffer2 = new LogBuffer(FilePath, FileName, bufferSize, 2);
				buffer2.Active = Constant.Negative;
				activeBuffer = ILogBuffer.Buffer1;
			}

			// If the method that the async keyword modifies doesn't contain an await expression or statement, the method executes synchronously. 
			public void Log(LogRecord log) {
				// select active buffer
				SelectBuffer();

				// log message using active buffer
				if (activeBuffer == ILogBuffer.Buffer1) {
					buffer1.Push(log);
				}
				else if (activeBuffer == ILogBuffer.Buffer2) {
					buffer2.Push(log);
				}

				// save full inactive buffer
				SaveFullBuffer();
			}

			public void SaveAllBuffer() {
				if (!buffer1.IsEmpty()) {
					buffer1.SaveToFile();
					buffer1.Clear();
				}
				if (!buffer2.IsEmpty()) {
					buffer2.SaveToFile();
					buffer2.Clear();
				}
			}
			#endregion

			#region Private Functions
			private void SelectBuffer() {
				if (!buffer1.Active && activeBuffer == ILogBuffer.Buffer1) {
					activeBuffer = ILogBuffer.Buffer2;
					buffer2.Active = Constant.Positive;
				}
				if (!buffer2.Active && activeBuffer == ILogBuffer.Buffer2) {
					activeBuffer = ILogBuffer.Buffer1;
					buffer1.Active = Constant.Positive;
				}
			}

			private void SaveFullBuffer() {
				if (buffer1.IsFull()) {
					buffer1.SaveToFile();
					buffer1.Clear();
				}
				else if (buffer2.IsFull()) {
					buffer2.SaveToFile();
					buffer2.Clear();
				}
			}
			#endregion
		}
	}
}
