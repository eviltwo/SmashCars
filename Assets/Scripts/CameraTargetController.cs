﻿using UnityEngine;
using System.Collections;

public class CameraTargetController : MonoBehaviour {

	public int TeamNum = -1;		// -1:フィールド全体 0~:特定チーム

	GameObject Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!PlayerManager.Instance.isGameEnd) {
			if (TeamNum >= 0) {
				if (PlayerManager.Instance.getTeamData () [TeamNum].PlayerValue > 0) {
					int bossnum = PlayerManager.Instance.getTeamData () [TeamNum].BossNumber;
					if (bossnum < 0) {
						GetComponent<CameraController> ().enabled = false;
						GetComponent<PutCameraController> ().enabled = true;
					} else {
						Target = PlayerManager.Instance.getTeamData () [TeamNum].TeamPlayers [bossnum];
						GetComponent<CameraController> ().enabled = true;
						GetComponent<PutCameraController> ().enabled = false;
					}
				} else {
					TeamNum = -1;
					GetComponent<CameraController> ().enabled = false;
					GetComponent<PutCameraController> ().enabled = true;
				}
			} else {
				GetComponent<CameraController> ().enabled = false;
				GetComponent<PutCameraController> ().enabled = true;
			}
		} else {
			// 試合終了後、置きカメラ
			GetComponent<CameraController> ().enabled = false;
			GetComponent<PutCameraController> ().enabled = true;
		}
	}

	public GameObject getTarget(){
		return Target;
	}

	public int getTeamNum(){
		return TeamNum;
	}
}
