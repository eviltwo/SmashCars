using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Team {
	public int TeamNumber;
	public Color TeamColor;
	public int PlayerValue;
	public bool isBossUser;
	public bool isCamera;
	public int InputNumber;
	public string BossName;
	public GameObject[] TeamPlayers;
}
