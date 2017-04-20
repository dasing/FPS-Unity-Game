﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillSelf : MonoBehaviour {

	float aliveTime = 1;


	void Start () {
		Invoke ("DestroySelf", aliveTime);
	}
	
	public void DestroySelf(){
		GameObject.Destroy (this.gameObject);
	}
}