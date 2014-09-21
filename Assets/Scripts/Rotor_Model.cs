using UnityEngine;
using System.Collections;

public class Rotor_Model : MonoBehaviour {

	public Vector3 RotSpeed = new Vector3 ();

	// Update is called once per frame
	void Update () {
		transform.Rotate(RotSpeed * Time.deltaTime);
	}
}
