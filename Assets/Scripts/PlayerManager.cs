using UnityEngine;
using System.Collections;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager> {

	public Team[] Teams;

	void Start(){
		/*if (!Teams && Teams [0].PlayerValue > 0) {
			for (int i = 0; i < Teams.Length; i++) {
				Teams[i].TeamPlayers = new GameObject[Teams[i].PlayerValue];
			}
		}*/
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
