using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

	private Animator animatorController;
	public Transform rotateYTransform;
	public Transform rotateXTransform;
	public float rotateSpeed; 
	public float currentRotateX = 0;
	public float MoveSpeed;
	float currentSpeed = 0;

	public JumpSensor JumpSensor;
	public float JumpSpeed;
	public GunManager gunManager;
	public GameUIManager uiManager;
	public int hp = 100;
	private AudioSource[] Audios;
	private AudioSource switchWeaponAudio;
	private AudioSource walkingAudio;

	public Rigidbody rigidBody;
	public GameObject[] gunList;
	public GunManager[] gunManagers;
	private int active_gun;
	private bool pressed = false;


	// Use this for initialization
	void Start () {
		animatorController = this.GetComponent<Animator> ();
		Audios = this.GetComponents<AudioSource> ();

		if (Audios [0].clip.name == "switchWeapon") {
			switchWeaponAudio = Audios [0];
			walkingAudio = Audios [1];
		} else {
			walkingAudio = Audios [0];
			switchWeaponAudio = Audios [1];
		}

		active_gun = 1;

	}
		
	public void switchWeapon(){


		if (active_gun == 1) {
			gunList [active_gun - 1].SetActive (false);
			active_gun = 2;
			gunList [active_gun - 1].SetActive (true);
		} else {
			gunList [active_gun - 1].SetActive (false);
			active_gun = 1;
			gunList [active_gun - 1].SetActive (true);
		}


		
	}

	public void Hit(int value){
		
		if (hp <= 0) {
			return;
		}

		hp -= value;
		uiManager.SetHP (hp);

		if (hp > 0) {
			uiManager.PlayHitAnimation ();
		} else {
			uiManager.PlayerDieAnimation ();
			rigidBody.gameObject.GetComponent<Collider> ().enabled = false;
			rigidBody.useGravity = false;
			rigidBody.velocity = Vector3.zero;
			this.enabled = false;
			rotateXTransform.transform.DOLocalRotate (new Vector3( -60,0,0 ), 0.5f );
			rotateYTransform.transform.DOLocalMoveY (-1.5f, 0.5f ).SetRelative(true);

		}
	}


	void Update () {


		//monitor switch weapon
		if (Input.GetKey (KeyCode.Alpha1)) {
			animatorController.SetTrigger ("switchWeapon");
			switchWeaponAudio.Play ();

		}

		Cursor.visible = false;

		if (Input.GetMouseButton (0)) {

			if (active_gun == 1)
				gunManagers [active_gun - 1].TryToTriggerGun1 ();
			else {
				if (!pressed) {
					gunManagers [active_gun - 1].TryToTriggerGun2 ();
					pressed = true;
				}
			}
		}

		if (Input.GetMouseButtonUp (0) && active_gun == 2 ) {
			gunManagers [active_gun - 1].unTriggerGun2 ();
			pressed = false;
		}

		Vector3 movDirection = Vector3.zero;
		float result = 0;

		if (Input.GetKey (KeyCode.W)) { 
			movDirection.z += 1;
			if( !walkingAudio.isPlaying )
				walkingAudio.Play ();
		}

		if (Input.GetKey (KeyCode.S)) { 
			movDirection.z -= 1;
			if( !walkingAudio.isPlaying )
				walkingAudio.Play ();
		}

		if (Input.GetKey (KeyCode.D)) {
			movDirection.x += 1;
			if( !walkingAudio.isPlaying )
				walkingAudio.Play ();
		}

		if (Input.GetKey (KeyCode.A)) {
			movDirection.x -= 1;
			if( !walkingAudio.isPlaying )
				walkingAudio.Play ();
		}

		if (Input.GetKeyUp (KeyCode.W)) {
			if( walkingAudio.isPlaying )
				walkingAudio.Stop ();
		}

		if (Input.GetKeyUp (KeyCode.S)) {
			if( walkingAudio.isPlaying )
				walkingAudio.Stop ();
		}

		if (Input.GetKeyUp (KeyCode.D)) {
			if( walkingAudio.isPlaying )
				walkingAudio.Stop ();
		}

		if (Input.GetKeyUp (KeyCode.A)) {
			if( walkingAudio.isPlaying )
				walkingAudio.Stop ();
		}
			
		movDirection = movDirection.normalized;

		//決定要給Animator的動畫參數
		if (movDirection.magnitude == 0 || !JumpSensor.IsCanJump() ) {
			currentSpeed = 0;
		} else {
			if (movDirection.z < 0) {
				currentSpeed = -MoveSpeed;
			} else {
				currentSpeed = MoveSpeed;
			}
		}
		animatorController.SetFloat ("Speed", currentSpeed);

		//轉換成世界座標的方向
		Vector3 worldSpaceDirection = movDirection.z * rotateYTransform.transform.forward + movDirection.x*rotateYTransform.transform.right;
		Vector3 velocity = rigidBody.velocity;
		velocity.x = worldSpaceDirection.x * MoveSpeed;
		velocity.z = worldSpaceDirection.z * MoveSpeed;

		if (Input.GetKey (KeyCode.Space) && JumpSensor.IsCanJump() ) {
			//Debug.Log ("here~~");
			velocity.y = JumpSpeed;
		}

		rigidBody.velocity = velocity;

		//計算滑鼠
		rotateYTransform.transform.localEulerAngles += new Vector3 ( 0, Input.GetAxis("Horizontal"), 0 )*rotateSpeed;
		currentRotateX += Input.GetAxis ("Vertical") * rotateSpeed;


		if (currentRotateX > 90) {
			currentRotateX = 90;
		} else if (currentRotateX < -90) {
			currentRotateX = -90;
		}

		rotateXTransform.transform.localEulerAngles = new Vector3 ( -currentRotateX, 0, 0);

		//currentSpeed = result;
		//rotateYTransform.transform.position += Time.deltaTime * currentSpeed * rotateYTransform.transform.forward;
		//this.transform.position += Time.deltaTime * currentSpeed * this.transform.forward;

	}
		
}
