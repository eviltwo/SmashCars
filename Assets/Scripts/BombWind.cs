﻿using UnityEngine;
using System.Collections;

public class BombWind : MonoBehaviour {

	public float WindPowerMax = 30.0f;         // 爆風の速さ
	public float WindUpPower = 20.0f;
	public float WindTimeMax = 0.2f;
	public float RotRandomMax = 10.0f;
	float WindTime = 0;

	int TeamNum;
	void Update(){
		WindTime += Time.deltaTime;
		if (WindTime >= WindTimeMax) {
			Destroy (this.gameObject);
		}
	}

	void StartSet(int team){
		TeamNum = team;
	}

	void OnTriggerStay(Collider col) {
		CarController cc = col.GetComponent<CarController> ();
		if (cc) {
			if (cc.TeamNum == TeamNum) {
				return;
			}
		}

		// 風速計算
		Vector3 velocity = (col.transform.position - transform.position).normalized * WindPowerMax;
		float mlt = 1 - WindTime / WindTimeMax;
		velocity.y = WindUpPower;
		// 風力与える
		col.rigidbody.velocity = (velocity);

		Vector3 rot = new Vector3 ();
		rot.x = Random.Range (-RotRandomMax, RotRandomMax);
		rot.y = Random.Range (-RotRandomMax, RotRandomMax);
		rot.z = Random.Range (-RotRandomMax, RotRandomMax);
		col.rigidbody.AddRelativeTorque (rot);
	}
}
