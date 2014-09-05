using UnityEngine;
using System.Collections;

public class DestroyTimer : MonoBehaviour {
	public float DestroyTime = 3.0f;

	float destime = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		destime += Time.deltaTime;
		if (destime >= DestroyTime) {
			Destroy (this.gameObject);
		}
	}
}
