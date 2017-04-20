using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class troll : MonoBehaviour {

	void OnParticleCollision( GameObject other ){
		Debug.Log ("particle hit2");
		Debug.Log (other.name);
	}
}
