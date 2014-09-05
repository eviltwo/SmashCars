using UnityEngine;
using System.Collections;

public class TestForward : MonoBehaviour {

	public GameObject TargetCar;

	CarController cController;
	// Use this for initialization
	void Start () {
		cController = TargetCar.GetComponent<CarController> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 fwd = cController.getForward ();
		transform.position = TargetCar.transform.position;
		transform.LookAt (transform.position + fwd);
		transform.position += transform.forward * 2f;
	}
}
