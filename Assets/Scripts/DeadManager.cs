using UnityEngine;
using System.Collections;

public class DeadManager : SingletonMonoBehaviour<DeadManager> {

	bool[] DeadList;
	// Use this for initialization
	void Start () {
		resetDeadList ();
	}
	
	// Update is called once per frame
	void Update () {
		checkRespawn ();
	}

	// リスポーンできるかチェック
	void checkRespawn(){
		for (int i = 0; i < DeadList.Length; i++) {
			if (DeadList [i]) {
				if (PlayerManager.Instance.getTeamData () [i].PlayerValue == 0) {
					DeadList [i] = false;
					WaitManager.Instance.resetWaitTime (i);
				} else {
					if (WaitManager.Instance.getWaitTime (i) <= 0) {
						// リスポーン
						respawnBoss (i);
						DeadList [i] = false;
					}
				}
			}
		}
	}

	// リスポーン処理
	void respawnBoss(int team){
		GameObject[] players = PlayerManager.Instance.getTeamData () [team].TeamPlayers;
		float maxhp = -1;
		int maxnum = -1;
		CarController cc;
		for (int i = 0; i < players.Length; i++) {
			if (players [i]) {
				cc = players [i].GetComponent<CarController> ();
				if (cc.HP > maxhp) {
					maxhp = cc.HP;
					maxnum = i;
				}
			}
		}
		// ボス復活
		cc = players [maxnum].GetComponent<CarController> ();
		cc.IsBoss = true;
		if (PlayerManager.Instance.getTeamData () [team].isBossUser) {
			cc.GetComponent<CarInput_AI> ().enabled = false;
			cc.GetComponent<CarInput_User> ().enabled = true;
			cc.InputNum = PlayerManager.Instance.getTeamData () [team].InputNumber;
		}
		PlayerManager.Instance.getTeamData () [team].BossNumber = maxnum;
	}

	// 死亡リストを初期化
	void resetDeadList(){
		int teamvalue = PlayerManager.Instance.getTeamData ().Length;
		DeadList = new bool[teamvalue];
	}

	public bool isDead(int team){
		return DeadList [team];
	}

	// 死亡を伝える
	public void setDead(int team){
		DeadList [team] = true;
		WaitManager.Instance.setDeadTime (team);
	}
}
