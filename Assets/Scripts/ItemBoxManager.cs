using UnityEngine;
using System.Collections;

public class ItemBoxManager : SingletonMonoBehaviour<ItemBoxManager> {

	GameObject[] ItemBoxList = new GameObject[0];

	// Use this for initialization
	void Start () {
	
	}

	// 箱情報を追加
	public void addItemBox(GameObject obj){
		GameObject[] newbox = new GameObject[ItemBoxList.Length+1];
		for (int i = 0; i < ItemBoxList.Length; i++) {
			newbox [i] = ItemBoxList [i];
		}
		newbox [newbox.Length - 1] = obj;
		ItemBoxList = newbox;
	}

	// 箱情報を取得
	public GameObject[] getItemBoxList(){
		return ItemBoxList;
	}
}
