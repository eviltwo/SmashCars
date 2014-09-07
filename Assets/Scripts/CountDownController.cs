using UnityEngine;
using System.Collections;

public class CountDownController : MonoBehaviour {
	public GameObject NumberPrefab;

	GameObject[] Counts;
	// Use this for initialization
	void Start () {
		createCount ();
	}
	
	// Update is called once per frame
	void Update () {
		moveCountStatus ();
	}

	// カウントを生成
	void createCount(){
		int teamvalue = PlayerManager.Instance.getTeamData ().Length;
		Counts = new GameObject[teamvalue];
		for (int i = 0; i < teamvalue; i++) {
			GameObject screen = (GameObject)Instantiate (NumberPrefab);
			screen.transform.parent = transform;
			screen.layer = LayerMask.NameToLayer ("UI_" + i);
			screen.SetActive (false);
			Counts [i] = screen;
		}
	}

	// カウントの状態を変更
	void moveCountStatus(){
		for (int i = 0; i < Counts.Length; i++) {
			bool active = false;
			if (PlayerManager.Instance.getTeamData () [i].isCamera) {
				float count = WaitManager.Instance.getWaitTime (i);
				if (count > 0) {
					int value = Mathf.CeilToInt (count);
					Counts [i].guiText.text = value.ToString ();
					active = true;
				}
			}
			Counts [i].SetActive (active);
		}
	}
}
