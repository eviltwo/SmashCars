using UnityEngine;
using System.Collections;

public class CarInput_User : MonoBehaviour {

	Vector2 dirinput = Vector2.zero;

	CarController cController;
	ItemController iController;
	// Use this for initialization
	void Start () {
		cController = GetComponent<CarController> ();
		iController = GetComponent<ItemController> ();
	}
	
	// Update is called once per frame
	void Update () {
		int InputNum = cController.InputNum;
		dirinput.x = Input.GetAxis ("Horizontal_"+InputNum);
		dirinput.y = Input.GetAxis ("Vertical_"+InputNum);
		cController.setInput (dirinput);

		if (dirinput.y > 0) {
			iController.useItem ();
		}

		// ゲーム終了時、自動運転
		if (PlayerManager.Instance.isGameEnd) {
			GetComponent<CarInput_AI> ().enabled = true;
			this.enabled = false;

		}
	}
}
