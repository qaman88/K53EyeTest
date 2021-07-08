using UnityEngine;

namespace ExpertWaves {
	namespace Audio {
		namespace Track {
			[System.Serializable]
			public struct VoiceClip {
				public Enum.IVoiceType clipType;
				public AudioClip audioClip;
			}
		}
	}
}
