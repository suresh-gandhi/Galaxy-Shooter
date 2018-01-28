using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	[SerializeField]
	private float speed = 10.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// move up
		transform.Translate(Vector3.up * speed * Time.deltaTime);

		if (transform.position.y >= 6.0f) {
			if (transform.parent != null) {
				Destroy (transform.parent.gameObject);
			}
			Destroy (this.gameObject);
		}
	}
}
