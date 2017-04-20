using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

	public float FlyingSpeed;
	public float LifeTime;
	public GameObject explosion;
	public AudioSource bulletAudio;


	public void InitAndShoot(Vector3 Direction ){
		Rigidbody rigidbody = this.GetComponent<Rigidbody> ();
		rigidbody.velocity = Direction * FlyingSpeed;
		Invoke ("KillYourself", LifeTime );
	}

	public float damageValue = 15;

	public void KillYourself(){
		GameObject.Destroy (this.gameObject);
	}

	void OnTriggerEnter(Collider other){
		
		other.gameObject.SendMessage ("Hit", damageValue);

		explosion.gameObject.transform.parent = null;
		explosion.gameObject.SetActive (true);
		bulletAudio.pitch = Random.Range (0.8f, 1);
		KillYourself ();
	}

}
