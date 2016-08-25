using UnityEngine;
using System.Collections;

public class MotherShip : MonoBehaviour {

	public int lifePoint;
	public float baseSpeed;
	public int initialDirection;
	public GameObject missile;
	public GameObject energy;
	public bool started;
	public bool dead;
	public float decisionTime;
	public float missileSpawnRate;
	public GameObject planetDummyObj;

	private float energyMissile;

	public int[] partsIdx = new int[2];
	/*
	 * 0 = core
	 * 1 = rim
	*/

	private int explodes;
	private int i;
	private SpriteRenderer[] sr;
	private Rigidbody2D rb2d;

	void Awake () {

		partsIdx [0] = EnemiesPool.instance.partsIdx [2];
		partsIdx [1] = EnemiesPool.instance.partsIdx [0];

		rb2d = GetComponent<Rigidbody2D> ();
		sr = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer child in sr) {
			switch (child.gameObject.name) {
				case "Core":
					child.sprite = EnemiesPool.instance.partsMotherCore [partsIdx[0]];
					break;
				case "Rim":
					child.sprite = EnemiesPool.instance.partsMotherRim [partsIdx [1]];
					break;
			}
		}
	}

	// Use this for initialization
	void Start () {
		
		dead = false;
		rb2d.velocity = new Vector2 (0, -1f);
		started = false;
		explodes = 6;

		switch (partsIdx [0]) {
		case 0:
			decisionTime = 1.8f;
			missileSpawnRate = 0.5f;
			break;
		case 1:
			decisionTime = 1.4f;
			missileSpawnRate = 0.8f;
			break;
		case 2:
			decisionTime = 2.4f;
			missileSpawnRate = 0.3f;
			break;
		}

		switch (partsIdx[1]) {
		case 0:
			lifePoint = 7;
			baseSpeed = 0.7f;
			break;
		case 1:
			lifePoint = 5;
			baseSpeed = 1.2f;
			break;
		case 2:
			lifePoint = 10;
			baseSpeed = 0.5f;
			break;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (!started && transform.position.y < 3f) {
			started = true;
			Player.instance.okToShoot = true;
			DefineBaseSpeed ();
		}
		if (lifePoint <= 0 && !dead) {
			rb2d.velocity = Vector3.zero;
			dead = true;
			KillEnemies(GameObject.FindGameObjectsWithTag("enemy"));
			KillEnemies(GameObject.FindGameObjectsWithTag("energyEnemy"));
			KillEnemies(GameObject.FindGameObjectsWithTag("missile"));
			StartCoroutine (DeathAnim ());

		}
	}

	IEnumerator DeathAnim () {
		ApplicationModel.okToShoot = false;
		ExplodesHolder.instance.PoolRunner (transform.position + new Vector3 (Random.Range (-0.5f, 0.5f), Random.Range (-0.5f, 0.5f), 0),1);
		Manager.instance.PlaySound (4);
		explodes--;
		if (explodes == 0) {
			ExplodesHolder.instance.PoolRunner (transform.position, 4);
			Manager.instance.PlaySound (4);
			MotherDie ();
		}
		else {
			yield return new WaitForSeconds (1f);
			StartCoroutine (DeathAnim ());
		}
	}

	void KillEnemies (GameObject[] enemies) {
		foreach (GameObject obj in enemies) {
			if (obj.tag == "enemy" && obj.name != "MotherShip(Clone)")
				obj.SendMessage ("DieKeep");
			else if (obj.tag != "enemy")
				Destroy (obj);
		}
	}

	void DefineBaseSpeed () {
		if (Random.value > 0.5f)
			initialDirection = 1;
		else
			initialDirection = -1;

		rb2d.velocity = new Vector2 (initialDirection * baseSpeed, 0);
		StartCoroutine (DecisionTimer ());
	}

	void MotherDie () {

		switch (ApplicationModel.gameType) {
		case 1:
			foreach (SpriteRenderer child in sr) {
				child.enabled = false;
			}
	
			Instantiate (planetDummyObj, new Vector3 (0, 6, 0), Quaternion.identity);

			UpdateSaveFile ();
			ApplicationModel.planetsInvaded++;
			ApplicationModel.tempPoints = Manager.instance.points;
			break;
		case 2:
			Manager.instance.wave++;
			Manager.instance.motherShipActive = false;
			Manager.instance.points += 250;
			ApplicationModel.okToShoot = true;
			break;
		}

		Destroy (gameObject);
	}

	void UpdateSaveFile () {
		for (i = SaveLoad.thisOne.names.Length-1; i > 0; i--) {
			SaveLoad.thisOne.names [i] = SaveLoad.thisOne.names [i - 1];
			SaveLoad.thisOne.bases [i] = SaveLoad.thisOne.bases [i - 1];
			SaveLoad.thisOne.lands [i] = SaveLoad.thisOne.lands [i - 1];
			SaveLoad.thisOne.atmospheres [i] = SaveLoad.thisOne.atmospheres [i - 1];
		}
		SaveLoad.thisOne.names [0] = ApplicationModel.tempPlanet [0];
		SaveLoad.thisOne.bases [0] = ApplicationModel.tempPlanet [1];
		SaveLoad.thisOne.lands [0] = ApplicationModel.tempPlanet [2];
		SaveLoad.thisOne.atmospheres [0] = ApplicationModel.tempPlanet [3];
		SaveLoad.thisOne.planetsInvaded++;
		SaveLoad.Save ();
	}

	IEnumerator DecisionTimer () {
		yield return new WaitForSeconds (decisionTime * Random.Range (0.5f, 1f));
		if (lifePoint > 0) {
			Action ();
			StartCoroutine (DecisionTimer ());
		}
	}

	void Action () {
		if (Random.value < missileSpawnRate) {
			switch (partsIdx [0]) {
			case 0:
				ShootBase ();
				break;
			case 1:
				if (GameObject.FindGameObjectsWithTag ("missile").Length > 2)
					HeatSeek ();
				else 
					ShootBase ();
				break;
			case 2:
				ShootBase ();
				break;
			}
		}
		else if(GameObject.FindGameObjectsWithTag ("enemy").Length <=2 )
			Manager.instance.SpawnEnemies(4 - GameObject.FindGameObjectsWithTag ("enemy").Length, -1);
	}

	void ShootBase () {
		energyMissile = Random.value;
		if (Player.instance.bulletCount == 0) {
			if (EnemiesPool.instance.partsIdx [2] == 1) energyMissile *= 3;
			else energyMissile *= 2; //if the player has no bullets, increase the chance of shooting energy
		}
		if (energyMissile < 0.6f) {
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

	void OnCollisionEnter2D(Collision2D collision) {
		switch (collision.gameObject.tag) {
		case "wall":
			initialDirection = -initialDirection;
			rb2d.velocity = new Vector2 (initialDirection * baseSpeed, 0);
			break;
		}
	}

	void OnTriggerEnter2D (Collider2D collision) {
		switch (collision.gameObject.tag) {
		case "energyPlayer":
			Manager.instance.PlaySound (4);
			lifePoint--;
			ExplodesHolder.instance.PoolRunner (collision.gameObject.transform.position, 1);
			Destroy (collision.gameObject);
			break;
		}
	}

	public void DieKeep () {
		lifePoint--;
	}
}
