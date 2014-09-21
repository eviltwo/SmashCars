using UnityEngine;
using System.Collections;

public class PlayerSelectManager : SingletonMonoBehaviour<PlayerSelectManager> {
	public GameObject MainGlobalObjectPrefab;

	// Use this for initialization
	void Start () {
		// PlayerManagerがあるオブジェクトを削除・設置
		GameObject findobj = GameObject.Find ("MainGlobalObject");
		if (findobj) {
			Destroy (findobj.gameObject);
		}
		findobj = (GameObject)Instantiate (MainGlobalObjectPrefab);
		findobj.name = "MainGlobalObject";

	}
	
	public void setUser(int team, bool user){
		PlayerManager.Instance.Teams [team].isBossUser = user;
	}
}
