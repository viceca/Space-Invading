using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour {

	public int alphaChanger;
	public float tempAlpha;

	private Image self;

	// Use this for initialization
	void Start () {
		alphaChanger = 1;
		tempAlpha = 0;
		self = GetComponent<Image> ();
		self.color = new Color (1, 1, 1, tempAlpha);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (alphaChanger == 1) {
			print ("hey");
			if (tempAlpha < 1) {
				tempAlpha += 0.02f;
				self.color = new Color (1, 1, 1, tempAlpha);
			} else {
				alphaChanger = 0;
				StartCoroutine (HoldImage ());
			}
		}

		if (alphaChanger == -1) {
			if (tempAlpha > 0) {
				tempAlpha -= 0.02f;
				self.color = new Color (1, 1, 1, tempAlpha);
			} else {
				SceneManager.LoadScene ("MainMenu");
			}
		}
	}

	IEnumerator HoldImage () {
		yield return new WaitForSeconds (0.5f);
		alphaChanger = -1;
	}
}
