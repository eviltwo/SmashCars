using UnityEngine;
using System.Collections;

public class Button_ControlChange : MonoBehaviour {
	public int TeamNum = 0;
	void OnPushButton(){
		PlayerSelectManager.Instance.nextInput (TeamNum);
	}
}
