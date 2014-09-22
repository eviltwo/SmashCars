using UnityEngine;
using System.Collections;

public class Button_FieldChange : MonoBehaviour {

	void OnPushButton(){
		PlayerSelectManager.Instance.nextFieldSelect ();
	}
}
