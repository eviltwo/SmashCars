using UnityEngine;
using System.Collections;

public class GameTimeController : MonoBehaviour {

	public float SingleModeHeight = 0.8f;
	public string OverTimeText = "サドンデス";
	public Vector3 OverTimeScale;
	public string GameEndText = "ESC長押し";
	public Vector3 GameEndScale;

	GameObject Text;
	int screen = -1;
	// Use this for initialization
	void Start () {
		Text = transform.FindChild ("Text").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		float GameTime = GameTimeManager.Instance.getTime ();
		if (PlayerManager.Instance.isGameEnd) {
			Text.guiText.text = GameEndText;
			Text.transform.localScale = GameEndScale;
		}else if (GameTimeManager.Instance.isTimeOver()) {
			Text.guiText.text = OverTimeText;
			Text.transform.localScale = OverTimeScale;
		}else{
			int m = Mathf.FloorToInt (GameTime / 60);
			int s = Mathf.FloorToInt (GameTime % 60);
			string ss = "";
			if (s < 10) {
				ss = "0";
			}
			Text.guiText.text = m + ":" + ss + s;
		}
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
