using ExpertWaves.Audio.Enum;
using UnityEngine;

namespace ExpertWaves {
	namespace Audio {
		namespace Track {
			[System.Serializable]
			public class VoiceTrack : NoneTrack {
				public new readonly ITrackType track = ITrackType.Voice;
				public AudioSource source;
				public VoiceClip[] clips;
			}
		}
	}
}