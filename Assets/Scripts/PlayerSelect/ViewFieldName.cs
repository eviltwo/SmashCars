﻿using UnityEngine;
using System.Collections;

public class ViewFieldName : MonoBehaviour {

	int nowselect = -1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (nowselect != PlayerSelectManager.Instance.FieldSelect) {
			nowselect = PlayerSelectManager.Instance.FieldSelect;
			guiText.text = PlayerSelectManager.Instance.FieldName [nowselect];
		}
	}
}
