using UnityEngine;
using System.Collections;

public class Billboard_SetName : MonoBehaviour {

	public GameObject CarObj;
	public Texture Boss;
	public Texture Zako;

	bool isboss = false;
	ArrayList BillboardList = new ArrayList();
	ArrayList BillboardNumList = new ArrayList();
	CarController cController;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (CarObj) {
			transform.position = CarObj.transform.position;
		}

		// 表示するか判定
		for (int i = 0; i < BillboardList.Count; i++) {
			GameObject board = (GameObject)BillboardList [i];
			int num = (int)BillboardNumList [i];
			int boss = PlayerManager.Instance.getTeamData () [num].BossNumber;
			board.SetActive (false);
			if(CarObj){
				if (boss >= 0) {
					GameObject car = PlayerManager.Instance.getTeamData () [num].TeamPlayers [boss];	// カメラを持つboss
					if (car && PlayerManager.Instance.getTeamData () [num].isCamera) {
						ItemController iController = car.GetComponent<ItemController> ();
						GameObject[] enemies = iController.getFindEnemy ();
						bool find = false;
						for (int j = 0; j < enemies.Length; j++) {
							GameObject enemy = enemies [j];
							if (enemy == CarObj) {
								GameObject cam = CameraManager.Instance.getCamera (num);
								if (cam) {
									Vector3 stpos = cam.transform.position;
									Vector3 tarpos = enemy.transform.position;
									Vector3 stdir = tarpos - stpos;
									float checkdist = stdir.magnitude;
									stdir = stdir / checkdist;
									RaycastHit hit;
									LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
									if (!Physics.Raycast (stpos, stdir, out hit, checkdist, mask)) {
										board.SetActive (true);
										find = true;
									}
								}
								break;
							}
						}
					}
				}
			}
		}

		if (isboss != cController.IsBoss) {
			isboss = cController.IsBoss;
			for (int i = 0; i < BillboardList.Count; i++) {
				GameObject board = (GameObject)BillboardList [i];
				TextMesh textmesh = board.transform.FindChild ("Name").gameObject.GetComponent<TextMesh>();
				MeshRenderer icon = board.transform.FindChild ("Icon").gameObject.GetComponent<MeshRenderer>();
				int team = CarObj.GetComponent<CarController> ().TeamNum;
				Team myteam = PlayerManager.Instance.getTeamData () [team];
				isboss = CarObj.GetComponent<CarController> ().IsBoss;
				if (isboss) {
					textmesh.text = myteam.BossName;
					icon.material.mainTexture = Boss;
				} else {
					textmesh.text = "";
					icon.material.mainTexture = Zako;
				}
			}
		}
	}

	void SetCar(GameObject car){
		CarObj = car;
	}


	void addBillboard(GameObject board){
		cController = CarObj.GetComponent<CarController> ();
		TextMesh textmesh = board.transform.FindChild ("Name").gameObject.GetComponent<TextMesh>();
		MeshRenderer icon = board.transform.FindChild ("Icon").gameObject.GetComponent<MeshRenderer>();
		int team = cController.TeamNum;
		Team myteam = PlayerManager.Instance.getTeamData () [team];
		isboss = cController.IsBoss;
		if (isboss) {
			textmesh.text = myteam.BossName;
			icon.material.mainTexture = Boss;
		} else {
			textmesh.text = "";
			icon.material.mainTexture = Zako;
		}
		Color c = myteam.TeamColor;
		c.a = 1.0f;
		textmesh.color = c;
		icon.material.color = c;
		BillboardList.Add (board);
	}
	void addBillboardNum(int n){
		BillboardNumList.Add (n);
	}
}
