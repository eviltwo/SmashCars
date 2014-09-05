using UnityEngine;
using System.Collections;

public class Billboard_SetName : MonoBehaviour {

	public GameObject CarObj;
	public Texture Boss;
	public Texture Zako;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = CarObj.transform.position;
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
	}
}
