using UnityEngine;
using System.Collections;

public class ViewTeamColor : MonoBehaviour {
	public int TeamNum;
	public float ColorMultiple = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Color c = PlayerSelectManager.Instance.getTeamColor(TeamNum);
		float mlt = ColorMultiple;
		c.r += (1-c.r)*mlt;
		c.g += (1-c.g)*mlt;
		c.b += (1-c.b)*mlt;
		guiTexture.color = c;
	}
}
