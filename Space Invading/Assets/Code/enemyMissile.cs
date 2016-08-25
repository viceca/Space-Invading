using UnityEngine;
using System.Collections;

public class enemyMissile : MonoBehaviour {

	public float downSpeed;
	public bool seeking;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		//set variables values
		downSpeed = 2f;
		seeking = false;


		//set working values
		rb2d = GetComponent<Rigidbody2D>();
		if (ApplicationModel.gameType == 3)
			DirectionShoot (0);
		else 
			DirectionShoot (EnemiesPool.instance.partsIdx[2]);
	}

	void DirectionShoot (int type) {
		switch (type) {
		case 0:
			rb2d.velocity = new Vector2 (0f, -downSpeed);
			break;
		case 1:
			rb2d.velocity = new Vector2 (0f, -0.5f);
			break;
		case 2:
			rb2d.velocity = new Vector2 (0f, -downSpeed);
			break;
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		switch(collision.gameObject.tag) {
		case "wallEnd":
			Destroy (gameObject);
			break;
		case "energyPlayer":
			Destroy (collision.gameObject);
			Destroy (gameObject);
			break;
		}
	}

	void SeekPlayer () {
		if (!seeking && transform.position.y > -2f) {
			if (Player.instance.gameObject.activeInHierarchy)
				rb2d.velocity = -(downSpeed / 2) * (transform.position - Player.instance.transform.position);
			else
				rb2d.velocity = new Vector2 (0, -downSpeed/2);
			seeking = true;
			transform.up = -rb2d.velocity;
		}
	}

}
