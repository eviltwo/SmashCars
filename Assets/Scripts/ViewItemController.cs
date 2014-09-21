using UnityEngine;
using System.Collections;

public class ViewItemController : MonoBehaviour {
	public GameObject ViewItemPrefab;
	public Texture[] Icon;
	public Vector3 IconPos;
	public Vector3 IconScale;

	GameObject[] Items;
	// Use this for initialization
	void Start () {
		createScreen ();
	}

	// Update is called once per frame
	void Update () {
		moveScreenStatus ();
	}

	// 生成
	void createScreen(){
		int teamvalue = PlayerManager.Instance.getTeamData ().Length;
		Items = new GameObject[teamvalue];
		for (int i = 0; i < teamvalue; i++) {
			GameObject item = (GameObject)Instantiate (ViewItemPrefab);
			item.transform.parent = transform;
			int layer = LayerMask.NameToLayer ("UI_" + i);
			setChildLayer (item, layer);
			item.SetActive (false);
			Items [i] = item;
		}
	}

	// 状態を変更
	void moveScreenStatus(){
		for (int i = 0; i < Items.Length; i++) {
			int boss = PlayerManager.Instance.getTeamData () [i].BossNumber;
			bool active = false;
			if (boss >= 0 && PlayerManager.Instance.getTeamData () [i].isCamera) {
				GameObject bossplayer = PlayerManager.Instance.getTeamData () [i].TeamPlayers [boss];
				if (bossplayer) {
					ItemController iController = bossplayer.GetComponent<ItemController> ();
					if (iController.getHaveItem ()) {
						int type = iController.getItemType ();
						GameObject icon = Items [i].transform.FindChild ("Icon").gameObject;
						icon.guiTexture.texture = Icon [type];
						Color c = PlayerManager.Instance.Teams [i].TeamColor;
						float cmlt = 0.4f;
						c.r += (1-c.r)*cmlt;
						c.g += (1-c.g)*cmlt;
						c.b += (1-c.b)*cmlt;
						icon.guiTexture.color = c;

						Rect rect = CameraManager.Instance.getCamera(i).camera.rect;
						float mlt = rect.height / rect.width;
						Vector3 scl = IconScale;
						scl.x *= mlt;
						icon.transform.localScale = scl;
						icon.transform.localPosition = IconPos;
						active = true;
					}
				}
			}
			Items [i].SetActive (active);
		}
	}

	// 全ての子のレイヤーを変更
	void setChildLayer(GameObject obj, int layer){
		obj.layer = layer;
		for (int i = 0; i < obj.transform.childCount; i++) {
			GameObject child = obj.transform.GetChild (i).gameObject;
			setChildLayer (child, layer);
		}
	}
}
