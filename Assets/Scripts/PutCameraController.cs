using UnityEngine;
using System.Collections;

public class PutCameraController : MonoBehaviour {

	public float MoveDist = 30.0f;
	public float ForwardMlt = 3.0f;
	public Vector3 RandomSize = new Vector3 (3,3,3);

	GameObject TargetObject;
	float MoveTimeMax = 1f;
	int CameraMode = 0;
	Vector3 NewPos = new Vector3 ();
	Vector3 OldPos = new Vector3();
	float movetime = 0;
	float rotheight = 0;
	float rotdist = 1;
	float rottime = 0;
	CarController cController;
	CameraTargetController ctController;
	// Use this for initialization
	void Start () {
		cController = TargetObject.GetComponent<CarController> ();
		ctController = GetComponent<CameraTargetController> ();
	}
	
	// Update is called once per frame
	void Update () {
		// ターゲット確認
		if (TargetObject != ctController.getTarget ()) {
			TargetObject = ctController.getTarget ();
			cController = TargetObject.GetComponent<CarController> ();
		}

		NewPos = TargetObject.transform.position;

		checkTime ();
		Move ();

		OldPos = NewPos;
	}

	void checkTime(){
		float dist = Vector3.Distance (transform.position, NewPos);
		movetime += Time.deltaTime;
		if (movetime >= MoveTimeMax || movetime/MoveTimeMax > 0.5f && cController.IsFlying || dist > MoveDist) {
			movetime = 0;
			if (cController.IsFlying) {
				CameraMode = 0;
				randomMoveStart ();
			} else {
				CameraMode = 1;
				rotMoveStart ();
			}
			MoveTimeMax = Random.Range (3f, 5f);
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
		default:
			break;
		}
	}

	void randomMoveStart(){
		Vector3 basepos = OldPos + (NewPos - OldPos) * ForwardMlt;
		basepos.x += Random.Range (-RandomSize.x, RandomSize.y);
		basepos.y = Random.Range (0.1f, RandomSize.y);
		basepos.z += Random.Range (-RandomSize.z, RandomSize.z);
		transform.position = basepos;
	}
	void randomMove(){
		transform.LookAt (TargetObject.transform.position);
	}

	void rotMoveStart(){
		rotdist = Random.Range (2f, 10f);
		rotheight = Random.Range (0f, 3f);
		rottime = Random.Range (0f, 360f);
	}
	void rotMove(){
		transform.position = TargetObject.transform.position;
		transform.localEulerAngles = new Vector3 (0,rottime,0);
		transform.position += -transform.forward * rotdist;
		transform.position += transform.up * rotheight;
		transform.LookAt (TargetObject.transform.position);

		rottime += 15f * Time.deltaTime;
		if (rottime >= 360) {
			rottime -= 360;
		} else if (rottime < 0) {
			rottime += 360;
		}
	}

}
