using ExpertWaves.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ExpertWaves {
	namespace Log {
		public class LogBuffer {
			#region Private Variables
			private string filePath;
			private string fileName;
			private Queue<LogRecord> buffer;
			private bool active = Constant.Negative;
			private int bufferSizeLimit;
			#endregion

			#region Variables Properties
			public bool Active { get => this.active; set => this.active = value; }
			public string FilePath { get => this.filePath; set => this.filePath = value; }
			public string FileName { get => this.fileName; set => this.fileName = value; }
			public int BufferSizeLimit { get => this.bufferSizeLimit; set => this.bufferSizeLimit = value; }
			public Queue<LogRecord> Buffer { get => this.buffer; set => this.buffer =  value ; }
			public int Id { get; set; }
			#endregion

			#region Public Functions
			public LogBuffer(string filePath, string fileName, int bufferSize, int id) {
				FilePath = filePath;
				FileName = fileName;
				BufferSizeLimit = bufferSize;
				Buffer = new Queue<LogRecord>();
				Id = id;
			}

			public void Push(LogRecord log) {
				if (IsFull()) {
					Debug.LogError($"Buffer {Id} is full.");
					Active = Constant.Negative;
					Buffer.Enqueue(log);
				}
				else {
					Buffer.Enqueue(log);
				}
			}

			public void Pop() {
				Buffer.Dequeue();
			}

			public void Clear() {
				Buffer.Clear();
				Active = Constant.Positive;
			}

			public bool IsEmpty() {
				return buffer.Count == 0;
			}

			public int Size() {
				return Buffer.Count;
			}

			public bool IsFull() {
				return Buffer.Count > BufferSizeLimit - 1;
			}

			public void SaveToFile() {
				string filePathName = $"{filePath}\\{Common.EpochDateTimeUtcNow()}.Buffer{Id}.{fileName}.json";
				string json = JsonConvert.SerializeObject(Buffer);
				byte[] byteData = System.Text.Encoding.ASCII.GetBytes(json);
				File.WriteAllBytes(filePathName, byteData);
			}
			#endregion

			#region Private Functions
			#endregion

			#region Private Functions
			#endregion
		}
	}
}
