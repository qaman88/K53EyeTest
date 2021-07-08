using ExpertWaves.Audio.Enum;
using UnityEngine;

namespace ExpertWaves {
	namespace Audio {
		namespace Track {
			[System.Serializable]
			public class EffectTrack : NoneTrack {
				public new readonly ITrackType track = ITrackType.Effect;
				public AudioSource source;
				public EffectClip[] clips;
			}
		}
	}
}