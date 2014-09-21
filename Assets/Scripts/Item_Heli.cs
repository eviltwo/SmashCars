using UnityEngine;
using System.Collections;

public class Item_Heli : MonoBehaviour {

	public GameObject MissilePrefab;
	public float Dist = 5.0f;
	public float JustShotangle = 10.0f;
	public float JustShotDist = 80.0f;
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
		//Arrow.transform.parent = CarObj.transform;
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
			Arrow.transform.localEulerAngles = new Vector3 (0, arrowrot, 0);
		} else {
			Arrow.SetActive (false);
		}
	}
}
