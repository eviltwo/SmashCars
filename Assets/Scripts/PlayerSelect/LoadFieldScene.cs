using UnityEngine;
using System.Collections;

public class LoadFieldScene : MonoBehaviour {
	public string FieldName = "Field";

	void OnPushButton(){
		string loadname = FieldName + PlayerSelectManager.Instance.FieldSelect;
		PlayerSelectManager.Instance.savePlayerData ();
		Application.LoadLevel (loadname);
	}
}
