using UnityEngine;
using System.Collections;

public class GameTimeManager : SingletonMonoBehaviour<GameTimeManager> {
	public float GameTimeMax = 300;

	float GameTime = 0;
	bool TimeOver = false;
	// Use this for initialization
	void Start () {
		GameTime = GameTimeMax;
	}
	
	// Update is called once per frame
	void Update () {
		if (WaitManager.Instance.IsGameStart && !PlayerManager.Instance.isGameEnd) {
			GameTime = Mathf.Max (0, GameTime - Time.deltaTime);
		}
		if (GameTime <= 0 && !TimeOver) {
			TimeOver = true;
		}
	}

	public float getTime(){
		return GameTime;
	}

	public bool isTimeOver(){
		return TimeOver;
	}
}
