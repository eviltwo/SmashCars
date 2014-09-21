using UnityEngine;
using System.Collections;

public class HeliCallController : MonoBehaviour {

	public GameObject MissilePrefab;
	public int MissileValue = 3;
	public float FallInterval = 0.5f;
	public float Height = 10.0f;
	public float RandomDist = 5.0f;
	public GameObject Heli;
	public float ComeDist = 100.0f;
	public float ComeTimeMax = 0.5f;
	public float ComeRot = 45.0f;
	public float LeaveDist = 100.0f;
	public float LeaveTimeMax = 3.0f;
	public GameObject[] ChangeColorObj;
	public GameObject[] ChangeColorParticle;

	int TeamNum = 0;
	float FallTime = 0;
	int FallValue = 0;
	int Mode = 0;
	float MoveTime = 0;
	// Use this for initialization
	void Start () {
		FallTime = FallInterval;
		transform.Rotate (0,Random.Range(0f,360f),0);
	}
	
	// Update is called once per frame
	void Update () {
		switch (Mode) {
		case 0:
			ComeMode ();
			break;
		case 1:
			MissileMode ();
			break;
		case 2:
			LeaveMode ();
			break;
		default:
			break;
		}
	}

	void ComeMode(){
		Heli.SetActive (true);
		MoveTime += Time.deltaTime;

		float mlt = Mathf.Min (1, MoveTime / ComeTimeMax);
		float dist = ComeDist * mlt;
		Vector3 pos = Vector3.up * Height - Vector3.forward * (ComeDist - dist);
		Heli.transform.localPosition = pos;
		Heli.transform.localEulerAngles = new Vector3 (ComeRot * (1 - mlt), 0, 0);
		if (MoveTime >= ComeTimeMax) {
			MoveTime = 0;
			Mode++;
		}
	}

	void MissileMode(){
		FallTime += Time.deltaTime;
		if (FallTime >= FallInterval) {
			FallTime -= FallInterval;
			GameObject missile = (GameObject)Instantiate (MissilePrefab);
			Vector3 pos = transform.position + Vector3.up * Height;
			pos.x += Random.Range (-RandomDist, RandomDist);
			pos.z += Random.Range (-RandomDist, RandomDist);
			missile.transform.position = pos;
			missile.SendMessage ("StartSet", TeamNum, SendMessageOptions.DontRequireReceiver);
			FallValue++;
			if (FallValue >= MissileValue) {
				Destroy (transform.FindChild ("RotEffect").gameObject);
				Mode++;
			}
		}
	}

	void StartSet(int team){
		TeamNum = team;
		changeColor ();
	}

	void LeaveMode(){
		MoveTime += Time.deltaTime;
		float mlt = Mathf.Min (1, MoveTime / LeaveTimeMax);
		float dist = LeaveDist * mlt;
		Vector3 pos = Vector3.up * Height + Vector3.forward * dist;
		Heli.transform.localPosition = pos;
		Heli.transform.localEulerAngles = new Vector3 (ComeRot * (mlt), 0, 0);
		if (MoveTime >= LeaveTimeMax) {
			Destroy (this.gameObject);
		}
	}

	// 色を変える
	void changeColor(){
		for (int i = 0; i < ChangeColorObj.Length; i++) {
			GameObject parts = ChangeColorObj [i];
			Color c = PlayerManager.Instance.Teams [TeamNum].TeamColor;
			float mlt = 0.7f;
			c.r += (1-c.r)*mlt;
			c.g += (1-c.g)*mlt;
			c.b += (1-c.b)*mlt;
			parts.renderer.material.color = c;
		}
		for (int i = 0; i < ChangeColorParticle.Length; i++) {
			GameObject parts = ChangeColorParticle [i];
			Color c = PlayerManager.Instance.Teams [TeamNum].TeamColor;
			c.a = 1;
			parts.GetComponent<ParticleRenderer>().material.SetColor("_TintColor",c);
		}
	}
}
