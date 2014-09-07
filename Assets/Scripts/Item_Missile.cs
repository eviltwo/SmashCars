using UnityEngine;
using System.Collections;

public class Item_Missile : MonoBehaviour {

	public GameObject MissilePrefab;
	public int Value = 5;
	public int PlusValue = 1;
	public float Dist = 5.0f;
	public float MinusDist = 0.1f;
	public float JustShotangle = 10.0f;
	public float JustShotDist = 80.0f;

	GameObject CarObj;
	int TeamNum;
	int MyType = 0;
	CarController cController;
	ItemController iController;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (iController.getHaveItem() && iController.getItemType () == MyType) {
			checkShot ();
		}
	}

	void StartSet(int type){
		CarObj = transform.parent.gameObject;
		cController = CarObj.GetComponent<CarController> ();
		iController = CarObj.GetComponent<ItemController> ();
		TeamNum = cController.TeamNum;
		MyType = type;
	}

	// アイテムボタンが押されている
	void Use(){
		// 発射
		spawnMissile ();

		// アイテムを使用済みにする
		transform.parent.SendMessage ("deleteItem", SendMessageOptions.DontRequireReceiver);
	}

	// ミサイル生成
	void spawnMissile(){
		int level = iController.getItemLevel ()-1;
		int missilevalue = Value + PlusValue * level;
		float dist = Dist - MinusDist * level;
		for (int i = 0; i < missilevalue; i++) {
			GameObject missile = (GameObject)Instantiate (MissilePrefab);
			missile.transform.position = CarObj.transform.position;
			missile.transform.LookAt (missile.transform.position + cController.getForward());
			float basepos = i * dist;
			float pos = -dist * (missilevalue-1) / 2 + basepos;
			missile.transform.Rotate (0,pos,0);
			missile.transform.position += missile.transform.forward*1.5f;
			missile.SendMessage ("StartSet", TeamNum);
		}
	}

	// 索敵・発射判定
	void checkShot(){
		GameObject[] enemies = iController.getFindEnemy ();
		for (int i = 0; i < enemies.Length; i++) {
			GameObject enemy = enemies [i];
			Vector3 myvec = cController.getForward ();
			if (enemy) {
				Vector3 envec = (enemy.transform.position - CarObj.transform.position);
				envec = envec / envec.magnitude;
				float angle = Vector3.Angle (myvec, envec);
				if (angle < JustShotangle) {
					Vector3 stpos = CarObj.transform.position;
					Vector3 stdir = (enemy.transform.position - CarObj.transform.position);
					float dist = stdir.magnitude;
					stdir = stdir / dist;
					RaycastHit hit;
					LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
					if (dist < JustShotDist) {
						if (!Physics.Raycast (stpos, stdir, out hit, dist, mask)) {
							CarObj.SendMessage ("justEnemy", SendMessageOptions.DontRequireReceiver);
						}
					}
				}
			}
		}
	}
}
