using UnityEngine;
using System.Collections;

public class PlayerSelectManager : SingletonMonoBehaviour<PlayerSelectManager> {
	public GameObject MainGlobalObjectPrefab;
	public int FieldSelect = 0;
	public int FieldValue = 2;
	public Texture[] FieldImage;
	public string[] FieldName;
	public Texture[] InputImage;
	public int InputNumMax = 6;

	// Use this for initialization
	void Start () {
		// PlayerManagerがあるオブジェクトを削除・設置
		GameObject findobj = GameObject.Find ("MainGlobalObject");
		if (findobj) {
			Destroy (findobj.gameObject);
		}
		GameObject obj = (GameObject)Instantiate (MainGlobalObjectPrefab);
		obj.name = "MainGlobalObject";
		// PlayerManagerのセーブデータを取得
		loadPlayerData ();
	}

	// プレイヤーデータをロードする
	public void loadPlayerData(){
		if (PlayerPrefs.HasKey ("IsData")) {
			for (int i = 0; i < 4; i++) {
				FieldSelect = PlayerPrefs.GetInt ("Field_" + i);
				PlayerManager.Instance.Teams [i].InputNumber = PlayerPrefs.GetInt ("Input_" + i);
				PlayerManager.Instance.Teams [i].isBossUser = PlayerPrefs.GetInt ("BossUser_" + i) == 1;
				PlayerManager.Instance.Teams [i].isCamera = PlayerPrefs.GetInt ("IsCamera_" + i) == 1;
			}
		}
	}
	// プレイヤーデータをセーブする
	public void savePlayerData(){
		PlayerPrefs.SetInt ("IsData", 1);
		for (int i = 0; i < 4; i++) {
			PlayerPrefs.SetInt ("Field_" + i, FieldSelect);
			PlayerPrefs.SetInt ("Input_" + i, PlayerManager.Instance.Teams [i].InputNumber);
			if (PlayerManager.Instance.Teams [i].isBossUser) {
				PlayerPrefs.SetInt ("BossUser_" + i, 1);
			} else {
				PlayerPrefs.SetInt ("BossUser_" + i, 0);
			}
			if (PlayerManager.Instance.Teams [i].isCamera) {
				PlayerPrefs.SetInt ("IsCamera_" + i, 1);
			} else {
				PlayerPrefs.SetInt ("IsCamera_" + i, 0);
			}
		}
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
	public void nextInput(int team){
		int n = PlayerManager.Instance.Teams [team].InputNumber;
		n++;
		if (n > InputNumMax) {
			n = 1;
		}
		PlayerManager.Instance.Teams [team].InputNumber = n;
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
	public int getInputNum(int team){
		return PlayerManager.Instance.Teams [team].InputNumber;
	}
}
