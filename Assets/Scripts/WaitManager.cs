using UnityEngine;
using System.Collections;

public class WaitManager : SingletonMonoBehaviour<WaitManager> {
	public float StartTimeMax = 3.0f;
	public float DeadTimeMax = 20.0f;
	public float[] WaitTime;
	public bool IsGameStart = false;

	float starttime = 0;
	// Use this for initialization
	void Start () {
		setStartValueAll ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!IsGameStart) {
			starttime += Time.deltaTime;
			if (starttime >= StartTimeMax) {
				starttime = 0;
				IsGameStart = true;
			}
		}
		countWaitTime ();
	}

	// カウントダウン
	void countWaitTime(){
		for (int i = 0; i < WaitTime.Length; i++) {
			WaitTime [i] = Mathf.Max (0,WaitTime[i]-Time.deltaTime);
		}
	}

	// ゲーム開始時のカウントをセット
	void setStartValueAll(){
		Team[] teams = PlayerManager.Instance.getTeamData ();
		int teamvalue = teams.Length;
		int playervalue = teams [0].PlayerValue;
		WaitTime = new float[teamvalue];
		for (int i = 0; i < teamvalue; i++) {
			WaitTime [i] = StartTimeMax;
		}
	}

	// 待機時間を取得
	public float getWaitTime(int team){
		return WaitTime[team];
	}
}
