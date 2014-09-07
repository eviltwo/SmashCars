using UnityEngine;
using System.Collections;

public class BlackScreenController : MonoBehaviour {

	public GameObject BlackScreenPrefab;

	GameObject[] Screens;
	// Use this for initialization
	void Start () {
		createScreen ();
	}
	
	// Update is called once per frame
	void Update () {
		moveScreenStatus ();
	}

	// スクリーンを生成
	void createScreen(){
		int teamvalue = PlayerManager.Instance.getTeamData ().Length;
		Screens = new GameObject[teamvalue];
		for (int i = 0; i < teamvalue; i++) {
			GameObject screen = (GameObject)Instantiate (BlackScreenPrefab);
			screen.transform.parent = transform;
			screen.layer = LayerMask.NameToLayer ("UI_" + i);
			screen.SetActive (false);
			Screens [i] = screen;
		}
	}

	// スクリーンの状態を変更
	void moveScreenStatus(){
		for (int i = 0; i < Screens.Length; i++) {
			int boss = PlayerManager.Instance.getTeamData () [i].BossNumber;
			bool active = false;
			if (boss < 0 || !PlayerManager.Instance.getTeamData () [i].isCamera) {
				active = true;
			}
			Screens [i].SetActive (active);
		}
	}
}
