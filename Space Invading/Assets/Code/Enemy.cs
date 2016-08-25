using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	//code for the enemies

	public float downSpeed;					//downward movement speed
	public float sideSpeed;					//sideward speed
	public float decisionTimer;				//average time between actions in seconds
	public float initialDirection;			//helper for changing movement direction when bumping
	public float enemyAggressivity;			//index of how many missiles are casted vs energy
	public float energyMissile;				//holder of the decision between missile or energy
	public bool loopHold;

	private float randomX;

	//Holders for GameObjects and RigidBody2D
	public GameObject energy;
	public GameObject missile;
	private Rigidbody2D rb2d;
	private float downSpeedMod;

	void Start () {

		decisionTimer = 2.5f;

		//increase aggressivity based on wave count
		if (Manager.instance.wave < 11) {
			enemyAggressivity = Manager.instance.wave / 15f;
			downSpeedMod= Repo.instance.downSpeedMod * Manager.instance.wave;
			decisionTimer -= Manager.instance.wave / 10f;
		} else {
			downSpeedMod = Repo.instance.downSpeedMod * 10;
			enemyAggressivity = 0.66f;
			decisionTimer -= 1f;
		}

		switch (EnemiesPool.instance.partsIdx [1]) {
		case 0:
			sideSpeed = Repo.instance.enemySideSpeed;
			downSpeed = Repo.instance.enemyDownSpeed + downSpeedMod;
			break;
		case 1:
			sideSpeed = 0;
			downSpeed = 0;
			StartCoroutine (RandomDirection ());
			break;
		case 2:
			sideSpeed = Repo.instance.enemySideSpeed / 2f;
			downSpeed = Repo.instance.enemyDownSpeed * 3f + downSpeedMod;
			break;
		}


		loopHold = true;

		if (Random.value > 0.5f)
			initialDirection = 1f;
		else
			initialDirection = -1f;
		
		//set working values
		rb2d = GetComponent<Rigidbody2D>();
			
		rb2d.velocity = new Vector2(initialDirection*sideSpeed, -downSpeed);

		//fixing the variables after ramming
		GetComponent<BoxCollider2D> ().isTrigger = false;
		transform.up = new Vector2 (0, 1);

		//call for the first action
		StartCoroutine (Action ());
	}

	void OnCollisionEnter2D(Collision2D collision) {
		//collision handler
		initialDirection *= -1;
		switch(collision.gameObject.tag) {
		case "wall": //change directions
			rb2d.velocity = new Vector2 (initialDirection*sideSpeed, -downSpeed);
			break;
		case "enemy": //change directions
			rb2d.velocity = new Vector2 (initialDirection*sideSpeed, -downSpeed);
			break;
		case "wallEnd": //dies when going too low
			DieKeep();
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D collision) {
		switch (collision.gameObject.tag) {
		case "wallEnd":
			DieKeep ();
			break;
		case "energyPlayer":
			DieKeep ();
			Destroy (collision.gameObject);
			Manager.instance.points += 50;
			Manager.instance.PlaySound (4);
			break;
		}
	}

	public void DieKeep () {
		ExplodesHolder.instance.PoolRunner (transform.position,1);
		transform.position = new Vector2 (0, 20f);
		gameObject.SetActive (false);
	}

	IEnumerator Action() {
		float timer = Random.Range (decisionTimer / 2, decisionTimer * 2);
		yield return new WaitForSeconds(timer); //run a timer with time between half and twice the decisionTimer variable
		switch (EnemiesPool.instance.partsIdx [2]) {
		case 0:
			ShootBase ();
			break;
		case 1:
			/*if (Player.instance.bulletCount == 0) {
				StartCoroutine (RamHim (1));
				Manager.instance.FinishHeatSeek ();
				loopHold = false;
			}*/
			if (GameObject.FindGameObjectsWithTag ("missile").Length> 2)
				HeatSeek ();
			else
				ShootBase ();
			break;
		case 2:
			if (transform.position.y < 1) {
				if (Mathf.Abs(transform.position.x - Player.instance.transform.position.x) < 1f) {
					StartCoroutine(RamHim (2));
					loopHold = false;
				} else
					ShootBase ();
			} else
				ShootBase ();
			break;
		}
		if (loopHold) StartCoroutine (Action ());   //Loop back
	}

	void ShootBase () {
		energyMissile = Random.value;
		if (Player.instance.bulletCount == 0) {
			if (EnemiesPool.instance.partsIdx [2] == 1) energyMissile *= 3;
			else energyMissile *= 2; //if the player has no bullets, increase the chance of shooting energy
		}
		if (energyMissile < enemyAggressivity) {
			Instantiate (missile, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity);
			Manager.instance.PlaySound (3);
		} else {
			Instantiate (energy, new Vector3 (transform.position.x, transform.position.y, 0), Quaternion.identity);
			Manager.instance.PlaySound (2);
		}
	}

	void HeatSeek () {
		GameObject[] bullets;

		bullets = GameObject.FindGameObjectsWithTag ("missile");

		for (int i = 0; i < bullets.Length; i++)
			bullets [i].SendMessage ("SeekPlayer");

	}

	IEnumerator RamHim (int direction) {
		GetComponent<BoxCollider2D> ().isTrigger = true;
		rb2d.velocity = new Vector2 (0, 0.5f);
		yield return new WaitForSeconds (0.5f);
		switch (direction) {
		case 1:
			rb2d.velocity = new Vector2 (0, -2f);
			break;
		case 2:
			if (Player.instance.gameObject.activeInHierarchy && transform.position.y > -1.5f) {
				downSpeed = Repo.instance.enemyDownSpeed * 8f;
				rb2d.velocity = -downSpeed * (transform.position - Player.instance.transform.position).normalized;
			}
			else
				rb2d.velocity = new Vector2 (0, -2f);
			transform.up = -rb2d.velocity;
			break;
		}
	}

	IEnumerator RandomDirection () {
		yield return new WaitForSeconds (1f);
		if (loopHold) {
			randomX = Random.Range (-1.5f, 1.5f) + transform.position.x;
			downSpeed = Repo.instance.enemyDownSpeed + downSpeedMod;
			rb2d.velocity = (transform.position - new Vector3 (randomX, transform.position.y - 2f, 0)).normalized * -downSpeed * 5f;
			downSpeed = -rb2d.velocity.y;
			sideSpeed = Mathf.Abs(rb2d.velocity.x);
			initialDirection = Mathf.Sign (rb2d.velocity.x);
			yield return new WaitForSeconds (0.6f);
			downSpeed = 0;
			sideSpeed = 0;
			if (loopHold) rb2d.velocity = Vector2.zero;
		}
		if(loopHold)
			StartCoroutine (RandomDirection ());
	}

}
