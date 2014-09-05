using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {
	public GameObject CarPrefab;
	public int Value;
	public GameObject CarCameraPrefab;
	public Vector3 CenterPos;
	public Vector3 RandomSize;
	public bool RandomRotY;

	int CameraValue;
	// Use this for initialization
	void Start () {
		spawnAllCar ();
	}

	void spawnRandom(){
		GameObject obj = (GameObject)Instantiate (CarPrefab);
		Vector3 pos = CenterPos;
		for (int i = 0; i < 3; i++) {
			pos [i] += Random.Range (-RandomSize [i], RandomSize [i]);
		}
		obj.transform.position = pos;
		if (RandomRotY) {
			obj.transform.Rotate (0,Random.Range(0,360),0);
		}
	}

	void spawnAllCar(){
		Team[] teams = PlayerManager.Instance.getTeamData ();
		// カメラの数を確認
		CameraValue = 0;
		for (int it = 0; it < teams.Length; it++) {
			Team t = teams [it];
			if (t.isCamera) {
				CameraValue++;
			}
		}
		CameraManager.Instance.setCameraValue (teams.Length);	// チーム数を登録

		// 車生成
		for (int it = 0; it < teams.Length; it++) {
			Team t = teams [it];
			// ボスを生成
			if (t.isBossUser) {
				spawnCar (t, 0, true);	// ユーザー車を生成
			} else {
				spawnCar (t, 1, true);	// AI車を生成
			}
			// ザコを生成
			for (int jn = 1; jn < teams[it].PlayerValue; jn++) {
				spawnCar (t, 1, false);
			}
		}
	}

	void spawnCar(Team team, int ctrl, bool boss){
		GameObject obj = (GameObject)Instantiate (CarPrefab);
		Vector3 pos = CenterPos;
		for (int i = 0; i < 3; i++) {
			pos [i] += Random.Range (-RandomSize [i], RandomSize [i]);
		}
		obj.transform.position = pos;
		if (RandomRotY) {
			obj.transform.Rotate (0,Random.Range(0,360),0);
		}
		PlayerManager.Instance.addTeamPlayer (team.TeamNumber, obj);
		// チームを伝える
		obj.GetComponent<CarController> ().TeamNum = team.TeamNumber;
		// 操作番号を伝える
		obj.GetComponent<CarController> ().InputNum = team.InputNumber;
		// CarInputコンポーネントの選択
		switch (ctrl) {
		case 0:
			// ユーザー
			obj.GetComponent<CarInput_User> ().enabled = true;
			break;
		case 1:
			// AI
			obj.GetComponent<CarInput_AI> ().enabled = true;
			break;
		default:
			Debug.LogError ("CarControllType [" + ctrl + "] is not found.");
			break;
		}

		// カメラをつける
		if (boss && team.isCamera) {
			GameObject cam = (GameObject)Instantiate (CarCameraPrefab);
			cam.GetComponent<CameraController> ().TargetObject = obj;
			setCameraPos (cam, team.TeamNumber);
			CameraManager.Instance.addCamera (cam, team.TeamNumber);		// カメラ登録
			cam.camera.cullingMask += 1 << LayerMask.NameToLayer("UI_"+team.TeamNumber);
		}

	}

	void setCameraPos(GameObject cam, int teamnum){
		Rect rect;
		switch (CameraValue) {
		case 1:
			rect = new Rect (0,0,1,1);
			cam.camera.rect = rect;
			break;
		case 2:
			rect = new Rect ();
			rect.width = 1;
			rect.height = 0.5f;
			rect.x = 0f;
			rect.y = 0.5f - 0.5f * teamnum;
			cam.camera.rect = rect;
			break;
		case 3:
		case 4:
			rect = new Rect ();
			rect.width = 0.5f;
			rect.height = 0.5f;
			rect.x = 0.5f * (teamnum % 2);
			rect.y = 0.5f - 0.5f * (teamnum/2);
			cam.camera.rect = rect;
			break;
		default:
			break;
		}
	}
}
