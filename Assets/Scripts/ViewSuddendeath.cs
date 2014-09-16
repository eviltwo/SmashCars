using UnityEngine;
using System.Collections;

public class ViewSuddendeath : MonoBehaviour {
	public GameObject ChildText;
	public float TimeMax = 5;
	public float MoveAngle = 30f;
	public float TextDistMax = 3;
	public AudioClip Audio;
	public float AudioVolume = 1;

	float movetime = 0;
	bool over = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (GameTimeManager.Instance.isTimeOver ()) {
			if (!over) {
				over = true;
				OverStart ();
			}

			movetime += Time.deltaTime;
			float mlt = movetime / TimeMax;
			Vector3 rot = transform.localEulerAngles;
			rot.y = -MoveAngle / 2 + (MoveAngle * mlt);
			transform.localEulerAngles = rot;
			Vector3 pos = ChildText.transform.localPosition;
			float ran = (Mathf.Max(0,(1-mlt)-0.8f)) * TextDistMax;
			pos.x = Random.Range (-ran, ran);
			pos.y = Random.Range (-ran, ran);
			ChildText.transform.localPosition = pos;

			if (movetime >= TimeMax) {
				Destroy (this.gameObject);
			}
		} else {
			renderer.enabled = false;
			ChildText.renderer.enabled = false;
		}
	}

	void OverStart(){
		renderer.enabled = true;
		ChildText.renderer.enabled = true;

		// 音
		AudioManager.Instance.playSE (Audio, AudioVolume);
	}
}
