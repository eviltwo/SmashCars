using UnityEngine;
using System.Collections;

public class GUITextureButton : MonoBehaviour {
	bool push = false;
	void Update(){
		if (!push) {
			if (InputManager.Instance.getClick () == 1) {
				Vector2 mousepos = InputManager.Instance.getMousePosMult ();
				Vector2 mypos = transform.position;
				Vector2 scl = transform.localScale;
				if (mypos.x - scl.x / 2 < mousepos.x && mypos.x + scl.x / 2 > mousepos.x && mypos.y - scl.y / 2 < mousepos.y && mypos.y + scl.y / 2 > mousepos.y) {
					SendMessage ("OnPushButton", SendMessageOptions.DontRequireReceiver);
					push = true;
				}
			}
		} else {
			if (InputManager.Instance.getClick () == 0) {
				SendMessage ("OnReleaseButton", SendMessageOptions.DontRequireReceiver);
				push = false;
			} else {
				Vector2 mousepos = InputManager.Instance.getMousePosMult ();
				Vector2 mypos = transform.position;
				Vector2 scl = transform.localScale;
				if (mypos.x - scl.x / 2 > mousepos.x || mypos.x + scl.x / 2 < mousepos.x || mypos.y - scl.y / 2 > mousepos.y || mypos.y + scl.y / 2 < mousepos.y) {
					SendMessage ("OnOutButton", SendMessageOptions.DontRequireReceiver);
					push = false;
				}
			}
		}
	}
}
