using UnityEngine;
using System.Collections;

public class Rotor : MonoBehaviour {

	public Vector3 RotSpeed = new Vector3 ();
	
	// Update is called once per frame
	void Update () {
		rigidbody.rotation = Quaternion.Euler(rigidbody.rotation.eulerAngles + RotSpeed * Time.deltaTime);
	}
}
