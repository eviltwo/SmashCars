using UnityEngine;
using System.Collections;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager> {

	public Team[] Teams;
	public int[] WinNumbers;
	public bool isGameEnd = false;

	int NextWinNumber = 4;
	void Start(){
	}

	void Update(){
		checkPlayer ();
		checkWin ();
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

	// 順位を確認
	void checkWin(){
		ArrayList zeroteam = new ArrayList ();
		for (int i = 0; i < Teams.Length; i++) {
			Team team = Teams [i];
			if (team.PlayerValue <= 0) {
				if (WinNumbers [i] == 0) {
					zeroteam.Add (i);
				}
			}
		}
		if (zeroteam.Count > 0) {
			NextWinNumber -= zeroteam.Count - 1;
			for (int i = 0; i < zeroteam.Count; i++) {
				int n = (int)zeroteam [i];
				WinNumbers [n] = NextWinNumber;
			}
			NextWinNumber -= 1;
			if (NextWinNumber <= 1) {
				for (int i = 0; i < Teams.Length; i++) {
					Team team = Teams [i];
					if (WinNumbers [i] == 0) {
						WinNumbers [i] = 1;
						break;
					}
				}
				isGameEnd = true;
			}
		}
	}

	public void setTeamValue(int value){
		Teams = new Team[value];
		WinNumbers = new int[value];
		NextWinNumber = value;
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
