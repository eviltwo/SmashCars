using UnityEngine;
using System.Collections;

[System.Serializable]
public struct Team {
	public int TeamNumber;
	public Color TeamColor;
	public int PlayerValue;
	public bool isBossUser;
	public int InputNumber;
	public GameObject[] TeamPlayers;
}
