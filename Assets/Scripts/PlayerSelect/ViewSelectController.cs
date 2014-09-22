using UnityEngine;
using System.Collections;

public class ViewSelectController : MonoBehaviour {
	public int TeamNum = 0;

	int nowselect = -1;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if (nowselect != PlayerSelectManager.Instance.getInputNum(TeamNum)) {
			nowselect = PlayerSelectManager.Instance.getInputNum(TeamNum);
			guiTexture.texture = PlayerSelectManager.Instance.InputImage [nowselect-1];
		}
	}
}
