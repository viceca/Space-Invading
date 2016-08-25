using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

	public int lives; 									//how many lives the player has
	public GameObject playerObj;						//define the player prefab
	public GameObject enemyObj;							//define the enemy prefab
	public float respawnTime;							//how long it takes for the player to respawn
	public bool respawnCall;							//flag to prevent the respawn function being called multiple times
	public int points;									//how many points the player has
	public bool isPlaying;								//flag if the game is on, used to confirm exit game selections
	public float wave;									//how many enemy waves the player has defeated
	public int lifeGain;								//track for the life gain at a "pointsLife" points threshold
	public int pointsLife;								//how many points the player need to receive a life
	public float screenWidth;							//how wide is the screen, used to place the enemies evenly across the screen
	public int tempHighscore;							//helper for handling the highscore
	public bool pausedFlag;								//flag for pausing the game
	public bool gameBegan;								//flag for begining the game
	public bool tutorialFlag;							//flag for being on tutorial
	public GameObject tutorialManager;					//handler to instantiate the tutorial manager
	public bool motherShipActive;
	public GameObject motherShip;
	public int motherShipWaves;


	/*
	 * Sound Library indexes
	 * 0 = 1 Up
	 * 1 = Game Over
	 * 2 = Energy
	 * 3 = Missile
	 * 4 = Enemy Kill
	 * 5 = Drain
	 * 6 = Player Kill
	*/
	public AudioClip[] soundLib;

	public static Manager instance = null;

	private Text pointsText;
	private Text gameOverText;
	public Text beginText;
	private Image highScoreHighLight;
	private string highLoop;
	private int i;
	private int j;
	private bool newHighscoreFlag;
	private string highscoreText;
	private Text pauseText;
	private GameObject menuButtonAggregate;
	private GameObject gameOverButtons;
	private AudioSource source;
	private Image tutorialImage;
	private Image pauseFadeImage;
	private GameObject muteSoundsButtom;
	private GameObject muteMusicButtom;
	private bool mutedSounds;
	private GameObject sureAggregate;
	private string [] highscoreTextArray;
	private int tempLength;
	private GameObject promoMaterial;

	void Awake() {
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy (gameObject);
	}

	public void LearnToPlay () {
		tutorialImage.enabled = !tutorialFlag;
		menuButtonAggregate.SetActive (tutorialFlag);
		tutorialFlag = !tutorialFlag;
	}

	public void PauseGame () {
		if (pausedFlag) {
			Time.timeScale = 1f;
			menuButtonAggregate.SetActive (false);
		} else {
			Time.timeScale = 0f;
			menuButtonAggregate.SetActive (true);
		}
		pausedFlag = !pausedFlag;
		pauseFadeImage.enabled = pausedFlag;
	}

	public void QuitGame () {
        ApplicationModel.selection = 2;
		if (isPlaying) {
            menuButtonAggregate.SetActive (false);
            sureAggregate.SetActive (true);
        } else {
            ConfirmSelection ();
        }
	}

	public void GoMainMenu () {
        ApplicationModel.selection = 1;
		if (isPlaying) {
            menuButtonAggregate.SetActive (false);
            sureAggregate.SetActive (true);
        } else {
            ConfirmSelection ();
        }
	}

	public void ConfirmSelection () {
		switch (ApplicationModel.selection) {
		case 1:
			Time.timeScale = 1f;
			ApplicationModel.gameType = 10;
			SceneManager.LoadScene ("MainMenu");
			break;
		case 2:
			Application.Quit ();
			break;
		}
	}

	public void NegateSelection () {
		menuButtonAggregate.SetActive (true);
		ApplicationModel.selection = 0;
		sureAggregate.SetActive (false);
	}

	public void MuteMusic () {
		if (source.isPlaying) {
			source.Stop ();
			muteMusicButtom.GetComponent<Image> ().color = Color.gray;
			SaveLoad.thisOne.gameSettings[1] = 1;
		} else {
			source.Play ();
			muteMusicButtom.GetComponent<Image> ().color = Color.white;
			SaveLoad.thisOne.gameSettings[1] = 0;
		}
		SaveLoad.Save ();
	}

	public void MuteSounds () {
		if (mutedSounds) {
			muteSoundsButtom.GetComponent<Image> ().color = Color.white;
			SaveLoad.thisOne.gameSettings[2] = 0;
		} else {
			muteSoundsButtom.GetComponent<Image> ().color = Color.gray;
			SaveLoad.thisOne.gameSettings[2] = 1;
		}
		mutedSounds = !mutedSounds;
		SaveLoad.Save ();

	}



	void BeginGame () {
		gameBegan = true;
		beginText.enabled = false;
		SpawningTimer ();
	}

	public void SpawnEnemies (int enemyCount, int downDist){
		int i;

		if (ApplicationModel.gameType == 2 && wave % motherShipWaves == 1 && wave > 0)
			EnemiesPool.instance.RePicker();

		for (i = 0; i < enemyCount/2; i++) {

			enemyObj = EnemiesPool.instance.PoolRunner ();
			enemyObj.transform.position = new Vector3 ((((float)i + 1 + ((float)i / 10)) / ((float)enemyCount)) * screenWidth, 3 + downDist, 0);
			enemyObj.SendMessage ("Start");

			enemyObj = EnemiesPool.instance.PoolRunner ();
			enemyObj.transform.position = new Vector3(-(((float)i+1+((float)i/10))/((float)enemyCount))*screenWidth, 3 + downDist, 0);
			enemyObj.SendMessage ("Start");

		}
		if (enemyCount % 2 == 1) {
			enemyObj = EnemiesPool.instance.PoolRunner ();
			enemyObj.transform.position = new Vector3(0,3 + downDist,0);
			enemyObj.SendMessage ("Start");
		}
		wave++;
	}

	public void SpawningTimer (){
		//yield return new WaitForSeconds(respawnTime);
		DummyInt.instance.gameObject.SetActive (true);
		DummyInt.instance.SendMessage ("Start");
	}

	public void Respawn () {
		Player.instance.transform.position = new Vector2 (0f, -3f);
		Player.instance.gameObject.SetActive (true);
		Player.instance.SendMessage ("Start");
		respawnCall = false;
	}

	void DisplayPromo () {
		promoMaterial.SetActive (true);
	}

	public void SendoFodao () {
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.Games_Oyo.Space_Invading");
	}

	public void ContinandoPaoDurisse () {
		promoMaterial.SetActive (false);
		EndGame ();
	}

	void EndGame () {
		for (i = 0; i < 3; i++) {
			if (points > SaveLoad.thisOne.highscore [i]) {
				tempHighscore = SaveLoad.thisOne.highscore [i];
				SaveLoad.thisOne.highscore [i] = points;
				for (j = i + 1; j < 3; j++) {
					points = SaveLoad.thisOne.highscore [j];
					SaveLoad.thisOne.highscore [j] = tempHighscore;
					tempHighscore = points;
				}
				newHighscoreFlag = true;
				highScoreHighLight = GameObject.Find ("HighScoreImage" + i).GetComponent<Image> ();
				highScoreHighLight.enabled = true;
				break;
			}
		}

		SaveLoad.Save ();

		for (i = 0; i < SaveLoad.thisOne.highscore.Length; i++) {
			highscoreTextArray[i] =  SaveLoad.thisOne.highscore[i].ToString();
			tempLength = highscoreTextArray [i].Length;
			for (int j = 0; j < 6 - tempLength; j++)
				highscoreTextArray [i] = "0" + highscoreTextArray [i];
			highscoreTextArray [i] = i + ". " + highscoreTextArray [i];
		}

		highscoreText = "HIGH SCORES\n" + highscoreTextArray [0] + "\n" + highscoreTextArray [1] + "\n" + highscoreTextArray [2];

		if (newHighscoreFlag) {
			highscoreText = "NEW " + highscoreText;
			PlaySound (0);
		}
		else PlaySound (1);

		gameOverText.text = highscoreText;
		gameOverText.enabled = true;
		gameOverButtons.SetActive (true);
	}
		
	public void PlaySound (int idx) {
		if(!mutedSounds)
			source.PlayOneShot (soundLib [idx], 1f);
	}

	public void ResetGame () {
		switch (ApplicationModel.gameType) {
		case 1:
			ApplicationModel.planetsInvaded = 0;
			ApplicationModel.gameType = 0;
			SceneManager.LoadScene ("PlanetSel");
			break;
		case 2:
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
			break;
		}
	}
		
	IEnumerator WaitToStart (){
		Instantiate (playerObj, new Vector3 (20f, 0f, 0f), Quaternion.identity);
		Player.instance.gameObject.SetActive(false);
		yield return new WaitForSeconds(1);
		BeginGame ();
	}

	public void FinishHeatSeek () {
		GameObject[] bullets;
		bullets = GameObject.FindGameObjectsWithTag ("energyEnemy");

		for (int i = 0; i < bullets.Length; i++)
			bullets [i].SendMessage ("DirectionShoot", 2);

		bullets = GameObject.FindGameObjectsWithTag ("missile");

		for (int i = 0; i < bullets.Length; i++)
			bullets [i].SendMessage ("DirectionShoot", 2);
	}

	public void MapSelAgain () {
		SceneManager.LoadScene ("PlanetSel");
	}
		
	public void ColorUpdateCaller () {
		StartCoroutine (ColorUpdate ());
	}

	public IEnumerator ColorUpdate () {
		GameObject.Find("WhiteBackground").GetComponent<Image>().color = Color.gray;
		GameObject.Find ("BulletBlackOverlay").GetComponent<Image> ().color = Color.gray;
		yield return new WaitForSeconds (0.2f);
		GameObject.Find("WhiteBackground").GetComponent<Image>().color = Color.white;
		GameObject.Find ("BulletBlackOverlay").GetComponent<Image> ().color = Color.white;
	}

	// Use this for initialization
	void Start () {

		if (ApplicationModel.planetsInvaded == 0)
			points = 0;
		else
			points = ApplicationModel.tempPoints;

		respawnTime = 1f;
		respawnCall = true;
		isPlaying = true;
		wave = 0f;
		lifeGain = 0;
		pointsLife = 500;
		tempHighscore = 0;
		pausedFlag = false;
		tutorialFlag = false;
		lives = Repo.instance.startinLives;
		gameBegan = false;
		mutedSounds = false;
		motherShipActive = false;
		motherShipWaves = 5;
		if (ApplicationModel.paoDuro)
			motherShipWaves = 1000;

		if (ApplicationModel.gameType == 1)
			motherShipWaves += ApplicationModel.planetsInvaded;

		highscoreTextArray = new string[3];
		newHighscoreFlag = false;

		pointsText = GameObject.Find("PointsText").GetComponent<Text>();
		gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
		beginText = GameObject.Find("BeginText").GetComponent<Text>();
		pauseText = GameObject.Find ("PauseText").GetComponent<Text> ();
		menuButtonAggregate = GameObject.Find ("MenuButtons");
		gameOverButtons = GameObject.Find ("GameOverButtons");
		source = GetComponent<AudioSource>();
		tutorialImage = GameObject.Find ("TutorialImage").GetComponent<Image> ();
		muteMusicButtom = GameObject.Find ("MusicButtom");
		muteSoundsButtom = GameObject.Find ("SoundsButtom");
		pauseFadeImage = GameObject.Find ("PauseFade").GetComponent<Image> ();
		sureAggregate = GameObject.Find ("SelectionButtons");
		promoMaterial = GameObject.Find ("Promo");

		tutorialImage.enabled = false;
		gameOverText.enabled = false;
		menuButtonAggregate.SetActive(false);
		promoMaterial.SetActive (false);
		gameOverButtons.SetActive (false);
		pauseFadeImage.enabled = false;
		screenWidth = (GameObject.Find ("wallRight").transform.position.x - 0.5f);
		sureAggregate.SetActive (false);

		if (ApplicationModel.gameType != 3) {
			GameObject.Find ("TutorialStuff").SetActive (false);
			ApplicationModel.okToShoot = true;
		} else {
			Instantiate (tutorialManager);
			GameObject.Find ("BeginText").GetComponent<Text>().enabled = false;
		}

		SaveLoad.Load ();

		if (SaveLoad.thisOne.gameSettings[1] == 1)
			MuteMusic ();

		if (SaveLoad.thisOne.gameSettings[2] == 1)
			MuteSounds ();
			

		SaveLoad.Save ();

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		pauseText.text = "'P' to Pause";
		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		pauseText.text = "PAUSE";
		#endif
		if (ApplicationModel.gameType != 3)
			StartCoroutine (WaitToStart ());
	}
	
	// Update is called once per frame
	void Update () {
		if (gameBegan) {
			if (!Player.instance.isActiveAndEnabled && !respawnCall && isPlaying) {
				if (lives > 0) {
					lives--;
					LiveManager.instance.UpdateLife (lives);
					SpawningTimer ();
					respawnCall = true;
				} else {
					isPlaying = false;
					pauseFadeImage.enabled = true;
					if (ApplicationModel.paoDuro)
						DisplayPromo ();
					else
						EndGame ();
				}

			}

			if (GameObject.FindGameObjectWithTag ("enemy") == null && isPlaying && !motherShipActive) {
				if (EnemiesPool.instance.partsIdx [2] == 1)
					FinishHeatSeek ();
				if (wave > 0 && wave%motherShipWaves == 0) {
					if (GameObject.FindGameObjectWithTag ("missile") == null && GameObject.FindGameObjectWithTag ("energyPlayer") == null && GameObject.FindGameObjectWithTag ("energyEnemy") == null) {
						Player.instance.okToShoot = false;
						Instantiate (motherShip, new Vector3 (0, 4.5f, 0), Quaternion.identity);
						motherShipActive = true;
					}
				}
				else if (GameObject.FindGameObjectWithTag("missile") == null && GameObject.FindGameObjectWithTag("energyPlayer") == null && GameObject.FindGameObjectWithTag("energyEnemy") == null)
					SpawnEnemies (Random.Range (3, 5), 0);
			}

			pointsText.text = "SCORE:" + points;

			if (!isPlaying) {
				#if UNITY_STANDALONE || UNITY_WEBPLAYER
				if(Input.GetKeyDown (KeyCode.S))
					Application.LoadLevel (Application.loadedLevel);
				#endif
			}

			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (sureAggregate.activeInHierarchy) {
					menuButtonAggregate.SetActive (true);
					sureAggregate.SetActive (false);
				}
					else PauseGame ();
				if (tutorialImage.enabled)
					tutorialImage.enabled = false;
			}

			if (points / pointsLife > lifeGain) {
				lifeGain++;
				if (lives < LiveManager.instance.maxLives) {
					lives++;
					PlaySound (0);
					LiveManager.instance.UpdateLife (lives);
				}
			}
			if (tutorialFlag) {
				#if UNITY_STANDALONE || UNITY_WEBPLAYER
				if(Input.GetKeyDown (KeyCode.S))
					LearnToPlay ();

				#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began)
					LearnToPlay ();
				#endif
			}
		}
	}
}