using UnityEngine;
using System.Collections;

public class BG : MonoBehaviour {

	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (new Vector3 (0, -0.5f, 0) * Time.deltaTime);
		if (transform.position.y < -20) {
			transform.position = new Vector3 (0, 10, 0);
		}
	}
}
