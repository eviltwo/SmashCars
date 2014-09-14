using UnityEngine;
using System.Collections;

public class ViewPlayerValueController : MonoBehaviour {
	public GameObject NumberPrefab;
	public float CenterDist = 0.4f;
	public float SideDist = 0.1f;
	public float SingleScreenHeight = 0.8f;

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
			GameObject back = count.transform.FindChild ("Back").gameObject;
			count.transform.parent = transform;
			//count.layer = LayerMask.NameToLayer ("UI_" + i);
			//count.SetActive (false);
			Color c = PlayerManager.Instance.getTeamData () [i].TeamColor*0.4f;
			c.a = 1;
			back.guiTexture.color = c;
			Counts [i] = count;
			OriginScale = count.transform.localScale;

			// 位置
			Vector3 pos = new Vector3 ();
			float lefta = -1;
			if(i%2==1){
				lefta = 1;
			}
			float leftb = -1;
			if(Mathf.FloorToInt(i/2) == 1){
				leftb = 1;
			}
			pos.x = 0.5f + CenterDist * lefta + SideDist * leftb;
			pos.y = 0.5f;
			if (CameraManager.Instance.ScreenValue == 1) {
				pos.y = SingleScreenHeight;
			}
			pos.z = 1f;
			Counts [i].transform.localPosition = pos;
		}
	}

	// カウントの状態を変更
	void moveCountStatus(){
		Team[] teams = PlayerManager.Instance.getTeamData ();
		for (int i = 0; i < Counts.Length; i++) {
			int value = teams [i].PlayerValue;
			Counts [i].guiText.text = value.ToString ();
			// サイズ
			/*GameObject cam = CameraManager.Instance.getCamera (i);
			if (cam) {
				Rect rect = cam.camera.rect;
				float mlt = rect.height / rect.width;
				Vector3 scl = OriginScale;
				scl.x *= mlt;
				Counts [i].transform.localScale = scl;
			}*/
			// 位置
			Vector3 pos = Counts[i].transform.localPosition;
			if (CameraManager.Instance.ScreenValue == 1) {
				pos.y = SingleScreenHeight;
			}
			Counts [i].transform.localPosition = pos;
		}
	}
}
