using ExpertWaves.Audio.Enum;
using UnityEngine;

namespace ExpertWaves {
	namespace Audio {
		namespace Track {
			[System.Serializable]
			public struct EffectClip {
				public IEffectType clipType;
				public AudioClip audioClip;
			}
		}
	}
}
