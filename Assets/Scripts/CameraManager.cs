using UnityEngine;
using System.Collections;

// チームごとのカメラを管理
public class CameraManager : SingletonMonoBehaviour<CameraManager> {

	GameObject[] CameraList = new GameObject[0];

	public void setCameraValue(int value){
		CameraList = new GameObject[value];
	}

	// カメラ追加
	public void addCamera(GameObject cam, int team){
		CameraList [team] = cam;
	}

	// カメラ取得
	public GameObject getCamera(int team){
		return CameraList [team];
	}

	// カメラ数取得
	public int getCameraValue(){
		return CameraList.Length;
	}
}
