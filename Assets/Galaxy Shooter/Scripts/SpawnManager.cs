using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	[SerializeField]
	private GameObject enemyShipPrefab;

	[SerializeField]
	private GameObject[] powerUps;

	private GameManager _gameManager;

	// Use this for initialization
	void Start () {
		_gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
	}
	
	public void StartSpawnRoutines(){
		StartCoroutine (EnemySpawnRoutine());
		StartCoroutine (PowerUpSpawnRoutine());
	}

	// create a coroutine to spawn the enemy every five seconds.

	IEnumerator EnemySpawnRoutine(){
		while(_gameManager.gameOver == false){
			Instantiate (enemyShipPrefab, new Vector3(Random.Range(-7f, 7f), 7f, 0), Quaternion.identity);
			yield return new WaitForSeconds (5.0f);
		}
	}

	IEnumerator PowerUpSpawnRoutine(){
		while(_gameManager.gameOver == false){
			int randomPowerUp = Random.Range (0, 3);
			Instantiate (powerUps[randomPowerUp], new Vector3(Random.Range(-7f, 7f), 7f, 0), Quaternion.identity);
			yield return new WaitForSeconds (5.0f);	
		}
	}
}
