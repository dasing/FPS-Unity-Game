using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GunManager : MonoBehaviour {

	public float MinimumShootPeriod;
	public float muzzleShowPeriod;

	private float shootCounter = 0;
	private float muzzleCounter = 0;

	public GameObject muzzleFlash;
	public GameObject bulletCandidate;
	private GameObject fireSprayObj;
	private ParticleSystem fireSpray;
	private AudioSource gunShootSound;
	private Vector3 shootPosition;
	private Quaternion shootRotation;


	public void Start(){

		gunShootSound = this.GetComponent<AudioSource> ();
		shootPosition = new Vector3 ( 0.012f, 0.088f, 0.319f );
		shootRotation = Quaternion.Euler ( -0.2f, -5.423f, 75.5f );
	}


	public void TryToTriggerGun1(){
		
		if (shootCounter <= 0) {
			
				gunShootSound.Stop ();
				gunShootSound.pitch = Random.Range (0.8f, 1);
				gunShootSound.Play ();


				this.transform.DOShakeRotation (MinimumShootPeriod * 0.8f, 3f);

				muzzleCounter = muzzleShowPeriod;
				muzzleFlash.transform.localEulerAngles = new Vector3 (0, 0, Random.Range (0, 360));

				shootCounter = MinimumShootPeriod;
				GameObject newBullet = GameObject.Instantiate (bulletCandidate);
				BulletScript bullet = newBullet.GetComponent<BulletScript> ();
				bullet.transform.position = muzzleFlash.transform.position;
				bullet.transform.rotation = muzzleFlash.transform.rotation;

				bullet.InitAndShoot (muzzleFlash.transform.forward);
		}



	}

	public void TryToTriggerGun2(){

		Object prefab = Resources.Load ("Fire Spray");
		fireSprayObj = Instantiate ( prefab, Vector3.zero, Quaternion.identity ) as GameObject;
		fireSprayObj.transform.parent = this.transform;
		fireSprayObj.transform.localPosition = shootPosition;
		fireSprayObj.transform.localRotation = shootRotation;
		fireSpray = fireSprayObj.GetComponent<ParticleSystem> ();
		if (!fireSpray.isPlaying) {
			fireSpray.Play ();
			var em = fireSpray.emission;
			em.enabled = true;
		}

		if (!gunShootSound.isPlaying) {
			gunShootSound.Play ();
		}

	}

	public void unTriggerGun2(){
		
		if (fireSpray.isPlaying) {
			fireSpray.Stop ();
			var em = fireSpray.emission;
			em.enabled = false;
			Destroy(fireSprayObj, 3);
		}


		if (gunShootSound.isPlaying) {
			gunShootSound.Stop ();
		}
	}


	
	// Update is called once per frame
	void Update () {

		if (string.Equals (this.name, "gun")) {

			if (shootCounter > 0) {
				shootCounter -= Time.deltaTime;
			}

			if (muzzleCounter > 0) {
				muzzleFlash.gameObject.SetActive (true);
				muzzleCounter -= Time.deltaTime;
			} else {
				muzzleFlash.gameObject.SetActive (false);
			}

		} else {

//			if (fireSpray.isPlaying ) {
//				Debug.Log ("playing");
//			}
		}

	}
}
