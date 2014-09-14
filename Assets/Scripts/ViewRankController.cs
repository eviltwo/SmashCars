using UnityEngine;
using System.Collections;

public class ViewRankController : MonoBehaviour {
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
			int rank = PlayerManager.Instance.WinNumbers [i];
			if (rank > 0) {
				Counts [i].guiText.text = rank+"位";
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
			Counts [i].SetActive (active);
		}
	}
}
