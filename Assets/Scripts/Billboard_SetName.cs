using UnityEngine;
using System.Collections;

public class Billboard_SetName : MonoBehaviour {

	public GameObject CarObj;
	public Texture Boss;
	public Texture Zako;

	ArrayList BillboardList = new ArrayList();
	ArrayList BillboardNumList = new ArrayList();
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = CarObj.transform.position;
		/*for (int j = 0; j < BillboardList.Count; j++) {
			GameObject board = (GameObject)BillboardList [j];
			GameObject text = board.transform.FindChild ("Name").gameObject;
			GameObject icon = board.transform.FindChild ("Icon").gameObject;
			text.SetActive (false);
			icon.SetActive (false);
		}*/
		for (int i = 0; i < BillboardList.Count; i++) {
			GameObject board = (GameObject)BillboardList [i];
			int num = (int)BillboardNumList [i];
			GameObject car = PlayerManager.Instance.getTeamData () [num].TeamPlayers [0];
			if (car) {
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
				if (!find) {
					board.SetActive (false);
				}
			}
		}
	}

	void SetCar(GameObject car){
		CarObj = car;
	}


	void addBillboard(GameObject board){
		TextMesh textmesh = board.transform.FindChild ("Name").gameObject.GetComponent<TextMesh>();
		MeshRenderer icon = board.transform.FindChild ("Icon").gameObject.GetComponent<MeshRenderer>();
		int team = CarObj.GetComponent<CarController> ().TeamNum;
		Team myteam = PlayerManager.Instance.getTeamData () [team];
		if (CarObj.GetComponent<CarController> ().IsBoss) {
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
