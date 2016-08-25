using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DummyMap : MonoBehaviour {
	//code for the dummy character that appears on the bottom of the screen, as a player entry animation
	public float speed;				//speed of movement
	public Vector3 finalDirection;
	public bool moving;

	private Rigidbody2D rb2d;		//holder for the RigidBody2D

	public static DummyMap instance = null;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		speed = 5f;
		rb2d = GetComponent<Rigidbody2D> ();
		moving = false;
	}

	void Update () {
		if (moving) {
			if ((transform.position - finalDirection).magnitude < 1f) {
				SceneManager.LoadScene ("GameBase");
				ApplicationModel.gameType = 1;
			}
		}
	}


	public void SendDummy (Vector3 direction) {
		rb2d.velocity = -speed * (transform.position - direction).normalized;
		GetComponent<Animator> ().SetBool ("moving", true);
		transform.up = rb2d.velocity;
		finalDirection = new Vector3(direction.x, direction.y,0);
		MapManager.instance.planetName.text = "";
		moving = true;
	}
		
}
