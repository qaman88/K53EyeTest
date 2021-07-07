using ExpertWaves.Data.Statistics;
using UnityEngine.Rendering;

namespace ExpertWaves {
	namespace Data  {
		public class ScreeningData: IBaseData {

			#region Public Variables
			#endregion

			#region Private Variables
			private float highScore;
			private float score;
			private float level;
			private int highLevel;
			#endregion

			#region Variables Properties
			public float HighScore {
				get => this.highScore;
				set => this.highScore = value;
			}

			public float Score {
				get => this.score;
				set => this.score = value;
			}

			public float Level {
				get => this.level;
				set => this.level = value;
			}

			public int HighLevel {
				get => this.highLevel;
				set => this.highLevel = value;
			}
			#endregion

			#region Public Functions
			public ScreeningData() {
				highScore = 0;
				score = 0;
				level = 0;
				HighLevel = 0;
			}

			public ScreeningData(float score, float highScore, float level, int highLevel) {
				this.highScore = highScore;
				this.score = score;
				this.level = level;
				this.highLevel = highLevel;
			}

			public void Reset() {
				score = 0;
				level = 0;
			}

			public override string ToString() {
				return $"level: {Level}, high level: { HighLevel}, score: {Score}, high score: {HighScore}";
			}

			public void onBeforeLoad() {
			}

			public void onAfterLoad() {
			}

			public void onBeforeSave() {
			}

			public void onAfterSave() {
			}

			public void onLoadFileNotFound() {

			}

			#endregion

			#region Private Functions
			#endregion
		}
	}
}