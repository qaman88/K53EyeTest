using ExpertWaves.Audio.Enum;
using UnityEngine;

namespace ExpertWaves {
	namespace Audio {
		namespace Track {
			[System.Serializable]
			public class BackgroundTrack : NoneTrack {
				public new readonly ITrackType track = ITrackType.Background;
				public AudioSource source;
				public BackgroundClip[] clips;
			}
		}
	}
}