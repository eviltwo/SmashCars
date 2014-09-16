using UnityEngine;
using System.Collections;

public class AudioPlayer : MonoBehaviour {

	public AudioClip Clip;
	public bool AutoDestroy = true;
	public bool Loop = false;
	public float Volume;
	public float Pan;

	bool start = false;
	
	// Update is called once per frame
	void Update () {
		if (!start) {
			if (audio.time > 0) {
				start = true;
			}
		} else {
			if (!Loop && AutoDestroy && audio.time == 0) {
				Destroy (this.gameObject);
			}
		}
	}

	public void setData(AudioClip clip, bool destroy, bool loop, float vol, float pan){
		Clip = clip;
		AutoDestroy = destroy;
		Loop = loop;
		Volume = vol;
		Pan = pan;
	}

	public void playAudio(){
		audio.clip = Clip;
		audio.volume = Volume;
		audio.loop = Loop;
		audio.pan = Pan;
		audio.Play ();
	}

}
