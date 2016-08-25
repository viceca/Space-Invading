using UnityEngine;
using System.Collections;

public class DummyInt : MonoBehaviour {
	//code for the dummy character that appears on the bottom of the screen, as a player entry animation
	public float speed;				//speed of movement
	private Rigidbody2D rb2d;		//holder for the RigidBody2D

	public static DummyInt instance = null;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		speed = 1.5f;
		if (ApplicationModel.gameType == 0)
			speed = 2f;
		rb2d = GetComponent<Rigidbody2D> ();
	}

	// Use this for initialization
	void Start () {
		//set velocity
		rb2d.velocity = new Vector2 (0f, speed);
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y + 3f > 0f) {					//when the dummy got into final position, store it away and call for the player spawn
			transform.position = new Vector2 (0f, -5f);
			rb2d.velocity = Vector2.zero;
			if (ApplicationModel.gameType == 0)
				MapManager.instance.SpawnDummy ();
			else 
				Manager.instance.Respawn ();
		}

	}
}
