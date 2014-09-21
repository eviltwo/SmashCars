using UnityEngine;
using System.Collections;

public class Item_Heli : MonoBehaviour {

	public GameObject MissilePrefab;
	public float Dist = 5.0f;
	public int PlusValue = 2;
	public float MinusInterval = 0.02f;
	public float JustShotDist = 50.0f;
	public AudioClip ShootAudio;
	public GameObject ArrowPrefab;
	public float ArrowRotSpeed = 180;

	GameObject CarObj;
	GameObject Arrow;
	int TeamNum;
	int MyType = 0;
	float arrowrot = 0;
	CarController cController;
	ItemController iController;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		Arrow.SetActive (false);
		if (iController.getHaveItem () && iController.getItemType () == MyType) {
			checkShot ();
			if (cController.IsBoss) {
				Arrow.SetActive (true);
				viewArrow ();
			}
		}
	}

	void StartSet(int type){
		CarObj = transform.parent.gameObject;
		cController = CarObj.GetComponent<CarController> ();
		iController = CarObj.GetComponent<ItemController> ();
		TeamNum = cController.TeamNum;
		MyType = type;


		Arrow = (GameObject)Instantiate (ArrowPrefab);
		Arrow.transform.FindChild("Arrow").GetComponent<SpriteRenderer>().color = PlayerManager.Instance.getTeamData()[TeamNum].TeamColor;
		Arrow.transform.FindChild("Arrow").gameObject.layer = LayerMask.NameToLayer ("UI_" + TeamNum);
		Arrow.transform.parent = CarObj.transform;
		Arrow.SetActive (false);

	}

	// アイテムボタンが押されている
	void Use(){
		// 発射
		spawnMissile ();
		// 発射音
		AudioManager.Instance.playSE (ShootAudio, CarObj.transform.position);
		// アイテムを使用済みにする
		transform.parent.SendMessage ("deleteItem", SendMessageOptions.DontRequireReceiver);
	}

	// ヘリ発生機を生成
	void spawnMissile(){
		Vector3 vec = cController.getForward ();
		vec.y = 0;
		vec = vec / vec.magnitude;
		Vector3 pos = CarObj.transform.position + vec * Dist;
		GameObject heli = (GameObject)Instantiate (MissilePrefab);
		RaycastHit hit;
		LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
		if (Physics.Raycast (pos + Vector3.up * 1000f, Vector3.down, out hit, Mathf.Infinity, mask)) {
			heli.transform.position = hit.point;
		} else {
			heli.transform.position = pos;
		}
		heli.SendMessage ("StartSet", TeamNum, SendMessageOptions.DontRequireReceiver);
		heli.GetComponent<HeliCallController> ().MissileValue += PlusValue * iController.getItemLevel ();
		heli.GetComponent<HeliCallController> ().FallInterval -= MinusInterval * iController.getItemLevel ();
	}

	// 索敵・発射判定
	void checkShot(){
		Vector3 vec = cController.getForward ();
		vec.y = 0;
		vec = vec / vec.magnitude;
		Vector3 pos = CarObj.transform.position + vec * Dist;
		RaycastHit hit;
		LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
		if (Physics.Raycast (pos + Vector3.up * 1000f, Vector3.down, out hit, Mathf.Infinity, mask)) {
			pos = hit.point;
		} else {
			return;
		}

		GameObject[] enemies = iController.getFindEnemy ();
		for (int i = 0; i < enemies.Length; i++) {
			GameObject enemy = enemies [i];
			if (enemy) {
				if (Vector3.Distance (enemy.transform.position, pos) < JustShotDist) {
					CarObj.SendMessage ("justEnemy", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}

	// 発射方向表示
	void viewArrow(){
		arrowrot += ArrowRotSpeed * Time.deltaTime;
		arrowrot = arrowrot % 360;
		Vector3 vec = cController.getForward ();
		vec.y = 0;
		vec = vec / vec.magnitude;
		Vector3 pos = CarObj.transform.position + vec * Dist;
		RaycastHit hit;
		LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
		if (Physics.Raycast (pos + Vector3.up * 1000f, Vector3.down, out hit, Mathf.Infinity, mask)) {
			pos = hit.point;
			Arrow.transform.position = pos;
			Arrow.transform.eulerAngles = new Vector3 (0, arrowrot, 0);
		} else {
			Arrow.SetActive (false);
		}
	}
}
