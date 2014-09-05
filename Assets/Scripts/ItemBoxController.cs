using UnityEngine;
using System.Collections;

public class ItemBoxController : MonoBehaviour {

	public GameObject Model;
	public float RespawnTimeMax = 5.0f;
	public float RotSpeed = 15.0f;
	public GameObject BreakPrefab;

	bool visible = true;
	float respawntime = 0;
	// Use this for initialization
	void Start () {
		ItemBoxManager.Instance.addItemBox (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (visible) {
			transform.Rotate (0, RotSpeed * Time.deltaTime, 0);
		} else {
			respawntime += Time.deltaTime;
			if (respawntime >= RespawnTimeMax) {
				visible = true;
				Model.renderer.enabled = true;
			}
		}
	}

	void OnTriggerEnter(Collider collider){
		if (visible) {
			collider.SendMessage ("getItem", SendMessageOptions.DontRequireReceiver);

			visible = false;
			respawntime = 0;
			Model.renderer.enabled = false;

			GameObject effect = (GameObject)Instantiate (BreakPrefab);
			effect.transform.position = transform.position;
			effect.particleEmitter.localVelocity = collider.rigidbody.velocity;
		}
	}

	public bool isVisible(){
		return visible;
	}
}
