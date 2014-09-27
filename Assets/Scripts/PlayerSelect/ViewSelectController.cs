using UnityEngine;
using System.Collections;

public class ViewSelectController : MonoBehaviour {
	public int TeamNum = 0;


	float InputMoveDist = 0.05f;
	float MoveMultiple = 0.1f;
	int nowselect = -1;
	Vector3 OldPos;
	Vector3 AddPos;
	// Use this for initialization
	void Start () {
		OldPos = transform.localPosition;
	}

	// Update is called once per frame
	void Update () {
		if (nowselect != PlayerSelectManager.Instance.getInputNum(TeamNum)) {
			nowselect = PlayerSelectManager.Instance.getInputNum(TeamNum);
			guiTexture.texture = PlayerSelectManager.Instance.InputImage [nowselect-1];
		}
		CheckInput ();
		transform.localPosition = OldPos + AddPos;
	}

	// 入力によって移動
	void CheckInput(){
		int inputnum = PlayerSelectManager.Instance.getInputNum (TeamNum);
		float horiax = Input.GetAxis ("Horizontal_" + inputnum);
		if (Mathf.Abs (horiax) > 0.05f) {
			AddPos.x = InputMoveDist * horiax;
		} else {
			AddPos.x *= MoveMultiple;
		}
		float vertax = Input.GetAxis ("Vertical_" + inputnum);
		if (Mathf.Abs (vertax) > 0.05f) {
			AddPos.y = InputMoveDist * vertax;
		} else {
			AddPos.y *= MoveMultiple;
		}
	}
}
