using UnityEngine;
using System.Collections;

public class enemyEnergy : MonoBehaviour {
	// code for the enemies energy shots
	public float downSpeed; 		//move speed
	public bool shrinking;			//flag to start shriking sprite when eaten by player
	public float targetScale;		//final shrink size
	public float shrinkSpeed;		//shrinking speed
	public float targetX;			//holder for the player position
	public float drainSpeed;		//speed with each the energy move towards the drainig player
	public float drainStep;			//helper to calculate movement
	public bool seeking;

	private Rigidbody2D rb2d;

	void Start () {
		//set variables values
		downSpeed = 2f;
		shrinking = false;
		targetScale = 0.1f;
		shrinkSpeed = 1.5f;
		drainSpeed = 3f;
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
			rb2d.velocity = new Vector2 (0f, -downSpeed);
			break;
		case 2:
			rb2d.velocity = new Vector2 (0f, -downSpeed);
			break;
		}
	}

	void DrainShrink () {			//called by the player, starts the shrinking and TURN OFF THE BOX COLLIDER
		shrinking = true;
		GetComponent<BoxCollider2D>().enabled = false;
		rb2d.velocity = Vector2.zero;
	}

	// Update is called once per frame
	void Update () {
		if (shrinking) {			//move the energy towards the player and shrink the sprite
			targetX = Player.instance.transform.position.x;
			drainStep = drainSpeed * Time.deltaTime;
			transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX,-3f), drainStep);
			drainStep = shrinkSpeed * Time.deltaTime;
			transform.localScale = Vector2.MoveTowards(transform.localScale, new Vector2(targetScale,targetScale), drainStep);
			if (transform.localScale.sqrMagnitude < targetScale || !Player.instance.isActiveAndEnabled)
				Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collision) {
		switch(collision.gameObject.tag) {
		case "wallEnd":
			Destroy (gameObject);
			break;
		}
	}

	void SeekPlayer () {
		if (!seeking && transform.position.y >-2f) {
			if (Player.instance.gameObject.activeInHierarchy)
				rb2d.velocity = -(downSpeed / 2) * (transform.position - Player.instance.transform.position);
			else
				rb2d.velocity = new Vector2 (0, -downSpeed/2);
			seeking = true;
		}
	}
}
