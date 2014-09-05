using UnityEngine;
using System.Collections;

public class CarSpawner : MonoBehaviour {
	public GameObject CarPrefab;
	public int Value;
	public GameObject CarCameraPrefab;
	public Vector3 CenterPos;
	public Vector3 RandomSize;
	public bool RandomRotY;

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
		for (int it = 0; it < teams.Length; it++) {
			Team t = teams [it];
			// ボスを生成
			if (t.isBossUser) {
				spawnCar (t, 0);	// ユーザー車を生成
			} else {
				spawnCar (t, 1);	// AI車を生成
			}
			// ザコを生成
			for (int jn = 1; jn < teams[it].PlayerValue; jn++) {
				spawnCar (t, 1);
			}
		}
	}

	void spawnCar(Team team, int ctrl){
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
		// CarInputコンポーネントの選択
		switch (ctrl) {
		case 0:
			// ユーザー
			obj.GetComponent<CarInput_User> ().enabled = true;
			GameObject cam = (GameObject)Instantiate (CarCameraPrefab);
			cam.GetComponent<CameraController> ().TargetObject = obj;
			break;
		case 1:
			// AI
			obj.GetComponent<CarInput_AI> ().enabled = true;
			break;
		default:
			Debug.LogError ("CarControllType [" + ctrl + "] is not found.");
			break;
		}

	}
}
