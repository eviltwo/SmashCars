using UnityEngine;
using System.Collections;

public class InputManager : SingletonMonoBehaviour<InputManager> {

	int Click = 0;
	Vector2 MousePos = new Vector2();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Application.platform == RuntimePlatform.Android) {
			// android
			clickAndroid ();
		} else {
			// pc
			clickPC ();
			retryPC ();
		}
	}

	void clickAndroid(){
		if (Input.touchCount > 0) {
			if (Click == 0) {
				Click = 1;
			} else if (Click == 1) {
				Click = 2;
			}
			MousePos = Input.touches [0].position;
		} else {
			Click = 0;
		}
	}

	void clickPC(){
		if (Input.GetKey (KeyCode.Mouse0) || Input.GetKey(KeyCode.Space)) {
			if (Click == 0) {
				Click = 1;
			} else if (Click == 1) {
				Click = 2;
			}
			MousePos = Input.mousePosition;
		} else {
			Click = 0;
		}
	}

	public int getClick(){
		return Click;
	}

	void retryPC(){
		if (Input.GetKeyDown (KeyCode.R)) {
			goRetry ();
		}
	}

	void goRetry(){
		Application.LoadLevel ("Main");
	}


	public void setRetry(){
		goRetry ();
	}

	public Vector2 getMousePos(){
		return MousePos;
	}

	public Vector2 getMousePosMult(){
		Vector2 pos = new Vector2 ();
		pos.x = MousePos.x / Screen.width;
		pos.y = MousePos.y / Screen.height;
		return pos;
	}
}
