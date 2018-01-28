using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

	[SerializeField]
	private GameObject _enemyExplosionPrefab;

	// variable for our speed
	private float _speed = 5.0f;

	private UIManager _uiManager;

	[SerializeField]
	private AudioClip _clip;

	// Use this for initialization
	void Start () {
		_uiManager = GameObject.Find ("Canvas").GetComponent<UIManager> ();
		// _audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		// move down
		transform.Translate(Vector3.down * _speed * Time.deltaTime);

		// when off the screen on the bottom
		// we are going to respawn at the top with a new x position between the bounds of the screen.
		if(transform.position.y < -7){
			float randomX = Random.Range (-7f, 7f);
			transform.position = new Vector3 (randomX, 7, 0);
		}
	}

	private void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Laser") {
			if (other.transform.parent != null) {
				Destroy (other.transform.parent.gameObject);
			}
			Destroy (other.gameObject);
			Instantiate (_enemyExplosionPrefab, transform.position, Quaternion.identity);
			_uiManager.UpdateScore ();
			AudioSource.PlayClipAtPoint (_clip, Camera.main.transform.position, 1f);
			// _audioSource.Play ();
			Destroy (this.gameObject);
		} else if (other.tag == "Player") {
			// Damage the player
			Player player = other.GetComponent<Player>();
			if (player != null) {
				// Debug.Log ("Damage done by the enemy");
				player.Damage ();
			}
			Instantiate (_enemyExplosionPrefab, transform.position, Quaternion.identity);
			// _audioSource.Play ();
			AudioSource.PlayClipAtPoint(_clip, Camera.main.transform.position, 1f);
			Destroy (this.gameObject);
		}
	}
}
