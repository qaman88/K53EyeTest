# [Audio](https://docs.unity3d.com/Manual/Audio.html)

Unity’s Audio features include full 3D spatial sound, real-time mixing and mastering, 
hierarchies of mixers, snapshots, predefined effects and much more.

A game would be incomplete without some kind of audio, be it background music or 
sound effects. Unity’s audio system is flexible and powerful. It can import most 
standard audio file formats and has sophisticated features for playing sounds 
in 3D space, optionally with effects like echo and filtering applied. 
Unity can also record audio from any available microphone on a user’s machine 
for use during gameplay or for storage and transmission.

## Audio Components in Unity:

* Audio Clip
* Audio Source
* Audio Listener
* Audio Mixer
* Audio Effects
* Sound Settings
* Reverb Zones 

---

## Audio Sources and Listener

> [sourceX] --> [listener]  <-- [sourceY]

To simulate the effects of position, Unity requires sounds to originate from Audio Sources
attached to objects. The sounds emitted are then picked up by an Audio Listener3
attached to another object, most often the main camera.

Unity can then simulate the effects of a source’s distance and position from the listener 
object and play them to the user accordingly. 

The relative speed of the source and listener objects can also be used to simulate the 
Doppler Effect for added realism.

Unity can’t calculate echoes purely from scene  geometry but you can simulate them by 
adding Audio Filters  to objects.  In situations where objects can move in and out of 
a place with a strong echo, you can add a Reverb Zone to the scene. 

The Unity Audio Mixer allows you to mix various audio sources, apply effects to them, 
and perform mastering.

---

## Audio Assets

In the same way as other assets, simply by dragging the files into the Project panel. 
Importing an audio file creates an Audio Clip  which can then be dragged to an 
Audio Source or used from a script. The Audio Clip reference page has more details 
about the import options available for audio files.

Supported audio formats:

	* `.mp3` MPEG layer 3	
	* `.ogg` Ogg Vorbis	
	* `.wav` Microsoft Wave	
	* `.aiff / .aif`  Audio Interchange File Format	
	* `.mod` Ultimate Soundtracker module	
	* `.it` Impulse Tracker module	
	* `.s3m` Scream Tracker module	
	* `.xm` FastTracker 2 module	

When audio is encoded in Unity the main options for how it is stored on disk is either:

	* PCM, 
	* ADPCM 
	* Compressed

### Audio Clip

The AudioClips merely refer to the files containing the audio data and there are various 
combinations of options in the AudioClip importer that determine how the clips are 
loaded at runtime. 

Components of Audio Clip:

* Audio data
* Meta-Information

The game may access meta-information about the audio data via the AudioClip even before 
the actual audio data has been loaded.

Any Audio File imported into Unity is available from scripts  as an Audio Clip instance, 
which provides a way for a game runtime of the audio system to access the encoded audio data.

The default mode is Compressed, where the audio data is compressed with either Vorbis/MP3 
for standalone and mobile platforms, or HEVAG/XMA for PS Vita and Xbox One.

---

## Audio Recording

Unity can access the computer’s microphones from a script and create Audio Clips by 
direct recording. The Microphone class provides a straightforward API to find available 
microphones, query their capabilities and start and end a recording session. 
The script reference page for Microphone has further information and code samples for 
audio recording.

---

## Tracker Modules (Instruments)

Tracker Modules are essentially just packages of audio samples that have been modeled, 
arranged and sequenced programmatically. 

They essentially a real time audio data producer just like an instrument. As result, they
have low memory size and high sound quality regardless of device playing the sound.

Tracker Module files are similar to MIDI files in many ways. The tracks are scores that 
contain information about when to play the instruments, and at what pitch and volume and 
from this, the melody and rhythm of the original tune can be recreated. 

In contrast, tracker modules include high quality PCM samples that ensure a similar 
experience regardless of the audio hardware in use.

Supported Tracker ModulesFormats:

* `.it` Impulse Tracker module	
* `.s3m` Scream Tracker module	
* `.xm` FastTracker 2 module	

A single sound sample can be modified in pitch and volume (and other effects applied), 
so it essentially acts as an “instrument” which can play a tune without the overhead 
of recording the whole tune as a sample.

---

