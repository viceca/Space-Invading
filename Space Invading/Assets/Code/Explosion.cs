using UnityEngine;
using System.Collections;

public class Explosion : MonoBehaviour {

	public void Deactivate () {
		transform.localScale = new Vector3 (1, 1, 1);
		transform.position = new Vector3 (0, 20, 0);
		gameObject.SetActive (false);
	}
}
