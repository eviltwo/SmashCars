using UnityEngine;
using System.Collections;

public class BillboardController : MonoBehaviour {
	public GameObject Prefab;
	public int MyNumber = -1;
	public bool IsBoss = false;
	public bool DisibleMyView = true;	// 自分のカメラに対して消すかどうか
	public float Height = 1.0f;

	int Value = 0;
	ArrayList Billboards = new ArrayList();
	// Update is called once per frame
	void Update () {
		// カメラが増えたならビルボード追加
		while (Value < CameraManager.Instance.getCameraValue ()) {
		
			createBillboard ();
			Value++;
		}

		// カメラを向く
		for (int i = 0; i < Value; i++) {
			GameObject cam = CameraManager.Instance.getCamera (i);
			GameObject board = (GameObject)Billboards [i];
			if (cam && board) {
				board.transform.LookAt (cam.transform);
			}
		}
	}

	void createBillboard(){
		GameObject board;
		if (DisibleMyView && MyNumber == Value && IsBoss) {
			board = null;
		} else {
			board = (GameObject)Instantiate (Prefab);
			board.transform.parent = transform;
			board.transform.localPosition = new Vector3(0,Height,0);
			int layer = LayerMask.NameToLayer ("UI_" + Value);
			setChildLayer (board, layer);
			SendMessage ("addBillboard", board);
			SendMessage ("addBillboardNum", Value);
		}
		Billboards.Add (board);
	}

	// 全ての子のレイヤーを変更
	void setChildLayer(GameObject obj, int layer){
		obj.layer = layer;
		for (int i = 0; i < obj.transform.childCount; i++) {
			GameObject child = obj.transform.GetChild (i).gameObject;
			setChildLayer (child, layer);
		}
	}


}
