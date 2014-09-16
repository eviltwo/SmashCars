using UnityEngine;
using System.Collections;

public class PutCameraController : MonoBehaviour {

	public float MoveDist = 30.0f;
	public float ForwardMlt = 3.0f;
	public Vector3 RandomSize = new Vector3 (3,3,3);

	int NowTeam = -2;
	GameObject TargetObject;
	float MoveTimeMax = 1f;
	int CameraMode = 0;
	Vector3 NewPos = new Vector3 ();
	Vector3 OldPos = new Vector3();
	float movetime = 0;
	float rotheight = 0;
	float rotdist = 1;
	float rottime = 0;
	float fieldrot = 0;
	CarController cController;
	CameraTargetController ctController;
	// Use this for initialization
	void Start () {
		//cController = TargetObject.GetComponent<CarController> ();
		ctController = GetComponent<CameraTargetController> ();
	}
	
	// Update is called once per frame
	void Update () {
		// チーム確認
		ctController = GetComponent<CameraTargetController> ();
		if (NowTeam != ctController.getTeamNum ()) {
			NowTeam = ctController.getTeamNum ();
			if (NowTeam >= 0) {
				setTeamTarget ();
			} else {
				CameraMode = 2;
			}
		}

		if (TargetObject) {
			NewPos = TargetObject.transform.position;
		}

		checkTime ();
		Move ();

		OldPos = NewPos;
	}

	void checkTime(){
		float dist = Vector3.Distance (transform.position, NewPos);
		movetime += Time.deltaTime;
		if (movetime >= MoveTimeMax || cController && movetime/MoveTimeMax > 0.8f && cController.IsFlying || dist > MoveDist) {
			movetime = 0;
			if (NowTeam >= 0) {
				if (cController.IsFlying) {
					CameraMode = 0;
					randomMoveStart ();
				} else {
					CameraMode = 1;
					rotMoveStart ();
					// ターゲット決め
					setTeamTarget ();
				}
			} else {
				CameraMode = 2;
				fieldMoveStart ();
			}
			MoveTimeMax = Random.Range (3f, 5f);
		}
	}

	// チームの誰かをターゲットにする
	void setTeamTarget(){
		int playervalue = PlayerManager.Instance.getTeamData () [NowTeam].PlayerValue;
		int ran = Random.Range (0, playervalue);
		GameObject[] players = PlayerManager.Instance.getTeamData () [NowTeam].TeamPlayers;
		int nowplayer = 0;
		for (int i = 0; i < players.Length; i++) {
			if (players [i]) {
				if (nowplayer == ran) {
					TargetObject = players [i];
					cController = TargetObject.GetComponent<CarController> ();
					break;
				}
				nowplayer++;
			}
		}
	}

	void Move(){
		switch (CameraMode) {
		case 0:
			randomMove ();
			break;
		case 1:
			rotMove ();
			break;
		case 2:
			fieldMove ();
			break;
		default:
			break;
		}
	}

	// ターゲットの先で待ち伏せて撮る
	void randomMoveStart(){
		//Vector3 basepos = OldPos + (NewPos - OldPos) * ForwardMlt;
		Vector3 velvec = TargetObject.rigidbody.velocity / TargetObject.rigidbody.velocity.magnitude;
		Vector3 basepos = TargetObject.transform.position+velvec*MoveDist;
		basepos.x += Random.Range (-RandomSize.x, RandomSize.y);
		basepos.y = Random.Range (0.1f, RandomSize.y);
		basepos.z += Random.Range (-RandomSize.z, RandomSize.z);
		transform.position = basepos;
	}
	void randomMove(){
		if (TargetObject) {
			transform.LookAt (TargetObject.transform.position);
		}
	}

	// ターゲット中心に回転して撮る
	void rotMoveStart(){
		rotdist = Random.Range (2f, 10f);
		rotheight = Random.Range (0f, 3f);
		rottime = Random.Range (0f, 360f);
	}
	void rotMove(){
		if (TargetObject) {
			transform.position = TargetObject.transform.position;
			transform.localEulerAngles = new Vector3 (0, rottime, 0);
			transform.position += -transform.forward * rotdist;
			transform.position += transform.up * rotheight;
			transform.LookAt (TargetObject.transform.position);
		}

		rottime += 15f * Time.deltaTime;
		if (rottime >= 360) {
			rottime -= 360;
		} else if (rottime < 0) {
			rottime += 360;
		}
	}

	// フィールドを撮る
	void fieldMoveStart(){

	}
	void fieldMove(){
		transform.position = Vector3.zero;
		transform.localEulerAngles = new Vector3 (0,fieldrot,0);
		transform.position += -transform.forward * 50;
		transform.position += transform.up * 10;
		transform.LookAt (Vector3.zero);

		fieldrot += 5f * Time.deltaTime;
		if (fieldrot >= 360) {
			fieldrot -= 360;
		} else if (fieldrot < 0) {
			fieldrot += 360;
		}
	}
}
