using UnityEngine;
using System.Collections;

public class AudioManager : SingletonMonoBehaviour<AudioManager> {

	public GameObject ListenerCamera;
	public GameObject AudioPlayerPrefab;
	public float ListenDistanceMax = 20.0f;
	public float PanValue = 0.5f;

	int targetteam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/// サウンドを再生(位置計算)
	public void playSE(AudioClip clip, Vector3 pos){
		float dist = getMinDist (pos);
		if (dist <= ListenDistanceMax && !PlayerManager.Instance.isGameEnd) {
			float vol = 1 - dist / ListenDistanceMax;
			float pan = 0;
			if (CameraManager.Instance.ScreenValue >= 3 ) {
				int team = targetteam % 2;
				if (team == 0) {
					team = -1;
				}
				pan = PanValue * team;
			}
			GameObject audioplayer = (GameObject)Instantiate (AudioPlayerPrefab);
			audioplayer.GetComponent<AudioPlayer> ().setData (clip, true, false, vol, pan);
			audioplayer.GetComponent<AudioPlayer> ().playAudio ();
		}
	}
	/// サウンドを再生(音量指定)
	public void playSE(AudioClip clip, float vol){
		float pan = 0;
		GameObject audioplayer = (GameObject)Instantiate (AudioPlayerPrefab);
		audioplayer.GetComponent<AudioPlayer> ().setData (clip, true, false, vol, pan);
		audioplayer.GetComponent<AudioPlayer> ().playAudio ();
	}

	// 引数の座標から、一番近いボスを調べてその距離を返す
	float getMinDist(Vector3 pos){
		float mindist = Mathf.Infinity;
		Team[] teams = PlayerManager.Instance.getTeamData ();
		for (int i = 0; i < teams.Length; i++) {
			Team team = teams [i];
			if (team.BossNumber >= 0 && team.isCamera) {
				GameObject boss = team.TeamPlayers [team.BossNumber];
				if (boss) {
					float dist = Vector3.Distance (pos, boss.transform.position);
					if (dist < mindist) {
						mindist = dist;
						targetteam = i;
					}
				}
			}
		}
		return mindist;
	}
}
