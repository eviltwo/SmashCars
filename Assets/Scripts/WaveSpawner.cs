using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour {
	public GameObject EffectPrefab;

	int TeamNum = 0;
	// Use this for initialization
	void Start () {
		spawnEffect ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartSet(int team){
		TeamNum = team;
	}

	void spawnEffect(){
		GameObject effect = (GameObject)Instantiate (EffectPrefab);
		effect.transform.position = transform.position;
		effect.SendMessage ("StartSet", TeamNum, SendMessageOptions.DontRequireReceiver);
	}
}
