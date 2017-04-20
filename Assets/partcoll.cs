using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partcoll : MonoBehaviour {

	void OnParticleCollision( GameObject other ){
		Debug.Log ("particle hit1");
		Debug.Log (other.name);
	}

}
