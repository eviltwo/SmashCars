using UnityEngine;
using System.Collections;

public class ReturnController : MonoBehaviour {

	public float ReturnTimeMax = 3.0f; 
	public string ReturnSceneName = "Demo";

	float returntime = 0;
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			returntime += Time.deltaTime;
			if (returntime >= ReturnTimeMax) {
				GameObject findobj = GameObject.Find ("MainGlobalObject");
				if (findobj) {
					Destroy (findobj.gameObject);
				}
				Application.LoadLevel (ReturnSceneName);
			}
		} else {
			returntime = 0;
		}
	}
}
