using UnityEngine;
using System.Collections;

public class FallMissileController : MonoBehaviour {
	public float BombDist = 5.0f;
	public float StartSpeed = 0.5f;
	public GameObject MissileBombWindPrefab;
	public GameObject BombEffectPrefab;
	public GameObject[] ChangeColorObj;
	public GameObject[] ChangeColorParticle;
	public AudioClip BombAudio;

	int TeamNum = 0;
	// Use this for initialization
	void Start () {
		rigidbody.velocity = new Vector3(0,-StartSpeed,0);
		changeColor ();
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < -5f) {
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
			Color c = PlayerManager.Instance.Teams [TeamNum].TeamColor;
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
