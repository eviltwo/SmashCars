using UnityEngine;
using System.Collections;

public class LoadFieldScene : MonoBehaviour {
	public string FieldName = "Field";
	public int FieldNum = 0;

	void OnPushButton(){
		string loadname = FieldName + FieldNum;
		Application.LoadLevel (loadname);
	}
}
