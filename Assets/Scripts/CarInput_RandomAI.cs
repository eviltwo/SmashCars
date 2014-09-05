using UnityEngine;
using System.Collections;

public class CarInput_RandomAI : MonoBehaviour {

	Vector2 dirinput = Vector2.zero;
	float randomtime = 0;
	float angle = 0;
	float timemax = 0;
	CarController cController;
	// Use this for initialization
	void Start () {
		cController = GetComponent<CarController> ();
	}

	// Update is called once per frame
	void Update () {
		dirinput.x = angle;
		dirinput.y = 0;
		cController.setInput (dirinput);

		randomtime += Time.deltaTime;
		if (randomtime >= timemax) {
			randomtime = 0;
			angle = Random.Range (-1f, 1f);
			timemax = 2.0f * (1 - Mathf.Abs (angle)) + 0.1f;
		}
	}
}
