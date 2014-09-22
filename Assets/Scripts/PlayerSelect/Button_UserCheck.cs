using UnityEngine;
using System.Collections;

public class Button_UserCheck : MonoBehaviour {
	public int TeamNum;
	public float DisableAlpha = 0.1f;
	
	// Update is called once per frame
	void Update () {
		if (PlayerSelectManager.Instance.getIsUser (TeamNum)) {
			Color c = guiTexture.color;
			c.a = 1;
			guiTexture.color = c;
		} else {
			Color c = guiTexture.color;
			c.a = DisableAlpha;
			guiTexture.color = c;
		}
	}

	void OnPushButton(){
		if (PlayerSelectManager.Instance.getIsUser (TeamNum)) {
			PlayerSelectManager.Instance.setIsUser (TeamNum, false);
			PlayerSelectManager.Instance.setIsCamera (TeamNum, false);
		} else {
			PlayerSelectManager.Instance.setIsUser (TeamNum, true);
			PlayerSelectManager.Instance.setIsCamera (TeamNum, true);
		}
	}
}
