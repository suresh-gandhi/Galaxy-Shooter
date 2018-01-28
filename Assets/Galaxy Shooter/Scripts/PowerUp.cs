using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {


	[SerializeField]
	private float _speed = 3.0f;

	[SerializeField]
	private int powerUpId;	// 0 = triple shot 1 = speed boost 2 = shields

	[SerializeField]
	private AudioClip _clip;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (Vector3.down * _speed * Time.deltaTime);	

		if (transform.position.y < -7) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		// Debug.Log ("Collider with : " + other.name);

		if (other.tag == "Player") {
			// access the player
			Player player = other.GetComponent<Player> ();

			AudioSource.PlayClipAtPoint (_clip, Camera.main.transform.position, 1f);
			// turn the triple shot bool to true

			if (player != null) {
				// enable triple shot here
				if (powerUpId == 0) {
					player.TripleShotPowerUpOn ();				// Now we have ensured that we have collided here and every player has a player script so we need to worry about the null pointer exception here.		
				} else if (powerUpId == 1) {
					// enable speed boost here	
					player.SpeedBoostPowerUpOn(); 
				} else if (powerUpId == 2) {
					// enable shields here
					player.EnableShields();
				}
			}

			// destroy ourselves
			Destroy (this.gameObject);	
		}

	}
}
