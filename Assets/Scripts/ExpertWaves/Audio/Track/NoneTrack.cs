using ExpertWaves.Audio.Enum;

namespace ExpertWaves {
	namespace Audio {
		namespace Track {
			[System.Serializable]
			public class NoneTrack {
				public readonly ITrackType track = ITrackType.None;
			}
		}
	}
}
