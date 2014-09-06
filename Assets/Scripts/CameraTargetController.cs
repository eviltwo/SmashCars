using UnityEngine;
using System.Collections;

public class CameraTargetController : MonoBehaviour {

	public int TeamNum = -1;		// -1:フィールド全体 0~:特定チーム

	GameObject Target;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int bossnum = PlayerManager.Instance.getTeamData () [TeamNum].BossNumber;
		if (bossnum < 0) {
			Target = null;
		} else {
			Target = PlayerManager.Instance.getTeamData () [TeamNum].TeamPlayers [bossnum];
		}
	}

	public GameObject getTarget(){
		return Target;
	}
}
