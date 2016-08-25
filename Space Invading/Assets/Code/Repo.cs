using UnityEngine;
using System.Collections;

public class Repo : MonoBehaviour {

	public static Repo instance = null;


	public float drainSpeed;
	public float enemyDownSpeed;
	public int startinLives;
	public int startinBullets;
	public float downSpeedMod;
	public float enemySideSpeed;


	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		drainSpeed = 6f;

		switch (ApplicationModel.gameType) {
		case 1:
			enemySideSpeed = 1.5f;
			enemyDownSpeed = 0.3f;
			startinLives = 3;
			startinBullets = 3;
			downSpeedMod = 0.02f;
			break;
		case 2:
			enemySideSpeed = 1.5f;
			enemyDownSpeed = 0.3f;
			startinLives = 3;
			startinBullets = 3;
			downSpeedMod = 0.02f;
			break;
		case 3:
			enemySideSpeed=1.5f;
			enemyDownSpeed = 0.3f;
			startinLives = 3;
			startinBullets = 0;
			downSpeedMod = 0.02f;
			break;
		}
	}
}
