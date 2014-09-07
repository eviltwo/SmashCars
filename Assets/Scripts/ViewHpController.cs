using UnityEngine;
using System.Collections;

public class ViewHpController : MonoBehaviour {
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
			GameObject count = (GameObject)Instantiate (NumberPrefab);
			count.transform.parent = transform;
			count.layer = LayerMask.NameToLayer ("UI_" + i);
			count.SetActive (false);
			count.guiText.color = PlayerManager.Instance.getTeamData () [i].TeamColor;
			Counts [i] = count;
		}
	}

	// カウントの状態を変更
	void moveCountStatus(){
		Team[] teams = PlayerManager.Instance.getTeamData ();
		for (int i = 0; i < Counts.Length; i++) {
			bool active = false;
			int boss = teams[i].BossNumber;
			if (boss >= 0) {
				if (teams [i].TeamPlayers [boss]) {
					float hp = teams [i].TeamPlayers [boss].GetComponent<CarController> ().HP;
					int value = Mathf.CeilToInt (hp);
					Counts [i].guiText.text = value.ToString ();
					active = true;
				}
			}
			Counts [i].SetActive (active);
		}
	}
}
