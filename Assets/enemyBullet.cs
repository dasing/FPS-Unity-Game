using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour {

	public float FlyingSpeed;
	public float LifeTime;
	private AudioSource bulletAudio;



	public void InitAndShoot(Vector3 Direction ){
		Rigidbody rigidbody = this.GetComponent<Rigidbody> ();
		rigidbody.velocity = Direction * FlyingSpeed;
		Invoke ("KillYourself", LifeTime );
	}

	public float damageValue = 15;

	public void Start(){
		bulletAudio = GetComponent<AudioSource> ();

	}

	public void KillYourself(){
		GameObject.Destroy (this.gameObject, 1000 );
	}

	void OnTriggerEnter(Collider other){


		if( other.name == "PlayerRoot" )
			other.gameObject.transform.GetChild (0).GetChild (0).SendMessage ("Hit", damageValue);

		//bulletAudio.pitch = Random.Range (0.8f, 1);
		//bulletAudio.Play ();
		KillYourself ();
	}



}
