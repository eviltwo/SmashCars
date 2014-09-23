using UnityEngine;
using System.Collections;

public class RandomParticleTimer : MonoBehaviour {
	public float IntervalMin = 5.0f;
	public float InterValMax = 10.0f;
	public float EnableMin = 1.0f;
	public float EnableMax = 5.0f;

	float ivmax = 0;
	float ivtime = 0;
	float enmax = 0;
	float entime = 0;
	int Mode = 0;
	// Use this for initialization
	void Start () {
		setInterValTime ();
		setEnableTime ();
		setDisable ();
	}
	
	// Update is called once per frame
	void Update () {
		switch (Mode) {
		case 0:
			ivtime += Time.deltaTime;
			if (ivtime >= ivmax) {
				Mode = 1;
				setInterValTime ();
				setEnable ();
			}
			break;
		case 1:
			entime += Time.deltaTime;
			if (entime >= enmax) {
				Mode = 0;
				setEnableTime ();
				setDisable ();
			}
			break;
		}
	}

	void setInterValTime(){
		ivmax = Random.Range (IntervalMin, InterValMax);
		ivtime = 0;
	}

	void setEnableTime(){
		enmax = Random.Range (EnableMin, EnableMax);
		entime = 0;
	}

	void setDisable(){
		particleEmitter.emit = false;
	}
	void setEnable(){
		particleEmitter.emit = true;
	}
}
