using UnityEngine;
using System.Collections;

public class CountDownController : MonoBehaviour {
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
			Counts [i] = count;
			OriginScale = count.transform.localScale;
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

					// サイズ
					GameObject cam = CameraManager.Instance.getCamera (i);
					if (cam) {
						Rect rect = cam.camera.rect;
						float mlt = rect.height / rect.width;
						Vector3 scl = OriginScale;
						scl.x *= mlt;
						Counts [i].transform.localScale = scl;
					}
				}
			}
			Counts [i].SetActive (active);
		}
	}
}
