using UnityEngine;
using System.Collections;

public class ViewHpController : MonoBehaviour {
	public GameObject NumberPrefab;

	GameObject[] Counts;
	Vector3 OriginScale;
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
			OriginScale = count.transform.localScale;
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

					// サイズ
					GameObject cam = CameraManager.Instance.getCamera (i);
					if (cam) {
						Rect rect = cam.camera.rect;
						float mlt = rect.height / rect.width;
						Vector3 scl = OriginScale;
						scl.x *= mlt;
						Debug.Log (i+":"+rect);
						Counts [i].transform.localScale = scl;
					}
				}
			}
			Counts [i].SetActive (active);
		}
	}
}
