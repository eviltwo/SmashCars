using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {
	public float MotorPowerMax = 10.0f;
	public float SpeedMax = 120.0f;
	public float HandleAngleMax = 20.0f;
	public float BrakeDefault = 10.0f;
	public float FlyRotSpeed = 5.0f;
	public Vector3 CenterOfMass = new Vector3();
	public GameObject[] WheelColliderObj = new GameObject[4];
	public GameObject[] WheelModel = new GameObject[4];
	public GameObject DamageEffectPrefab;
	public bool IsFlying = false;
	public int TeamNum;
	public GameObject[] ChangeColorParts;

	int SpeedLogMax = 5;
	float[] SpeedLog;
	int SpeedLogNum = 0;
	float AverageSpeed = 0;
	float oldvec = 0;
	float KmParHour = 0;
	WheelCollider[] wCollider = new WheelCollider[4];
	Vector2 dirinput = Vector2.zero;
	Vector3 OldPos = Vector3.zero;
	float handleangle;
	float forword = 1;
	float stoptime = 0;
	float VecSpeed = 0;
	bool IsStop = false;
	// Use this for initialization
	void Start () {
		for (int i = 0; i < WheelColliderObj.Length; i++) {
			wCollider [i] = WheelColliderObj[i].GetComponent<WheelCollider>();
		}
		rigidbody.centerOfMass = CenterOfMass;
		SpeedLog = new float[SpeedLogMax];
		changeColor ();
	}
	
	// Update is called once per frame
	void Update () {
		moveCar ();
		checkSpeed ();
		changeForward ();
		moveModel ();
		flyMove ();
		checkStop ();
		calcAverageSpeed ();
		respawnCar ();

		if (Input.GetKeyDown (KeyCode.R)) {
			restartCar ();
		}
	}

	// 入力を読み込ませる
	public void setInput(Vector2 input){
		dirinput = input;
	}

	// 車体の色を変更
	void changeColor(){
		for (int i = 0; i < ChangeColorParts.Length; i++) {
			GameObject parts = ChangeColorParts [i];
			Color c = PlayerManager.Instance.Teams [TeamNum].TeamColor;
			float mlt = 0.5f;
			if (i == 0) {
				mlt = 0.6f;
			}
			c.r += (1-c.r)*mlt;
			c.g += (1-c.g)*mlt;
			c.b += (1-c.b)*mlt;
			parts.renderer.material.color = c;
		}
	}

	// 運転関係の動き
	void moveCar(){
		// アクセル
		//float motorpow = MotorPowerMax * dirinput [1];
		float motorpow = MotorPowerMax * 1;
		if (dirinput [1] >= 0) {
			if (KmParHour > SpeedMax) {
				motorpow = 0;
			}
			wCollider [2].motorTorque = motorpow * forword * 0.1f;
			wCollider [3].motorTorque = motorpow * forword * 0.1f;
			wCollider [2].motorTorque = motorpow * forword;
			wCollider [3].motorTorque = motorpow * forword;

			wCollider [0].brakeTorque = Mathf.Abs (BrakeDefault);
			wCollider [1].brakeTorque = Mathf.Abs (BrakeDefault);
			wCollider [2].brakeTorque = Mathf.Abs (BrakeDefault);
			wCollider [3].brakeTorque = Mathf.Abs (BrakeDefault);
		} else {
			wCollider [0].brakeTorque = Mathf.Abs (motorpow+BrakeDefault);
			wCollider [1].brakeTorque = Mathf.Abs (motorpow+BrakeDefault);
			wCollider [2].brakeTorque = Mathf.Abs (motorpow+BrakeDefault);
			wCollider [3].brakeTorque = Mathf.Abs (motorpow+BrakeDefault);
		}
		if (Mathf.Abs(wCollider [2].rpm) < 100) {
			wCollider [0].brakeTorque = 0;
			wCollider [1].brakeTorque = 0;
			wCollider [2].brakeTorque = 0;
			wCollider [3].brakeTorque = 0;
		}

		// ハンドル
		float handmlt = Mathf.Max(0.05f, 1-VecSpeed / 64.0f);
		float stangle = HandleAngleMax * dirinput [0]*handmlt;
		wCollider [0].steerAngle = stangle;
		wCollider [1].steerAngle = stangle;

	}

	// 速度関係を測定
	void checkSpeed(){
		Vector3 newpos = transform.position;
		float dist = Vector3.Distance (OldPos, newpos);
		float mlt = (60*60)/Time.deltaTime;
		float kmh = dist * mlt / 1000;
		float trq = (wCollider [2].rpm + wCollider [3].rpm) / 2;
		OldPos = newpos;
		if (kmh > 0) {
			KmParHour = kmh;
		}

		VecSpeed = Vector3.Distance (Vector3.zero, rigidbody.velocity);
		if (oldvec-VecSpeed > 1.0f && oldvec-VecSpeed < 200.0f) {
			int value = Mathf.CeilToInt((oldvec - VecSpeed - 1.0f) / 2f);
			for (int i = 0; i < value; i++) {
				effectDamage ();
			}
		}

		oldvec = VecSpeed;

	}

	// 車を進行方向に反転させる
	void changeForward(){
		// 前後反転
		float cary = Quaternion.LookRotation(getForward()).eulerAngles.y;
		float vely = Quaternion.LookRotation(rigidbody.velocity/rigidbody.velocity.magnitude).eulerAngles.y;
		if(Mathf.Abs(Mathf.DeltaAngle(cary,vely)) > 90){
			forword *= -1;
			WheelCollider[] tempc = new WheelCollider[]{ wCollider [0], wCollider [1] };
			wCollider [0] = wCollider [2];
			wCollider [1] = wCollider [3];
			wCollider [2] = tempc [0];
			wCollider [3] = tempc [1];
			wCollider [2].steerAngle = 0;
			wCollider [3].steerAngle = 0;
			wCollider [0].motorTorque = 0;
			wCollider [1].motorTorque = 0;
			wCollider [0].brakeTorque = 0;
			wCollider [1].brakeTorque = 0;
			GameObject[] tempg = new GameObject[]{ WheelModel [0], WheelModel [1] };
			WheelModel [0] = WheelModel [2];
			WheelModel [1] = WheelModel [3];
			WheelModel [2] = tempg [0];
			WheelModel [3] = tempg [1];
		}

		// 上下反転
		if (transform.eulerAngles.z > 90 && transform.eulerAngles.z < 270) {
			transform.Rotate (0,0,180);
		}
	}

	// モデルをColliderに合わせて描画
	void moveModel(){
		for (int i = 0; i < 4; i++) {
			Vector3 angle = Vector3.zero;
			angle.y = wCollider [i].steerAngle;
			WheelModel [i].transform.localEulerAngles = angle;
		}
	}

	// 空中判定・動き
	void flyMove(){
		bool fly = true;
		for (int i = 0; i < 4; i++) {
			if (wCollider [i].isGrounded) {
				fly = false;
				break;
			}
		}
		IsFlying = fly;

		// 空中で方向転換
		if (IsFlying) {
			rigidbody.AddTorque (new Vector3(0,dirinput.x*FlyRotSpeed*Time.deltaTime,0));
		}
	}

	// 壁等に衝突して止まっているかどうか
	void checkStop(){
		if (KmParHour < 4) {
			stoptime += Time.deltaTime;
			IsStop = true;
			if (stoptime > 1) {
				stoptime = 0;
				restartCar ();
			}
		} else {
			stoptime = 0;
			IsStop = false;
		}
	}

	// 衝突エフェクト生成
	void effectDamage(){
		GameObject effect = (GameObject)Instantiate (DamageEffectPrefab);
		effect.transform.position = transform.position;
		effect.particleEmitter.localVelocity = rigidbody.velocity*0.5f;
	}

	// その場から簡易復帰
	void restartCar(){
		transform.position += new Vector3(Random.Range(-10f,10f),5,Random.Range(-10f,10f));
		rigidbody.velocity = new Vector3(Random.Range(-10f,10f),0,Random.Range(-10f,10f));
	}

	// 速度の平均を算出
	void calcAverageSpeed(){
		SpeedLog [SpeedLogNum] = KmParHour;
		SpeedLogNum++;
		if (SpeedLogNum >= SpeedLogMax) {
			SpeedLogNum = 0;
		}
		float total = 0;
		for (int i = 0; i < SpeedLogMax; i++) {
			total += SpeedLog [i];
		}
		AverageSpeed = total / SpeedLogMax;
	}

	// 落ちたら復帰
	void respawnCar(){
		if (transform.position.y < -5) {
			transform.position = new Vector3(Random.Range(-10f,10f),Random.Range(5f,10f),Random.Range(-10f,10f));
			transform.Rotate (0,Random.Range(0f,360f),0);
			oldvec = Vector3.Distance (Vector3.zero, rigidbody.velocity);
		}
	}

	//正面のベクトルを取得
	public Vector3 getForward(){
		Vector3 fwd = new Vector3 ();
		fwd = transform.forward * forword;
		return fwd;
	}

	// 止まっているか
	public bool isStop(){
		return IsStop;
	}
}