## [Audio Mixer](https://docs.unity3d.com/Manual/AudioMixerOverview.html)
```
[AudioMixer]{
	[Group1], ... , [GroupN], [MasterGroup] 
	AudioMixer1{[GroupA], ... , [GroupN], [MasterGroup] }
	...
	AudioMixerN{[GroupA], ... , [GroupN], [MasterGroup] }
}
```
The Unity Audio Mixer allows you to mix various audio sources, apply effects to them, 
and perform mastering.

The window displays the Audio Mixer which is basically a tree of Audio Mixer Groups.

An Audio Mixer group is essentially a mix of audio, a signal chain which allows you to 
apply volume attenuation and pitch correction;.

It allows you to insert effects that process the audio signal and change the parameters 
of the effects.

An Audio Mixer is an asset. You can create one or more Audio Mixer and have more than 
one active at any time. An Audio Mixer always contains a master group. Other groups 
can then be added to define the structure of the mixer.

### Common Mixer Usage:

* Volume attenuation
* Pitch correction
* Reverb zone
* DSP effects
* Audio mastering


### How it works

AudioMixers now sit between the AudioSource and the AudioListener in the audio signal
processing space and allow you to take the output signal from the AudioSource perform 
whatever routing and mixing operations they wish until finally all audio is output to 
the AudioListener and is heard from the speakers.

DSP effects and other audio mastering concepts can be applied to the audio signal as 
it is routed from the AudioSource to the AudioListener.

> [Clip] --> [AudioClip]--> [AudioSource] --> 
> 
> `[AudioMixer]{Mixers,Groups}` --> 
> 
> [AudioListener] --> [Speakers]

You route the output of an Audio Source  to a group within an Audio Mixer. 
The effects will then be applied to that signal.

The output of an Audio Mixer can be routed into any other group in any other Audio Mixer 
in a scene enabling you to chain up a number of Audio Mixers in a scene to produce 
complex routing, effect processing and snapshot applying.

* `Snapshots` can capture the settings of all the parameters in a group as a snapshot.
If you create a list of snapshots you can then transition between them in gameplay 
to create different moods or themes.

	* Snapshots capture the values of all of the parameters within the AudioMixer:

		* Volume
		* Pitch
		* Send Level
		* Wet Mix Level
		* Effect Parameters
		* SFX Reverb Effect

* `Ducking` allows you to alter the effect of one group based on what is happening in 
another group. An example might be to reduce the background ambient noise while 
something else is happening.

* View can disable the visibility of certain groups within a mixer and set this as a view.
You can then transition between views as required. Different views can be set up.

* Audio routing is the process of taking a number of input audio signals and outputting
one or more output signals. 

---

## Overview of Usage and API

The AudioMixer is a very self-contained asset with a streamlined API.

* Using the Snapshot and AudioGroup objects
* Transitioning Snapshots
* Blending Snapshot states
* Finding Groups
* Re-routing at Runtime00

--- 

## [Audio Reference](https://docs.unity3d.com/Manual/class-AudioClip.html)
* Audio Clip
* Audio Listener
* Audio Source
* Audio Mixer

* Audio Filters
	* Audio Low Pass Filter
	* Audio High Pass Filter
	* Audio Echo Filter
	* Audio Distortion Filter
	* Audio Reverb Filter
	* Audio Chorus Filter

* Audio Effects
	* Audio Low Pass Effect
	* Audio High Pass Effect
	* Audio Echo Effect
	* Audio Flange Effect
	* Audio Distortion Effect
	* Audio Normalize Effect
	* Audio Parametric Equalizer Effect
	* Audio Pitch Shifter Effect
	* Audio Chorus Effect
	* Audio Compressor Effect
	* Audio SFX Reverb Effect
	* Audio Low Pass Simple Effect
	* Audio High Pass Simple Effect

* Reverb Zones
* Microphone
* Audio Settings


---

## Application

* Audio Track Components
  * Audio Source
  * Audio Objects

* Audio Tracks
  * Track 1 --> Source Effects
  * Track 2 --> Source Music
  * Track 3 --> Source Voices


* Audio Tables
  * Track 1 --> {Effect 1, ..., Effect N} => PlayEffect(...)
  * Track 2 --> {Music  1, ..., Music  N} => PlayMusic(...)
  * Track 3 --> {Voice  1, ..., Voice  N} => PlayVoice(...)

* Audio Job - Coroutine
  * Audio Type --> Running Job
    * Job1 {Effect X, Track 1}
    * Job2 {Music  X, Track 2}
    * Job3 {Voice  X, Track 3}
