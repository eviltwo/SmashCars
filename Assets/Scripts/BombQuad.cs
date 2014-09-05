using UnityEngine;
using System.Collections;

public class BombQuad : MonoBehaviour {

	public Vector2 MinSize = new Vector2 (0.2f,0.5f);
	public Vector2 MaxSize = new Vector2 (0.5f,2.0f);
	public float MinDist = 0;
	public float MaxDist = 2;
	// Use this for initialization
	void Start () {
		Vector3 angle = new Vector3 ();
		angle.x = Random.Range (0f,360f);
		angle.y = Random.Range (0f,360f);
		angle.z = Random.Range (0f,360f);
		transform.localEulerAngles = angle;

		Vector3 scl = new Vector3 (1,1,1);
		scl.x = Random.Range (MinSize.x,MaxSize.x);
		scl.z = Random.Range (MinSize.y,MaxSize.y);
		transform.localScale = scl;

		float dist = Random.Range (MinDist, MaxDist);
		transform.position += transform.forward * dist;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
