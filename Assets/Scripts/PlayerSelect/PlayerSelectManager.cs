using UnityEngine;
using System.Collections;

public class PlayerSelectManager : SingletonMonoBehaviour<PlayerSelectManager> {
	public GameObject MainGlobalObjectPrefab;
	public int FieldSelect = 0;
	public int FieldValue = 2;
	public Texture[] FieldImage;
	public string[] FieldName;

	// Use this for initialization
	void Start () {
		// PlayerManagerがあるオブジェクトを削除・設置
		GameObject findobj = GameObject.Find ("MainGlobalObject");
		if (findobj) {
			Destroy (findobj.gameObject);
		}
		GameObject obj = (GameObject)Instantiate (MainGlobalObjectPrefab);
		obj.name = "MainGlobalObject";
	}
	
	public void setIsUser(int team, bool user){
		PlayerManager.Instance.Teams [team].isBossUser = user;
	}
	public void setIsCamera(int team, bool cam){
		PlayerManager.Instance.Teams [team].isCamera = cam;
	}
	public void nextFieldSelect(){
		FieldSelect++;
		if (FieldSelect >= FieldValue) {
			FieldSelect = 0;
		}
	}


	public Color getTeamColor(int team){
		return PlayerManager.Instance.Teams [team].TeamColor;
	}
	public bool getIsUser(int team){
		return PlayerManager.Instance.Teams [team].isBossUser;
	}
	public bool getIsCamera(int team){
		return PlayerManager.Instance.Teams [team].isCamera;
	}
}
