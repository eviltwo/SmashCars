using UnityEngine;
using System.Collections;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager> {

	public Team[] Teams;

	void Start(){
	}

	void Update(){
		checkPlayer ();
	}

	// 人数・bossを確認
	void checkPlayer(){
		for (int i = 0; i < Teams.Length; i++) {
			int playervalue = 0;
			for (int j = 0; j < Teams [i].TeamPlayers.Length; j++) {
				if (Teams [i].TeamPlayers [j]) {
					playervalue++;
				} else {
					// ボス不在
					if (Teams [i].BossNumber == j) {
						Teams [i].BossNumber = -1;
					}
				}
			}
			Teams [i].PlayerValue = playervalue;
		}
	}

	public void setTeamValue(int value){
		Teams = new Team[value];
	}

	public void setTeamPlayerCalue(int team,int value){
		Teams [team].PlayerValue = value;
		Teams [team].TeamPlayers = new GameObject[value];
	}

	public void setTeamData(int team, Team data){
		Teams [team] = data;
	}

	public Team[] getTeamData(){
		return Teams;
	}

	public void addTeamPlayer(int team, GameObject player){
		for (int i = 0; i < Teams [team].TeamPlayers.Length; i++) {
			if (!Teams [team].TeamPlayers [i]) {
				Teams [team].TeamPlayers [i] = player;
				break;
			}
		}
	}
}
