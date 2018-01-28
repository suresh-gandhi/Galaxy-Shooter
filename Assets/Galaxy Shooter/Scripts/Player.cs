using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[SerializeField]
	private bool canTripleShot = false;				// Ideally and following our standards we should use an underscore here and in the next line as well, but no worries.

	[SerializeField]
	private bool canSpeedBoost = false;

	[SerializeField]
	private bool haveShield = false;

	public int lives = 1;

	[SerializeField]
	private float speedMutiplyFactor = 2.0f;

	// we need another variable to see if we have the speed boost power up with us.

	[SerializeField]	
	private GameObject _laserPrefab;

	[SerializeField]
	private GameObject _explosionPrefab;

	[SerializeField]
	private GameObject _tripleShotPrefab;

	[SerializeField]
	private GameObject _shieldGameObject;

	[SerializeField]
	private GameObject[] _engines;

	// fireRate is 0.25f
	[SerializeField]
	private float _fireRate = 0.25f;

	[SerializeField]
	private float _nextFire = 0.0f;
	// Time.time 

	[SerializeField]
	private float _speed = 5.0f;

	private GameManager _gameManager;

	private UIManager _uiManager; 	

	private SpawnManager _spawnManager;

	private AudioSource _audioSource;

	private int hitCount = 0;

	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (0, 0, 0);

		_uiManager = GameObject.Find ("Canvas").GetComponent<UIManager> ();
	
		if (_uiManager != null) {			// just to make sure.
			_uiManager.UpdateLives(lives);
		}

		_gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();

		_spawnManager = GameObject.Find ("Spawn_Manager").GetComponent<SpawnManager>();

		if (_spawnManager != null) {
			_spawnManager.StartSpawnRoutines ();
		}

		_audioSource = GetComponent<AudioSource> ();

		hitCount = 0;

	}
	
	// Update is called once per frame
	void Update () {
		Movement ();

		// if the space key is pressed
		// spawn the laser at the player position

		if (Input.GetKeyDown (KeyCode.Space) || Input.GetMouseButtonDown(0)) {
			Shoot ();
		}	
	}

	private void Shoot(){

		// if triple shot
		// shoot 3 lasers
		// else
		// shoot 1

		if (Time.time > _nextFire) {
			_audioSource.Play ();
			if (canTripleShot == true) {
				Instantiate (_tripleShotPrefab, transform.position, Quaternion.identity);
			} else {
				// just fire one laser normally
				Instantiate (_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
			}
			_nextFire = Time.time + _fireRate;			// this is done as we dont want the inside if to run for the next 0.25 seconds. Think Time.time as time bar passing continously and nextFire as the steps that you take. The loop wont get executed till we cross the next step and once we cross the next step our step moves further forward so that for the next 0.25 seconds we cant fire again and this goes on.
		}												// In this way the player can no longer spam the fire button.
	}

	private void Movement(){
		float horizontalInput = Input.GetAxis ("Horizontal");
		float verticalInput = Input.GetAxis ("Vertical");

		if (canSpeedBoost == true) {
			transform.Translate (Vector3.right * _speed * speedMutiplyFactor * horizontalInput * Time.deltaTime);
			transform.Translate (Vector3.up * _speed * speedMutiplyFactor * verticalInput * Time.deltaTime);
			// Debug.Log (verticalInput + "-" + horizontalInput);	
		} else {
			transform.Translate (Vector3.right * _speed * horizontalInput * Time.deltaTime);
			transform.Translate (Vector3.up * _speed * verticalInput * Time.deltaTime);
			// Debug.Log (verticalInput + "-" + horizontalInput);
		}

		if(transform.position.y > 0){
			transform.position = new Vector3 (transform.position.x, 0, 0);
		}
		else if (transform.position.y < -4.2f) { // as both of them cannot happen simultaneously so using else if here saves computation.
			transform.position = new Vector3 (transform.position.x, -4.2f, 0);
		} 

		if (transform.position.x > 9.5f) {
			transform.position = new Vector3 (-9.5f, transform.position.y, 0);
		}
		else if(transform.position.x < -9.5f){
			transform.position = new Vector3 (9.5f, transform.position.y, 0);
		}
	}

	public void Damage(){
		// subtract 1 life from the player
		// Debug.Log("The number of lives are" + lives);
		// is player has shields then do nothing

	
		if(haveShield == true){			
			haveShield = false;		// because this was our free hit.
			_shieldGameObject.SetActive(false);
			return;
		}

		hitCount++;

		if (hitCount == 1) {
			// turn left engine_failure on
			_engines[0].SetActive(true);
		} else if (hitCount == 2) {
			// turn right engine_failure on
			_engines[1].SetActive(true);
		}


		lives = lives - 1;
		_uiManager.UpdateLives (lives);

		// if lives < 1 (meaning 0)
		if (lives < 1) {
			// Debug.Log ("Inside the if statement in the damage function");
			Instantiate (_explosionPrefab, transform.position, Quaternion.identity);
			_gameManager.gameOver = true;
			_uiManager.ShowTitleScreen ();
			Destroy (this.gameObject);
		}
		// destroy this object
	}

	public void TripleShotPowerUpOn(){
		canTripleShot = true;
		StartCoroutine (TripleShotPowerDownRoutine ());
	}

	IEnumerator TripleShotPowerDownRoutine(){
		yield return new WaitForSeconds (5.0f);
		canTripleShot = false;
	}

	// method to enable up the power up
	public void SpeedBoostPowerUpOn(){
		canSpeedBoost = true;
		StartCoroutine(SpeedBoostPowerDownRoutine());
	}

	public void EnableShields(){
		haveShield = true;
		_shieldGameObject.SetActive (true);
	}

	// IEnumerator to power down is needed for the speed boost
	IEnumerator SpeedBoostPowerDownRoutine(){
		yield return new WaitForSeconds (5.0f);
		canSpeedBoost = false;
	}

}
