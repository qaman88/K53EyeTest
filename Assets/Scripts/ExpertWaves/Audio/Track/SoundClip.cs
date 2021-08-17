using ExpertWaves.Audio.Enum;
using UnityEngine;

namespace ExpertWaves {
	namespace Audio {
		namespace Track {
			[System.Serializable]
			public struct SoundClip {
				public IBackgroundType clipType;
				public AudioClip audioClip;
			}
		}
	}
}