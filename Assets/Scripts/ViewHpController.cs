using UnityEngine;
using System.Collections;

public class ViewHpController : MonoBehaviour {
	public GameObject NumberPrefab;
	public Vector3 NumverPos;
	public float RandomTimeMax = 0.5f;
	public float RandomDistMax = 2f;

	GameObject[] Counts;
	Vector3 OriginScale;
	float[] OldHp;
	float[] DamageTime;
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
		OldHp = new float[teamvalue];
		DamageTime = new float[teamvalue];
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
						Counts [i].transform.localScale = scl;
					}
					// 位置
					if (OldHp[i] != hp) {
						DamageTime[i] = 0;
					}
					DamageTime[i] += Time.deltaTime;
					float multi = Mathf.Min(1,(DamageTime[i] / RandomTimeMax));
					float ran = (Mathf.Max(0,(1-multi)-0.8f)) * RandomDistMax;
					Vector3 pos = Vector3.zero;
					pos.x = Random.Range (-ran, ran);
					pos.y = Random.Range (-ran, ran);
					Counts [i].transform.localPosition = NumverPos+pos;
					OldHp [i] = hp;
				}
			}
			Counts [i].SetActive (active);
		}
	}
}
