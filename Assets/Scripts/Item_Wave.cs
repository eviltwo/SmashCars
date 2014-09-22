using UnityEngine;
using System.Collections;

public class Item_Wave : MonoBehaviour {
	public GameObject WavePrefab;
	public AudioClip ShootAudio;

	GameObject CarObj;
	//GameObject Arrow;
	int TeamNum;
	int MyType = 0;
	CarController cController;
	ItemController iController;
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		//Arrow.SetActive (false);
		if (iController.getHaveItem () && iController.getItemType () == MyType) {
			checkShot ();
			/*if (cController.IsBoss) {
				Arrow.SetActive (true);
				viewArrow ();
			}*/
		}
	}

	void StartSet(int type){
		CarObj = transform.parent.gameObject;
		cController = CarObj.GetComponent<CarController> ();
		iController = CarObj.GetComponent<ItemController> ();
		TeamNum = cController.TeamNum;
		MyType = type;

		/*Arrow = (GameObject)Instantiate (ArrowPrefab);
		Arrow.transform.FindChild("Arrow").GetComponent<SpriteRenderer>().color = PlayerManager.Instance.getTeamData()[TeamNum].TeamColor;
		Arrow.transform.FindChild("Arrow").gameObject.layer = LayerMask.NameToLayer ("UI_" + TeamNum);
		Arrow.transform.parent = CarObj.transform;
		Arrow.SetActive (false);*/
	}

	// アイテムボタンが押されている
	void Use(){
		// 発射
		spawnWave ();
		// 発射音
		AudioManager.Instance.playSE (ShootAudio, CarObj.transform.position);
		// アイテムを使用済みにする
		transform.parent.SendMessage ("deleteItem", SendMessageOptions.DontRequireReceiver);
	}

	// 波紋発動
	void spawnWave(){
		GameObject wave = (GameObject)Instantiate (WavePrefab);
		wave.transform.position = CarObj.transform.position;
	}

	// 索敵・発射判定
	void checkShot(){

	}
}
