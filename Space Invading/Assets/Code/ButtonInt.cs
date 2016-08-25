using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonInt : MonoBehaviour {

	public int gameStateFlag; 					
	/*
	 * 0 = intro
	 * 1 = menu
	 * 2 = high scores buttons
	 * 3 = tutorial
	 * 4 = newbie tutorial
	 * 5 = mode select
	 * 6 = high scores on screen (planets/points)
	*/ 


	public string [] highscoreText;				//array for handling the highscore
	public bool tutorialFlag;					//flag for checking if the player is reading the controls
	public Sprite introLogoPC;					//holder for the logo image for pc build

	private string highLoop;					//handles the loop of the highscore
	private int i;								//looping variable
	private Text gameOverText;					//text for the highscores
	private int tempLength;						//used to calculate the length of the highscores, so they can be written in the 6 digit number format

	//below here are all the variables that hold the different menu items, in order to switch them on or off
	private Text gameModeText;
	private GameObject introLogoImage;				
	private Text miniLogoImage;
	private Image miniLogoImageBot;
	private GameObject menuButtonAggregate;
	private GameObject gameModeAggregate;
	private Image tutorialImage;
	private Text versionText;
	private AudioSource source;
	private GameObject muteMusicButtom;
	private GameObject selectionAggregate;
	private GameObject highScoresButtonsAggregate;
	private Image deleteAllButton;
	private GameObject logoObj;
	private GameObject invadedPlanets;
	private GameObject lastInvaded;

	void Awake () {
		ApplicationModel.gameType = 10;
	}

	//mutes and unmutes the music
	public void MuteMusic () {
		if (source.isPlaying) {
			source.Stop ();
			muteMusicButtom.GetComponent<Image> ().color = Color.gray;
			SaveLoad.thisOne.gameSettings [1] = 1;
		} else {
			source.Play ();
			muteMusicButtom.GetComponent<Image> ().color = Color.white;
			SaveLoad.thisOne.gameSettings [1] = 0;
		}
		SaveLoad.Save ();
	}

	void Start () {

		//finding references and preparing the menu
		introLogoImage = GameObject.Find ("IntroLogo");
		tutorialImage = GameObject.Find ("TutorialImage").GetComponent<Image> ();
		miniLogoImage = GameObject.Find ("Logo").GetComponent<Text> ();
		miniLogoImageBot = GameObject.Find ("LogoBottom").GetComponent<Image> ();
		gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
		menuButtonAggregate = GameObject.Find ("MenuButtons");
		gameModeAggregate = GameObject.Find ("GameModeButtons");
		versionText = GameObject.Find("VersionText").GetComponent<Text>();
		gameModeText = GameObject.Find("ModeText").GetComponent<Text>();
		muteMusicButtom = GameObject.Find ("MusicButtom");
		source = GetComponent<AudioSource>();
		selectionAggregate = GameObject.Find ("SelectionButtons");
		deleteAllButton = GameObject.Find ("ResetPrefsButton").GetComponent<Image> ();
		highScoresButtonsAggregate = GameObject.Find ("HighScoresButtons");
		logoObj = GameObject.Find ("LogoDef");
		invadedPlanets = GameObject.Find ("PlanetsButton");
		lastInvaded = GameObject.Find ("Planet");

		if(!ApplicationModel.paoDuro)
			Destroy(GameObject.Find("Comprar"));


		menuButtonAggregate.SetActive (false);
		selectionAggregate.SetActive (false);
		highScoresButtonsAggregate.SetActive (false);
		muteMusicButtom.SetActive (false);
		invadedPlanets.SetActive (false);
		lastInvaded.SetActive (false);
		gameModeAggregate.SetActive (false);
		deleteAllButton.enabled = false;
		gameOverText.enabled = false;
		gameModeText.enabled = false;
		miniLogoImage.enabled = false;
		miniLogoImageBot.enabled = false;
		tutorialImage.enabled = false;
		tutorialFlag = false;

		gameStateFlag = 0;
		highscoreText = new string [3];

		SaveLoad.Load ();

		//reading the stored highscore
		for (i = 0; i < SaveLoad.thisOne.highscore.Length; i++) {
			highscoreText[i] =  SaveLoad.thisOne.highscore[i].ToString();
			tempLength = highscoreText [i].Length;
			for (int j = 0; j < 6 - tempLength; j++)
				highscoreText [i] = "0" + highscoreText [i];
			highscoreText [i] = i + ". " + highscoreText [i];
		}

		//check if the player muted the music in a previous play

		gameOverText.text = "HIGH SCORE\n" + highscoreText [0] + "\n" + highscoreText [1] + "\n" + highscoreText [2];;


		if (SaveLoad.thisOne.gameSettings [1] == 1)
			MuteMusic ();
		

		versionText.text = "Version: " + Application.version+" ";
		if (ApplicationModel.paoDuro)
			versionText.text += "TRIAL";

		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		GameObject.Find ("ContinueText").GetComponent<Text> ().text= "press S to start";
		#endif
	}

	public void SendoFodao () {
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.Games_Oyo.Space_Invading");
	}


	//function for the start the game button
	public void StartGame() {
		ApplicationModel.EnemyTemp = new int[] { 0, 0, 0, 0 };
		if(ApplicationModel.paoDuro) {
			ApplicationModel.gameType = 2;
			SceneManager.LoadScene ("GameBase");
		}
		if (SaveLoad.thisOne.gameSettings[0] == 0) { 				//if it's the first time playing the game, starts the tutorial
			SaveLoad.thisOne.gameSettings[0] = 1;
			ApplicationModel.tutorialEntry = 1;
			ApplicationModel.gameType = 3;
			SceneManager.LoadScene ("GameBase");
		} else {											//otherwise open up the game mode selection
			menuButtonAggregate.SetActive (false);
			gameModeAggregate.SetActive (true);
			gameStateFlag = 5;
		}
	}

	//function for the original mode button
	public void StartOriginal () {							//this funtion open up the mode description, if the player has pressed the button for the first time or start the game if the player is confirming his selection
		if (ApplicationModel.gameType == 0) {
			ApplicationModel.planetsInvaded = 0;
			SceneManager.LoadScene ("PlanetSel");
		} else {
			gameModeText.enabled = true;
			ApplicationModel.gameType = 0;
			gameModeText.text = 
				"Invade planet after\n" +
				"planet and become\n" +
				"the most dangerous\n" +
				"Space Invader!";
		}
	}

	public void StartDrainer () {
		if (ApplicationModel.gameType == 2) {				//this works the same as the Original game mode button
			SceneManager.LoadScene ("GameBase");
		} else {
			gameModeText.enabled = true;
			ApplicationModel.gameType = 2;
			gameModeText.text = 
				"Face an endless\n" +
				"army of defenders\n" +
				"and discover how\n" +
				"strong you are!";
		}
	}

	//just closing the game, used by the Quit button
	public void QuitGame () {
		Application.Quit ();
	}

	//displays the highscores
	public void BraggingRights () {
		highScoresButtonsAggregate.SetActive (true);
		menuButtonAggregate.SetActive (false);
		gameStateFlag = 2;
	}


	public void CheckPlanetList () {
		highScoresButtonsAggregate.SetActive (false);
		gameStateFlag = 6;
		invadedPlanets.SetActive (true);
		for (i = 0; i < SaveLoad.thisOne.planetsInvaded && i < 3; i++) {
			GameObject.Find ("Planet" + i).GetComponentInChildren<Text> ().text = ApplicationModel.namesList [SaveLoad.thisOne.names [i]];
		}

	}

	public void UpdatePlanetImage (int idx) {
		if (!lastInvaded.activeInHierarchy)
			lastInvaded.SetActive (true);
		
		if (SaveLoad.thisOne.planetsInvaded > idx) {
			foreach (Image part in lastInvaded.GetComponentsInChildren<Image>()) {
				switch (part.gameObject.name) {
				case "Base":
					part.sprite = PlanetsPartsRepo.instance.partsBase [SaveLoad.thisOne.bases [idx]];
					break;
				case "Land":
					part.sprite = PlanetsPartsRepo.instance.partsLand [SaveLoad.thisOne.lands [idx]];
					break;
				case "Atmosphere":
					part.sprite = PlanetsPartsRepo.instance.partsAtmosphere [SaveLoad.thisOne.atmospheres [idx]];
					break;
				}
			}
		}
	}

	public void CheckArcadePoints () {
		gameOverText.enabled = true;
		highScoresButtonsAggregate.SetActive (false);
		gameStateFlag = 6;
	}


	//boots up the tutorial
	public void LearnToPlay () {
		ApplicationModel.tutorialEntry = 2;
		ApplicationModel.gameType = 3;
		SceneManager.LoadScene ("GameBase");
	}

	//clean Settings
	public void DeleteData () {
		menuButtonAggregate.SetActive (false);
		selectionAggregate.SetActive (true);
	}

	//confirm cleaning
	public void YesDelete () {
		SaveLoad.TotalZero ();
		SceneManager.LoadScene ("SplashArt");
	}

	//rejects cleaning
	public void NoDelete () {
		menuButtonAggregate.SetActive (true);
		selectionAggregate.SetActive (false);
	}

	void Update () {																	//this is just checking for random touchs on empty parts, when they are unexpected
		#if UNITY_STANDALONE || UNITY_WEBPLAYER
		if (Input.GetKeyDown (KeyCode.S))
		if (gameStateFlag != 1 && gameStateFlag != 5) ChangeScreens();

		if (Input.GetMouseButtonDown(0))
		if (gameStateFlag != 1 && gameStateFlag < 5 && gameStateFlag != 2 ) ChangeScreens();

		#elif UNITY_IOS || UNITY_ANDROID || UNITY_WP8 || UNITY_IPHONE
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) 
		if (gameStateFlag != 1 && gameStateFlag < 5) ChangeScreens();

		#endif

		if (Input.GetKeyDown (KeyCode.Escape))
			BackButtom();
	}

	public void ChangeScreens (){									//handler of the transitions
		switch (gameStateFlag) {
		case 0:
			introLogoImage.SetActive(false);
			miniLogoImage.enabled = true;
			miniLogoImageBot.enabled = true;
			logoObj.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
			logoObj.transform.position = new Vector3 (0, 2.5f, -3f);
			menuButtonAggregate.SetActive (true);
			gameStateFlag = 1;
			muteMusicButtom.SetActive (true);
			deleteAllButton.enabled = true;
			break;
		case 1:
			StartGame ();
			break;
		case 2:
			highScoresButtonsAggregate.SetActive(false);
			menuButtonAggregate.SetActive (true);
			gameStateFlag = 1;
			break;
		case 3:
			LearnToPlay ();
			gameStateFlag = 1;
			break;
		case 4:
			ApplicationModel.gameType = 1;
			StartOriginal ();
			break;
		case 5:
			switch (ApplicationModel.gameType) {
			case 0:
				StartOriginal ();
				break;
			case 2:
				StartDrainer ();
				break;
			default:
				menuButtonAggregate.SetActive (true);
				gameModeAggregate.SetActive (false);
				gameStateFlag = 1;
				break;
			}
			break;
		case 6:
			gameOverText.enabled = false;
			menuButtonAggregate.SetActive (true);
			invadedPlanets.SetActive (false);
			gameStateFlag = 1;
			break;
		}
		return;
	}
		
	public void BackButtom () {									//handler of transitions for when the player pressed esc or the back button on mobile
		switch (gameStateFlag) {
		case 0:
			QuitGame ();
			break;
		case 1:
			introLogoImage.SetActive (true);
			miniLogoImage.enabled = false;
			miniLogoImageBot.enabled = false;
			logoObj.transform.localScale = new Vector3 (1f, 1f, 1f);
			logoObj.transform.position = new Vector3 (0, 0, -3f);
			menuButtonAggregate.SetActive (false);
			gameStateFlag = 0;
			muteMusicButtom.SetActive (false);
			deleteAllButton.enabled = false;
			break;
		case 2:
			highScoresButtonsAggregate.SetActive (false);
			menuButtonAggregate.SetActive (true);
			gameStateFlag = 1;
			break;
		case 3:
			LearnToPlay ();
			gameStateFlag = 1;
			break;
		case 4:
			tutorialImage.enabled = false;
			miniLogoImage.enabled = true;
			logoObj.transform.localScale = new Vector3 (0.7f, 0.7f, 0.7f);
			logoObj.transform.position = new Vector3 (0, 2.5f, -3f);
			menuButtonAggregate.SetActive (true);
			gameStateFlag = 1;
			break;
		case 5:
			menuButtonAggregate.SetActive (true);
			gameModeAggregate.SetActive (false);
			gameStateFlag = 1;
			ApplicationModel.gameType = 0;
			gameModeText.enabled = false;
			break;
		case 6:
			gameOverText.enabled = false;
			menuButtonAggregate.SetActive (true);
			invadedPlanets.SetActive (false);
			gameStateFlag = 1;
			break;
		}
		return;
	}
}
