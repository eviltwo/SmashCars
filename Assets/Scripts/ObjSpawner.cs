using UnityEngine;
using System.Collections;

public class ObjSpawner : MonoBehaviour {
	public GameObject ObjPrefab;
	public int Value;
	public Vector3 CenterPos;
	public Vector3 RandomSize;
	public bool RandomRotY;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < Value; i++) {
			spawnRandom ();
		}
	}
	
	void spawnRandom(){
		GameObject obj = (GameObject)Instantiate (ObjPrefab);
		Vector3 pos = CenterPos;
		for (int i = 0; i < 3; i++) {
			pos [i] += Random.Range (-RandomSize [i], RandomSize [i]);
		}
		obj.transform.position = pos;
		if (RandomRotY) {
			obj.transform.Rotate (0,Random.Range(0,360),0);
		}
	}
}
