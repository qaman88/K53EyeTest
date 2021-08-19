using ExpertWaves.Data.Statistics;
using ExpertWaves.Log;
using UnityEngine;

namespace ExpertWaves {
	namespace Data {
		public partial class DataController : MonoBehaviour {
			#region Public Variables
			public static DataController instance;
			public LogController log;
			#endregion

			#region Private Variables
			private DataStore<ScreeningData> screeningStore; 
			DataStore<DeviceUserData> userDeviceStore;
			int count = 0;
			#endregion

			#region Unity Functions
			private void Awake() {
				Configure();
			}

			private void Start() {
				LoadData();
			}

			private void Update() {
				if (++count <= 20) {
					screeningStore.Data.Level += 1;
					screeningStore.Data.Score += 5;
					if (screeningStore.Data.HighScore < screeningStore.Data.Score) {
						screeningStore.Data.HighScore = screeningStore.Data.Score;
					}
					if (screeningStore.Data.HighLevel < count) {
						screeningStore.Data.HighLevel = count;
					}
				}
			}

			private void OnDestroy() {
				SaveData();
			}
			#endregion

			#region Public Functions
			public void LoadData() {
				screeningStore.LoadData();
				userDeviceStore.LoadData();
			}

			public void SaveData() {
				screeningStore.SaveData();
				userDeviceStore.SaveData();
			}
			#endregion

			#region Private Functions
			private void Configure() {
				// ensure instance is defined
				if (!instance) {
					instance = this;
					DontDestroyOnLoad(gameObject);
				}
				else {
					Destroy(gameObject);
				}

				// ensure log controller is defined
				if (!log) {
					log = LogController.instance;
				}

				screeningStore = new DataStore<ScreeningData>(new ScreeningData(), log, "Screening");
				userDeviceStore = new DataStore<DeviceUserData>(new DeviceUserData("Expert", "Ngobeni"), log, "DeviceData");
			}
			#endregion
		}
	}
}