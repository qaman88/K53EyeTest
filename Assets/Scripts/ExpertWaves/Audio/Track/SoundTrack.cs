using ExpertWaves.Audio.Enum;
using UnityEngine;

namespace ExpertWaves {
	namespace Audio {
		namespace Track {
			[System.Serializable]
			public class SoundTrack : NoneTrack {
				public new readonly ITrackType track = ITrackType.Sound;
				public AudioSource source;
				public SoundClip[] clips;
			}
		}
	}
}