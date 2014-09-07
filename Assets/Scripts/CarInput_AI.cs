using UnityEngine;
using System.Collections;

public class CarInput_AI : MonoBehaviour {
	public GameObject TestBall;
	public float CheckDist = 50.0f;		// 探索距離
	public float DistRot = 50.0f;		// 曲がり始める距離(Angle * Now/Max)
	public int CheckValue = 5;			// レイ判定 設定値の2倍+1回
	public float CenterDist = 120.0f;	// 中心から離れると内側に戻りだす
	public float CheckRot = 0.00002f;	// 縦方向のレイのずれ　小さいほど誤差がないかも
	public float WallRot = 0.8f;		// 壁とみなす角度(0~1)
	//public GameObject TestBallPrefab;

	Vector2 dirinput = Vector2.zero;
	float ForwardDist = 0;
	float checktime = 0;
	float nowhandle = 0;
	CarController cController;
	ItemController iController;
	float UndecideRight = 1;	// 曲がる方向が決まらない時の強制方向
	float UndecideRight2 = 1;
	Vector3 HitNormal = Vector3.forward;
	ArrayList HitWallList = new ArrayList();
	//GameObject testball;
	// Use this for initialization
	void Start () {
		cController = GetComponent<CarController> ();
		iController = GetComponent<ItemController> ();
		//testball = (GameObject)Instantiate (TestBallPrefab);
	}

	// Update is called once per frame
	void Update () {
		dirinput = Vector2.zero;

		checktime += Time.deltaTime;
		if (checktime >= 0.2f) {
			checktime = 0;
			// ハンドル決め
			checkWall ();
			calcAngle ();
		}

		goItemBox ();
		goWorldCenter ();
		dirinput.x = nowhandle;

		//testUserHandle ();

		cController.setInput (dirinput);
	}

	// 実験用手動操作
	void testUserHandle(){
		if (Input.GetKey (KeyCode.Space)) {
			dirinput.x = Input.GetAxis ("Horizontal");
			dirinput.y = Input.GetAxis ("Vertical");
		}
	}

	// アイテム箱に移動
	void goItemBox(){
		if (nowhandle == 0) {
			GameObject box = iController.getFindItemBox ();
			if (box) {
				// アイテムまでのベクトル
				Vector3 dir = box.transform.position - transform.position;
				dir = dir / dir.magnitude;
				float carY = Quaternion.LookRotation (cController.getForward ()).eulerAngles.y;
				float dirY = Quaternion.LookRotation (dir).eulerAngles.y;
				float rot = Mathf.DeltaAngle (carY, dirY);
				nowhandle = rot / 60;
				nowhandle = Mathf.Min (1, Mathf.Max (-1, nowhandle));
			}
		}
	}

	// 世界の中心に移動
	void goWorldCenter(){
		if (nowhandle == 0) {
			// 中心から離れている時
			if (Vector3.Distance (transform.position, Vector3.zero) >= CenterDist) {

				// 中心までのベクトル
				Vector3 dir = Vector3.zero - transform.position;
				dir = dir / dir.magnitude;
				float carY = Quaternion.LookRotation (cController.getForward ()).eulerAngles.y;
				float dirY = Quaternion.LookRotation (dir).eulerAngles.y;
				float rot = Mathf.DeltaAngle (carY, dirY);
				nowhandle = rot / 180;
			}
		}
	}

	// 壁を調べる
	void checkWall(){
		HitWallList.Clear ();
		GameObject BaseTemp = new GameObject ();
		BaseTemp.transform.position = transform.position;
		BaseTemp.transform.LookAt (transform.position + cController.getForward());
		ForwardDist = -1;
		rayWall (BaseTemp,0);
		for (int i = 1; i < CheckValue; i++) {
			GameObject temp = new GameObject ();
			temp.transform.position = BaseTemp.transform.position;
			temp.transform.rotation = BaseTemp.transform.rotation;
			temp.transform.position += temp.transform.right * 0.6f;
			temp.transform.Rotate (0,6f*(float)i,0);
			rayWall (temp,i*2);
			temp.transform.position = BaseTemp.transform.position;
			temp.transform.rotation = BaseTemp.transform.rotation;
			temp.transform.position += -temp.transform.right * 0.6f;
			temp.transform.Rotate (0,-6f*(float)i,0);
			rayWall (temp,i*2+1);
			Destroy (temp);
		}
		Destroy (BaseTemp);
	}

