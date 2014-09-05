using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	public GameObject[] ItemPrefab;
	public float EnemyCheckTimeMax = 0.1f;
	public float ItemCheckTimeMax = 0.1f;
	public float OutEyeAngle = 45.0f;
	public float ItemBoxHeightMax = 0.5f;
	public float WallHeight = 0.5f;

	GameObject[] Items;
	bool HaveItem = false;
	int ItemType = 0;
	int time = 0;
	GameObject[] Enemy = new GameObject[0];
	GameObject FindItemBox;
	float enemychecktime = 0;
	float itemchecktime = 0;
	CarController cController;
	// Use this for initialization
	void Start () {
		cController = GetComponent<CarController> ();
		createItem ();
	}
	
	// Update is called once per frame
	void Update () {
		/*time++;
		if (time > 300) {
			time = 0;
			HaveItem = true;
		}*/

		enemychecktime += Time.deltaTime;
		if (enemychecktime >= EnemyCheckTimeMax && HaveItem) {
			enemychecktime = 0;
			searchEnemy ();
		}

		itemchecktime += Time.deltaTime;
		if (itemchecktime >= ItemCheckTimeMax && !HaveItem) {
			itemchecktime = 0;
			searchItemBox ();
		}

	}

	// アイテム追加
	void getItem(){
		HaveItem = true;
		ItemType = 0;
	}

	// 子にアイテムを追加
	void createItem(){
		Items = new GameObject[ItemPrefab.Length];
		for (int i = 0; i < ItemPrefab.Length; i++) {
			GameObject item = (GameObject)Instantiate (ItemPrefab [i]);
			item.transform.parent = transform;
			item.SendMessage ("StartSet", i);
			Items [i] = item;
		}
	}

	// アイテム使用
	public void useItem(){
		if (HaveItem) {
			Items [ItemType].SendMessage ("Use");
		}
	}

	// アイテム使用済み
	void deleteItem(){
		HaveItem = false;
	}

	// アイテムを持っているか
	public bool getHaveItem(){
		return HaveItem;
	}

	public int getItemType(){
		return ItemType;
	}

	// 視界内の敵を捜索
	void searchEnemy(){
		ArrayList find = new ArrayList ();
		Team[] teams = PlayerManager.Instance.getTeamData ();
		int myteam = cController.TeamNum;
		for (int i = 0; i < teams.Length; i++) {
			Team team = teams [i];
			if (team.TeamNumber != myteam) {
				for (int j = 0; j < team.PlayerValue; j++) {
					GameObject enemy = team.TeamPlayers [j];
					Vector3 myvec = cController.getForward ();
					Vector3 envec = (enemy.transform.position - transform.position);
					envec = envec / envec.magnitude;
					float angle = Vector3.Angle (myvec, envec);
					if (angle < OutEyeAngle) {
						find.Add (enemy);
					}
				}
			}
		}
		GameObject[] list = new GameObject[find.Count];
		for (int i = 0; i < list.Length; i++) {
			list [i] = (GameObject)find [i];
		}
		Enemy = list;
	}
	// 捜索した敵を取得
	public GameObject[] getFindEnemy(){
		return Enemy;
	}

	// 視界内のアイテムボックスを捜索
	void searchItemBox(){
		ArrayList find = new ArrayList ();
		ArrayList distlist = new ArrayList ();
		GameObject[] boxlist = ItemBoxManager.Instance.getItemBoxList ();
		for (int i = 0; i < boxlist.Length; i++) {
			GameObject box = boxlist [i];
			Vector3 myvec = cController.getForward ();
			Vector3 envec = (box.transform.position - transform.position);
			float dist = envec.magnitude;
			envec = envec / dist;
			float angle = Vector3.Angle (myvec, envec);
			if (angle < OutEyeAngle && Mathf.Abs(myvec.y-envec.y) < ItemBoxHeightMax) {
				Vector3 stpos = transform.position;
				if (box.GetComponent<ItemBoxController> ().isVisible ()) {
					Vector3 stdir = (box.transform.position - transform.position);
					dist = stdir.magnitude;
					stdir = stdir / dist;
					RaycastHit hit;
					LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
					if (!Physics.Raycast (stpos, stdir, out hit, dist, mask)) {
						find.Add (box);
						distlist.Add (dist);
					}
				}
			}
		}
		GameObject minbox = null;
		float mindist = Mathf.Infinity;
		for (int i = 0; i < find.Count; i++) {
			float nowdist = (float)distlist [i];
			if (nowdist < mindist) {
				mindist = nowdist;
				minbox = (GameObject)find [i];
			}
		}
		FindItemBox = minbox;

		// 坂になっているか判定
		if (FindItemBox) {
			Vector3 stpos = transform.position;
			Vector3 tarpos = FindItemBox.transform.position;
			tarpos.y = transform.position.y;
			Vector3 stdir = tarpos-transform.position;
			stdir = stdir / stdir.magnitude;
			float checkdist = Vector3.Distance (transform.position, FindItemBox.transform.position);
			RaycastHit hit;
			LayerMask mask = (1 << LayerMask.NameToLayer ("Field"));
			if (Physics.Raycast (stpos, stdir, out hit, checkdist, mask)) {
				float mlt = 1 - hit.normal.y;
				if (mlt > WallHeight) {
					FindItemBox = null;
				}
			} else {
				// ボックスが宙に浮いてて取れない
				Vector3 tardir = FindItemBox.transform.position-transform.position;
				tardir = tardir / stdir.magnitude;
				if (tardir.y - cController.getForward().y > 0.3f) {
					FindItemBox = null;
				}
			}
		}
	}
	// 捜索したアイテムボックスを取得
	public GameObject getFindItemBox(){
		return FindItemBox;
	}
}
