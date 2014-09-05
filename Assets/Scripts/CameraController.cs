using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject TargetObject;

	public Vector3 Position = new Vector3 ();
	public Vector3 Look = new Vector3();
	public Vector3 OldPos = new Vector3();
	public float SmoothPosMultiple = 0.1f;
	public float SmoothRotMultiple = 0.1f;

	float RotSpeedMax = 40.0f;
	float RotSpeed = 0;
	float BackSpeed = 0.5f;
	float RotRan = 1;
	CarController cController;
	// Use this for initialization
	void Start () {
		cController = TargetObject.GetComponent<CarController> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 NewPos = TargetObject.transform.position;
		GameObject temp = new GameObject ();
		temp.transform.position = OldPos;
		temp.transform.LookAt (NewPos);
		temp.transform.position += temp.transform.right * Position.x;
		temp.transform.position += temp.transform.up * Position.y;
		temp.transform.position += temp.transform.forward * Position.z;

		float dist = Vector3.Distance (Vector3.zero, TargetObject.rigidbody.velocity);
		if (!cController.isStop()) {
			transform.position = temp.transform.position;
			Vector3 oldrot = transform.localEulerAngles;
			Vector3 newrot = temp.transform.localEulerAngles;
			newrot.y = Mathf.DeltaAngle (oldrot.y, newrot.y) * SmoothRotMultiple + oldrot.y;
			transform.localEulerAngles = newrot;
			RotSpeed = RotSpeedMax;
			RotRan = Mathf.Sign (Random.Range (-1, 1));
		} else {
			transform.Rotate (0,0,RotSpeed*Time.deltaTime*RotRan);
			transform.position += -transform.forward*BackSpeed*Time.deltaTime;
			RotSpeed *= 0.95f;
		}
		Destroy (temp);
		OldPos += (NewPos-OldPos)*SmoothPosMultiple;

		if (cController.gameObject != TargetObject) {
			cController = TargetObject.GetComponent<CarController> ();
		}
	}
}
