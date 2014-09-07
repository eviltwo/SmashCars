using UnityEngine;
using System.Collections;

public class GameTimeController : MonoBehaviour {
	public float GameTimeMax = 180;
	public float SingleModeHeight = 0.8f;

	float GameTime = 0;
	int screen = -1;
	// Use this for initialization
	void Start () {
		GameTime = GameTimeMax;
	}
	
	// Update is called once per frame
	void Update () {
		if (WaitManager.Instance.IsGameStart) {
			GameTime = Mathf.Max (0, GameTime - Time.deltaTime);
		}
		int m = Mathf.FloorToInt (GameTime / 60);
		int s = Mathf.FloorToInt (GameTime % 60);
		string ss = "";
		if (s < 10) {
			ss = "0";
		}
		guiText.text = m + ":" + ss + s;

		if (CameraManager.Instance.ScreenValue == 1) {
			if (screen != 1) {
				Vector3 pos = transform.localPosition;
				pos.y = SingleModeHeight;
				transform.localPosition = pos;
				screen = 1;
			}
		}
	}
}
