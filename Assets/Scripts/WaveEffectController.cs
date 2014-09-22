using UnityEngine;
using System.Collections;

public class WaveEffectController : MonoBehaviour {
	public GameObject LinePrefab;
	public int LineValue = 20;
	public float DistMax = 5.0f;
	public float Height = 0.5f;
	public float SpeedMlt = 0.1f;
	public float ScaleMax = 5;

	float Dist = 0;
	GameObject[] Lines;
	// Use this for initialization
	void Start () {
		spawnLine ();
		moveLine ();
	}
	
	// Update is called once per frame
	void Update () {
		Dist += (DistMax - Dist) * SpeedMlt;
		moveLine ();
	}

	void spawnLine(){
		Lines = new GameObject[LineValue];
		for (int i = 0; i < LineValue; i++) {
			GameObject line = (GameObject)Instantiate (LinePrefab);
			line.transform.parent = transform;
			Lines [i] = line;
		}
	}

	void moveLine(){
		for (int i = 0; i < LineValue; i++) {
			float raneuler = Random.Range (0f,360f);
			float rad = raneuler * (Mathf.PI / 180); 
			Vector3 vec = new Vector3 (Mathf.Sin (rad), 0, Mathf.Cos (rad));
			Vector3 pos = vec * Dist + Vector3.up * Height;
			Lines [i].transform.localPosition = pos;
			Lines [i].transform.LookAt (transform);
			float mlt = Random.Range (0.1f, 1);
			Vector3 scl = Lines [i].transform.localScale;
			scl.x = ScaleMax * mlt;
			Lines [i].transform.localScale = scl;
			Lines [i].transform.Rotate (0, 0, 90f * Random.Range (-(1-mlt), (1-mlt)));
		}
	}
}
