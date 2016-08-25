using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour {

	public int planetTempSel;
	public bool pausedSel;

	public static MapManager instance;

	private int i;
	private GameObject menuButtonAggregate;
	private AudioSource source;
	private Image pauseFadeImage;
	private GameObject muteSoundsButtom;
	private GameObject muteMusicButtom;
	private bool mutedSounds;
	private GameObject sureAggregate;
	public Text planetName;

	void Awake() {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}
		

	// Use this for initialization
	void Start () {
		menuButtonAggregate = GameObject.Find ("MenuButtons");
		sureAggregate = GameObject.Find ("SelectionButtons");
		muteMusicButtom = GameObject.Find ("MusicButtom");
		planetName = GameObject.Find ("PlanetName").GetComponent<Text> ();
		source = GetComponent<AudioSource> ();

		pausedSel = true;
		menuButtonAggregate.SetActive (false);
		sureAggregate.SetActive (false);

		if (!PlayerPrefs.HasKey ("musicMute"))
			PlayerPrefs.SetInt ("musicMute", 0);

		PlayerPrefs.Save ();

		if (PlayerPrefs.GetInt ("musicMute") == 1)
			MuteMusic ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			PauseButton ();
		}
	}

	public void QuitGame () {
		ApplicationModel.selection = 2;
		if (!sureAggregate.activeInHierarchy) {
			menuButtonAggregate.SetActive (false);
			sureAggregate.SetActive (true);
		} else {
			ConfirmSelection ();
		}
	}

	public void GoMainMenu () {
		ApplicationModel.selection = 1;
		if (!sureAggregate.activeInHierarchy) {
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

	public void PauseButton () {
		EnemiesPool.instance.MapDummyEnabler (pausedSel);
		planetName.enabled = pausedSel;
		pausedSel = !pausedSel;
		menuButtonAggregate.SetActive (pausedSel);
	}

	public void PlanetButton (int selPlanetName, int[] Idx, Vector3 direction) {
		if (!pausedSel) {
			if (planetName.text == ApplicationModel.namesList[selPlanetName]) {
				DummyMap.instance.SendDummy (direction);
				Destroy (EnemiesPool.instance.enemyBase);
				ApplicationModel.tempPlanet [0] = selPlanetName;
				pausedSel = true;
				for (i = 0; i < Idx.Length; i++) {
					ApplicationModel.tempPlanet [i + 1] = Idx [i];
				}
			} else {
				planetName.text = ApplicationModel.namesList[selPlanetName];
			}
		}
	}

	public void SpawnDummy () {
		DummyMap.instance.transform.position = new Vector2 (0, -3f);
		pausedSel = false;
	}

	public void MuteMusic () {
		if (source.isPlaying) {
			source.Stop ();
			muteMusicButtom.GetComponent<Image> ().color = Color.grey;
			PlayerPrefs.SetInt ("musicMute", 1);
		} else {
			source.Play ();
			muteMusicButtom.GetComponent<Image> ().color = Color.white;
			PlayerPrefs.SetInt ("musicMute", 0);
		}
		PlayerPrefs.Save ();
	}


}
