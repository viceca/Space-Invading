using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

	public GameObject canvasStuff;
	public GameObject playerObj;
	public Image missileDummy;
	public Image energyDummy;
	public GameObject[] bullets;
	public int shootTimes;
	public int tutorialStep;
	public float tempX;
	public bool stepFlag;
	public int tutorialQuality;

	/*Tutorial Steps
	 * 0 - intro
	 * 1 - moving
	 * 2 - missile text
	 * 3 - dodging missile
	 * 4 - eat energy text
	 * 5 - eating energy
	 * 6 - shoot text
	 * 7 - shooting
	 * 8 - farewell
	 * 9 - exit tutorial
	 */

	private Text tutorialText;
	private int tempQuality;
	private bool timeHold;
	private int i;
	private int targetBullets;

	void FixTextEnding (string finish) {
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		tutorialText.text = tutorialText.text + "press S to ";
		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		tutorialText.text = tutorialText.text + "tap to ";
		#endif
		tutorialText.text = tutorialText.text + finish;
	}

	void UpdateText () {
		switch (tutorialStep) {
		case 0:
			tutorialText.text = 
				"Hello young invader!\n" +
			"I see you are ready to move away from home,\n" +
			"but before that you need to learn our ways.\n" +
			"\n";
			FixTextEnding ("continue.");
			StartCoroutine (WaitTime ());
			break;
		case 1:
			tutorialText.text = 
				"First you need to know how to move.\n" +
				"Tilt your body to move sideways.";
			break;
		case 2:
			tutorialText.text = 
				"Great!\n" +
			"Now the most important part: dangers.\n" +
			"See these red thing? It's a deadly weapon\n" +
			"used by our enemies, try to avoid these, OK?\n" +
			"\n";
			FixTextEnding ("test your skills.");
			StartCoroutine (WaitTime ());
			missileDummy.enabled = true;
			break;
		case 3:
			StartCoroutine (BulletSpawner ());
			missileDummy.enabled = false;
			tutorialText.enabled = false;
			tempQuality = tutorialQuality;
			break;
		case 4:
			tutorialText.enabled = true;
			if (tempQuality == tutorialQuality)
				tutorialText.text = "Good Job!\n";
			else
				tutorialText.text = "Humm, that was... something.\n";
			tutorialText.text = tutorialText.text +
			"Our dumb enemies also try to shoot energy.\n" +
			"But we can eat those!\n" +
			"Just stand still, open your mouth and try!\n" +
			"\n";
			FixTextEnding ("test your skills.");
			StartCoroutine (WaitTime ());
			energyDummy.enabled = true;
			break;
		case 5:
			StartCoroutine (BulletSpawner ());
			energyDummy.enabled = false;
			tutorialText.enabled = false;
			tempQuality = tutorialQuality;
			ApplicationModel.okToShoot = false;
			Player.instance.okToShoot = false;
			break;
		case 6:
			tutorialText.enabled = true;
			if (tempQuality == tutorialQuality)
				tutorialText.text = "You are a natural!\n";
			else
				tutorialText.text = "Remember to receive food with an open mouth.\n";
			tutorialText.text = tutorialText.text +
			"All the energy we eat, not only feed us,\n" +
			"but we can shoot back at our enemies!\n" +
			"Tap into yourself to shoot.\n" +
			"\n";
			FixTextEnding ("test your skills.");
			StartCoroutine (WaitTime ());
			break;
		case 7:
			tutorialText.enabled = false;
			StartCoroutine (WaitTime ());
			break;
		case 8:
			tutorialText.enabled = true;
			if (tutorialQuality < -2)
				tutorialText.text = "You are... hummm... special.\n";
			else
				tutorialText.text = "You learn very fast!\n";
			tutorialText.text = tutorialText.text +
			"Now it is time for you to explore!\n" +
			"For you to face dangers!\n" +
			"For you to CONQUER THE UNIVERSE!!!\n" +
			"\n";
			FixTextEnding ("start CONQUERING!!!");
			StartCoroutine (WaitTime ());
			break;
		case 9:
			ApplicationModel.gameType = 0;
			ApplicationModel.planetsInvaded = 0;
			SceneManager.LoadScene ("PlanetSel");
			break;
		}
	}

	IEnumerator BulletSpawner() {
		yield return new WaitForSeconds (1f);
		if(i == 1)
			Instantiate (bullets[i], new Vector3 (Random.Range (-2, 2), 1f, 0), Quaternion.identity);
		else
			Instantiate (bullets[i], new Vector3 (Player.instance.transform.position.x + Random.Range (-0.2f, 0.2f), 1f, 0), Quaternion.identity);
		shootTimes--;
		if (shootTimes > 0 || Player.instance.bulletCount < targetBullets)
			StartCoroutine (BulletSpawner ());
		else {
			stepFlag = true;
			shootTimes = 3;
			i++;
			targetBullets = 2;
		}
	}

	IEnumerator WaitTime() {
		yield return new WaitForSeconds (1f);
		timeHold = true;
		if (tutorialStep == 7) {
			ApplicationModel.okToShoot = true;
			Player.instance.okToShoot = true;
			timeHold = false;
		}
	}

	// Use this for initialization
	void Start () {
		canvasStuff = GameObject.Find ("TutorialStuff");
		missileDummy = GameObject.Find ("MissileDummy").GetComponent<Image>();
		energyDummy = GameObject.Find ("EnergyDummy").GetComponent<Image>();
		Instantiate (playerObj, new Vector3 (20f, 0f, 0f), Quaternion.identity);
		Player.instance.gameObject.SetActive(false);
		DummyInt.instance.gameObject.SetActive (true);
		DummyInt.instance.SendMessage ("Start");
		tutorialText = canvasStuff.GetComponentInChildren<Text> ();
		tutorialStep = 0;
		shootTimes = 3;
		stepFlag = false;
		timeHold = false;
		i = 0;
		targetBullets = 0;
		tutorialQuality = 0;

		UpdateText ();
	}
	
	// Update is called once per frame
	void Update () {
		if (tutorialStep == 0 && (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.S)) && timeHold) {
			tutorialStep = 1;
			UpdateText ();
			tempX = Player.instance.transform.position.x;
			timeHold = false;
		}

		if (tutorialStep == 1 && Mathf.Abs (tempX - Player.instance.transform.position.x) > 2f && Player.instance.isActiveAndEnabled) {
			tutorialStep = 2;
			UpdateText ();
		}

		if (tutorialStep == 2 && (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.S)) && timeHold) {
			tutorialStep = 3;
			UpdateText ();
			timeHold = false;
		}

		if (tutorialStep == 4 && (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.S)) && timeHold) {
			tutorialStep = 5;
			UpdateText ();
			timeHold = false;
		}

		if (stepFlag && GameObject.Find("EnemyMissile(Clone)") == null && tutorialStep == 3) {
			tutorialStep = 4;
			UpdateText ();
			stepFlag = false;
		}

		if (stepFlag && GameObject.Find("enemyEnergy(Clone)") == null && tutorialStep == 5) {
			tutorialStep = 6;
			UpdateText ();
			stepFlag = false;
		}

		if (tutorialStep == 6 && (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.S)) && timeHold) {
			tutorialStep = 7;
			UpdateText ();
			timeHold = false;
		}

		if (Player.instance.bulletCount == 0 && tutorialStep == 7 && GameObject.Find("playerEnergy(Clone)") == null) {
			tutorialStep = 8;
			UpdateText ();
		}

		if (tutorialStep == 8 && (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began || Input.GetKeyDown(KeyCode.S)) && timeHold) {
			tutorialStep = 9;
			UpdateText ();
		}

		if (!Player.instance.isActiveAndEnabled && !Manager.instance.respawnCall && tutorialStep > 2) {
			DummyInt.instance.gameObject.SetActive (true);
			DummyInt.instance.SendMessage ("Start");
			Manager.instance.respawnCall = true;
			tutorialQuality--;
		}
	}
}
