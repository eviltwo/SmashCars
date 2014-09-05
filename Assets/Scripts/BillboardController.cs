using UnityEngine;
using System.Collections;

public class BillboardController : MonoBehaviour {
	public GameObject Prefab;

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
			if (cam) {
				board.transform.LookAt (cam.transform);
			}
		}
	}

	void createBillboard(){
		GameObject board = (GameObject)Instantiate (Prefab);
		board.transform.parent = transform;
		board.transform.localPosition = Vector3.zero;
		Billboards.Add (board);
		int layer = LayerMask.NameToLayer ("UI_" + (Billboards.Count - 1));
		setChildLayer (board, layer);
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
