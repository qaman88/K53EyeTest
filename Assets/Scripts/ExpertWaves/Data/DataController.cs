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
			private DataStore<ScreeningData> store; 
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
					store.Data.Level += 1;
					store.Data.Score += 5;
					if (store.Data.HighScore < store.Data.Score) {
						store.Data.HighScore = store.Data.Score;
					}
					if (store.Data.HighLevel < count) {
						store.Data.HighLevel = count;
					}
				}
			}

			private void OnDestroy() {
				SaveData();
			}
			#endregion

			#region Public Functions
			public void LoadData() {
				store.LoadData();
				userDeviceStore.LoadData();
			}

			public void SaveData() {
				store.SaveData();
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

				store = new DataStore<ScreeningData>(new ScreeningData(), log);
				userDeviceStore = new DataStore<DeviceUserData>(new DeviceUserData("Expert", "Ngobeni"), log);
			}
			#endregion
		}
	}
}