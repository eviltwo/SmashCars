using UnityEngine;
using System.Collections;

public class ViewFPS: MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	float TotalTime = 0;
	int TotalFrame = 0;
	float sfps = 60;
	void Update () {
		float fps = Mathf.Floor(1 / Time.deltaTime);

		TotalTime += Time.deltaTime;
		TotalFrame += 1;
		if (TotalTime >= 1.0f) {
			sfps = TotalFrame;
			TotalTime = 0;
			TotalFrame = 0;
		}

		guiText.text = "fps:" + sfps + ":" + fps + " /60";
	}
}
