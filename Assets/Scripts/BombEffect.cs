using UnityEngine;
using System.Collections;

public class BombEffect : MonoBehaviour {

	public GameObject BeamPrefab;
	public int BeamValue = 3;
	public GameObject SmokePrefab;
	public GameObject BallPrefab;

	int TeamNum;
	// Use this for initialization
	void Start () {
		createBeam ();
		GameObject temp;
		Color c = PlayerManager.Instance.Teams [TeamNum].TeamColor;
		c.a = 1;
		temp = Create (SmokePrefab);
		temp.GetComponent<ParticleRenderer>().material.SetColor("_TintColor",c);
		temp = Create (BallPrefab);
		temp.renderer.material.SetColor("_TintColor",c);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StartSet(int team){
		TeamNum = team;
	}

	void createBeam(){
		for (int i = 0; i < BeamValue; i++) {
			GameObject beam = Create (BeamPrefab);
			Color c = PlayerManager.Instance.Teams [TeamNum].TeamColor;
			c.a = 1;
			beam.transform.FindChild("Quad").gameObject.renderer.material.SetColor("_TintColor",c);
		}
	}

	GameObject Create(GameObject prefab){
		GameObject effect = (GameObject)Instantiate (prefab);
		effect.transform.position = transform.position;
		return effect;
	}
}