	void rayWall(GameObject RayShotObj, int n){
		ArrayList HitList = new ArrayList ();	// レイ衝突リスト
		Vector3 stpos = RayShotObj.transform.position;
		Vector3 stdir = RayShotObj.transform.forward;
		RaycastHit hit;
		LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
		HitWall hitwall = new HitWall ();
		hitwall.angle = RayShotObj.transform.eulerAngles.y;
		hitwall.isWall = false;
		hitwall.distance = 1000;
		if (Physics.Raycast (stpos, stdir, out hit, CheckDist, mask)) {
			HitList.Add (hit.point);
			stdir.y -= CheckRot;
			if (Physics.Raycast (stpos, stdir, out hit, CheckDist, mask)) {
				HitList.Add (hit.point);
				Vector3 bef = (Vector3)HitList [0];
				Vector3 aft = (Vector3)HitList [1];
				GameObject temp = new GameObject ();
				temp.transform.position = aft;
				temp.transform.LookAt (bef);

				float angle = temp.transform.forward.y;
				if (angle >= WallRot) {
					hitwall.isWall = true;
					hitwall.distance = hit.distance;
					if (ForwardDist == -1 && n == 0) {
						ForwardDist = hit.distance;
						HitNormal = hit.normal;
					}
				}

				Vector2 basepos = new Vector2 (RayShotObj.transform.forward.x, RayShotObj.transform.forward.z);
				Vector2 tarpos = new Vector2 (temp.transform.forward.x, temp.transform.forward.z);
				if ((tarpos + basepos).magnitude < basepos.magnitude / 2) {
					hitwall.isWall = true;
					hitwall.distance = hit.distance;
					if (ForwardDist == -1 && n == 0) {
						ForwardDist = hit.distance;
						HitNormal = hit.normal;
					}
				}
				Destroy (temp);
			}
		}
		HitWallList.Add (hitwall);
	}


	void calcAngle(){
		if (ForwardDist < 0) {
			ForwardDist = Mathf.Infinity;
		}
		float turnpow = Mathf.Max (0f, (1 - ForwardDist / DistRot));
		int L = 0;
		int R = 0;
		float distmax = 0;
		float distlevel = 0;
		UndecideRight = 1;
		for(int i=0; i<HitWallList.Count; i++){
			HitWall hw = (HitWall)HitWallList [i];
			float angle = Mathf.DeltaAngle (transform.eulerAngles.y, hw.angle);
			if (!hw.isWall) {
				if (i == 0) {
					//UndecideRight2 *= -1;
					break;
				}
				if (angle < 0) {
					if (L < 3) {
						L++;
					} else {
						nowhandle = -turnpow;
						return;
					}
				} else if (angle > 0) {
					if (R < 3) {
						R++;
					} else {
						nowhandle = turnpow;
						return;
					}
				}
			} else {
				if (angle < 0) {
					if (hw.distance > distmax && i > 0) {
						distmax = hw.distance;
						distlevel -= hw.distance;
						UndecideRight = -1;
					}
				} else if (angle > 0) {
					if (hw.distance > distmax && i > 0) {
						distmax = hw.distance;
						distlevel += hw.distance;
						UndecideRight = 1;
					}
				}
			}
		}

		nowhandle = turnpow * UndecideRight;

		float carY = Quaternion.LookRotation (cController.getForward ()).eulerAngles.y;
		float wallY = Quaternion.LookRotation (HitNormal).eulerAngles.y;
		float rot = Mathf.DeltaAngle (carY, wallY);
		nowhandle = turnpow * Mathf.Sign (rot);
	}


	// 敵が攻撃範囲内に居ると呼ばれる
	void justEnemy(){
		if (iController && this.enabled) {
			iController.useItem ();
		}
	}
}
