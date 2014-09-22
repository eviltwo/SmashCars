using UnityEngine;
using System.Collections;

public class Item_Wave : MonoBehaviour {
	public GameObject WavePrefab;
	public GameObject WaveWindPrefab;
	public float Dist = 10.0f;
	public float DistPlus = 0.5f;
	public float Damage = 16.0f;
	public float DamagePlus = 5.0f;
	public AudioClip ShootAudio;
	public GameObject ArrowPrefab;
	public int ArrowValue;
	public float ArrowRotSpeed = 90.0f;

	GameObject CarObj;
	GameObject[] Arrow;
	int TeamNum;
	int MyType = 0;
	float arrowrot;
	GameObject ArrowParent;
	CarController cController;
	ItemController iController;
	// Use this for initialization
	void Start () {
	
	}
	
	void Update () {
		ArrowParent.SetActive (false);
		if (iController.getHaveItem () && iController.getItemType () == MyType) {
			checkShot ();
			if (cController.IsBoss) {
				ArrowParent.SetActive (true);
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

		createArrow ();
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
		// エフェクト
		GameObject wave = (GameObject)Instantiate (WavePrefab);
		wave.transform.position = CarObj.transform.position;
		wave.SendMessage ("StartSet", TeamNum, SendMessageOptions.DontRequireReceiver);
		float dist = Dist + DistPlus * iController.getItemLevel ();
		wave.SendMessage ("SetDist", dist, SendMessageOptions.DontRequireReceiver);

		// 爆風
		GameObject wind = (GameObject)Instantiate (WaveWindPrefab);
		wind.transform.position = CarObj.transform.position;
		float dam = Damage + DamagePlus * iController.getItemLevel ();
		wind.GetComponent<BombWind> ().WindDamage = dam;
		wind.transform.localScale = new Vector3 (dist, dist, dist);
		wind.SendMessage ("StartSet", TeamNum, SendMessageOptions.DontRequireReceiver);
	}

	// 索敵・発射判定
	void checkShot(){
		GameObject[] enemies = iController.getFindEnemy ();
		for (int i = 0; i < enemies.Length; i++) {
			GameObject enemy = enemies [i];
			if (enemy) {
				float dist = Dist + DistPlus * iController.getItemLevel ();
				dist *= 1.2f;
				if (Vector3.Distance (enemy.transform.position, CarObj.transform.position) < dist) {
					CarObj.SendMessage ("justEnemy", SendMessageOptions.DontRequireReceiver);
				}
			}
		}
	}


	void createArrow(){
		ArrowParent = new GameObject ();
		ArrowParent.transform.parent = CarObj.transform;
		ArrowParent.transform.localPosition = Vector3.zero;
		Arrow = new GameObject[ArrowValue];
		for (int i = 0; i < ArrowValue; i++) {
			Arrow[i] = (GameObject)Instantiate (ArrowPrefab);
			Arrow[i].transform.FindChild ("Arrow").GetComponent<SpriteRenderer> ().color = PlayerManager.Instance.getTeamData () [TeamNum].TeamColor;
			Arrow[i].transform.FindChild ("Arrow").gameObject.layer = LayerMask.NameToLayer ("UI_" + TeamNum);
			Arrow[i].transform.parent = ArrowParent.transform;
		}
		ArrowParent.SetActive (false);
	}
	// 発射方向表示
	void viewArrow(){
		arrowrot += ArrowRotSpeed * Time.deltaTime;
		for (int i = 0; i < ArrowValue; i++) {
			float rot = 360f / ArrowValue * i + arrowrot;
			float rad = rot * (Mathf.PI / 180); 
			Vector3 vec = new Vector3 (Mathf.Sin (rad), 0, Mathf.Cos (rad));
			float dist = Dist + DistPlus * iController.getItemLevel ();
			Vector3 pos = vec * dist / 2;
			Arrow[i].transform.position = transform.parent.position + pos;
			Arrow [i].transform.LookAt (CarObj.transform);
		}
	}
}
