using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class enemySolider : MonoBehaviour {

	private float actionKeepingTime = 0;
	private Animator animationController;
	private int action = 0; // 0 -> idle, 1 -> walking
	private bool hit = false;
	private float speed = 1.0f;
	private GameObject followTarget;
	private Rigidbody rigidbody;

	private AudioSource gunShoot;
	public GameObject gun;
	public GameObject muzzleFlash;
	public GameObject bulletCandidate;
	public float MinimumShootPeriod;
	public float muzzleShowPeriod;
	private float shootCounter = 0;
	private float muzzleCounter = 0;
	private float CurrentHP;

	void Start () {
		animationController = GetComponent<Animator> ();
		rigidbody = GetComponent<Rigidbody> ();
		CurrentHP = 100;
		gunShoot = GetComponent<AudioSource> ();
	}
	

	void Update () {

		AnimatorStateInfo currentState = animationController.GetCurrentAnimatorStateInfo(0);

//		Debug.Log (currentState.IsName("Idle"));
//		Debug.Log (currentState.IsName("Run"));

		if (hit) {
			
			Vector3 lookAt = followTarget.transform.position;
			lookAt.y = this.transform.position.y;

			this.transform.LookAt (lookAt);
			animationController.SetBool( "Hit", true );
			
		} else {

			if (actionKeepingTime > 0 ) {
				if (action == 1) {
					//Debug.Log ("moving");
					if (currentState.IsName ("randomRun"))
						transform.position += transform.forward * speed * Time.deltaTime;

					//			} else {
					//				if(currentState.IsName("idle"))
					//					animationController.SetTrigger("idle");
				}
				actionKeepingTime -= Time.deltaTime;

			} else {

				actionKeepingTime = 5;
				action = Random.Range ( 0, 2 );
				Debug.Log ("change action to " + action );
				if (action == 0) {
					//idle
					animationController.SetTrigger("idle");

					currentState = animationController.GetCurrentAnimatorStateInfo(0);
					if (currentState.IsName ("Idle"))
						Debug.Log ("Idle");
					else
						Debug.Log ("randomRun");

				} else if( action == 1 ){


					//walking
					animationController.SetTrigger("randomRun");
					Quaternion randomDirection = Quaternion.Euler( 0, Random.value*360, 0 );
					this.transform.rotation =  Quaternion.Slerp(transform.rotation, randomDirection, Time.deltaTime * 1000);


					currentState = animationController.GetCurrentAnimatorStateInfo(0);
					if( currentState.IsName ("randomRun"))
						Debug.Log ("randomRun");
					else
						Debug.Log ("Idle");

				}
			}
		}




	}

	public void OnTriggerEnter( Collider other ){
		
//		Debug.Log ("enter");
//		Debug.Log (other.name);

		if (other.name == "PlayerRoot") {
			hit = true;
			followTarget = other.gameObject;
		}



	}

	public void Hit(float value ){
		

		CurrentHP -= value;
		animationController.SetFloat ("HP", CurrentHP );
		animationController.SetTrigger ("Hurt");

	}

	public void ContinousHit( float value ){


		CurrentHP -= value;

		animationController.SetFloat ("HP", CurrentHP );
		animationController.SetTrigger ("Hurt");

	}

	void OnParticleCollision( GameObject other ){
		ContinousHit (2.0f);
	}

	private void BuryTheBody(){
		GameObject.Destroy (this.gameObject );
	}

	public void Attack(){


		gunShoot.Play ();

		muzzleCounter = muzzleShowPeriod;

		shootCounter = MinimumShootPeriod;
		GameObject newBullet = GameObject.Instantiate (bulletCandidate);
		enemyBullet bullet = newBullet.GetComponent<enemyBullet> ();
		bullet.transform.position = muzzleFlash.transform.position;
		bullet.transform.rotation = muzzleFlash.transform.rotation;

		bullet.InitAndShoot (muzzleFlash.transform.forward);

	}

	public void OnTriggerExit( Collider other ){

		if ( other.name == "PlayerRoot") {
//			Debug.Log ("exit");
//			Debug.Log (other.name);
			hit = false;
			animationController.SetBool( "Hit", false );
			actionKeepingTime = 0;
		}

	}


}


