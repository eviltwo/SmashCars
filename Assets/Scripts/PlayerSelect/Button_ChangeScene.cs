using UnityEngine;
using System.Collections;

public class Button_ChangeScene : MonoBehaviour {
	public string SceneName;
	void OnPushButton(){
		Application.LoadLevel (SceneName);
	}
}
