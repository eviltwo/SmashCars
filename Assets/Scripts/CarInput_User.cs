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
		dirinput.x = Input.GetAxis ("Horizontal");
		dirinput.y = Input.GetAxis ("Vertical");
		cController.setInput (dirinput);

		if (dirinput.y > 0) {
			iController.useItem ();
		}
	}
}
