using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

	public float Speed = 50.0f;
	public float WallHeight = 0.7f;
	public float FlyHeight = 0.2f;
	public float Gravity = 9.8f;
	public float BombDist = 120.0f;
	public GameObject MissileBombWindPrefab;
	public GameObject BombEffectPrefab;
	public GameObject[] ChangeColorObj;
	public GameObject[] ChangeColorParticle;
	public AudioClip BombAudio;
	public GameObject RotObject;
	public float RandomRotMax = 100;

	int TeamNum;
	Vector3 StartPos;
	Vector3 OldGround = Vector3.zero;
	float totaldist = 0;
	float ranrot = 0;
	// Use this for initialization
	void Start () {
		StartPos = transform.position;
		OldGround = transform.position;
		changeColor ();
		ranrot = Random.Range (-RandomRotMax, RandomRotMax);
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 stpos = transform.position;
		Vector3 stdir = transform.forward;
		float checkdist = Speed * Time.deltaTime;
		RaycastHit hit;
		LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
		if (Physics.Raycast (stpos, stdir, out hit, checkdist, mask)) {
			float mlt = 1-hit.normal.y;
			if (mlt > WallHeight) {
				Bomb ();
			} else {
				transform.position = hit.point + Vector3.up * FlyHeight;
				OldGround = hit.point;
				totaldist += Vector3.Distance (transform.position, hit.point);
			}
		} else {
			stpos = transform.position + transform.forward * Speed * Time.deltaTime;
			stdir = -Vector3.up;
			checkdist = FlyHeight + Gravity * Time.deltaTime;
			if (Physics.Raycast (stpos, stdir, out hit, checkdist, mask)) {
				float mlt = (hit.point.y-OldGround.y)/(Speed * Time.deltaTime);
				if (mlt > WallHeight) {
					transform.position = stpos;
					totaldist += Speed * Time.deltaTime;
				} else {
					transform.position = hit.point + Vector3.up * FlyHeight;
					OldGround = hit.point;
					totaldist += Speed * Time.deltaTime;
				}
			} else {
				transform.position = stpos - Vector3.up * Gravity * Time.deltaTime;
			}
		}

		// 回転
		Vector3 rot = RotObject.transform.localEulerAngles;
		rot.z += ranrot * Time.deltaTime;
		RotObject.transform.localEulerAngles = rot;

		if (totaldist >= BombDist) {
			Bomb ();
		}
	}


	void StartSet(int team){
		TeamNum = team;
	}

	void OnTriggerEnter(Collider collider){
		CarController cc = collider.GetComponent<CarController> ();
		if (cc) {
			if (cc.TeamNum == TeamNum) {
				return;
			}
		} else {
			if (collider.gameObject.layer == LayerMask.NameToLayer ("FieldCollider")) {
				return;
			}
		}
		Bomb ();
	}

	// 爆発
	void Bomb(){
		GameObject wind = (GameObject)Instantiate (MissileBombWindPrefab);
		wind.transform.position = transform.position;
		wind.SendMessage ("StartSet", TeamNum);
		GameObject effect = (GameObject)Instantiate (BombEffectPrefab);
		effect.transform.position = transform.position;
		effect.SendMessage ("StartSet", TeamNum);
		// 音
		AudioManager.Instance.playSE (BombAudio, transform.position);
		Destroy (this.gameObject);
	}

	// 色を変える
	void changeColor(){
		for (int i = 0; i < ChangeColorObj.Length; i++) {
			GameObject parts = ChangeColorObj [i];
			parts.renderer.material.color = PlayerManager.Instance.Teams [TeamNum].TeamColor;
		}
		for (int i = 0; i < ChangeColorParticle.Length; i++) {
			GameObject parts = ChangeColorParticle [i];
			Color c = PlayerManager.Instance.Teams [TeamNum].TeamColor;
			c.a = 1;
			parts.GetComponent<ParticleRenderer>().material.SetColor("_TintColor",c);
		}
	}
}
