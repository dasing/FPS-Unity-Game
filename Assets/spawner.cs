using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {

	private bool activate = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 pos = new Vector3 ( 162.2342f, 9.48f, -286.82f );
		if (Input.GetMouseButton (0) && !activate ) {

			activate = true;
			Object prefab = Resources.Load ("Circle");
			GameObject circle = Instantiate ( prefab, pos, Quaternion.identity  ) as GameObject;

		}
		
	}
}
