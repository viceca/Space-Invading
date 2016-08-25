using UnityEngine;
using System.Collections;

public class PlanetDummy : MonoBehaviour {

	private SpriteRenderer[] sr;

	void Start () {
		sr = GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer child in sr) {
			switch (child.gameObject.name) {
			case "Base":
				child.sprite = PlanetsPartsRepo.instance.partsBase [ApplicationModel.tempPlanet [1]];
				break;
			case "Land":
				child.sprite = PlanetsPartsRepo.instance.partsLand [ApplicationModel.tempPlanet [2]];
				break;
			case "Atmosphere":
				child.sprite = PlanetsPartsRepo.instance.partsAtmosphere [ApplicationModel.tempPlanet [3]];
				break;
			}
		}

		GetComponent<Rigidbody2D> ().velocity = new Vector3 (0, -0.5f, 0);

	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < 3.5f) {
			GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
			Player.instance.playing = false;
			StartCoroutine (SceneTimer ());
		}
	}

	IEnumerator SceneTimer () {
		Player.instance.rb2d.velocity = new Vector3 (0, -0.5f, 0);
		yield return new WaitForSeconds (0.5f);
		Player.instance.rb2d.velocity = -Player.instance.maxSpeed * (Player.instance.transform.position - transform.position);
		Player.instance.transform.up = Player.instance.rb2d.velocity;
		ApplicationModel.gameType = 0;
	}

	void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "player") {
			Manager.instance.beginText.text = ApplicationModel.namesList [ApplicationModel.tempPlanet [0]] + "\nwas Invaded!";
			Manager.instance.beginText.enabled = true;
			StartCoroutine (EndScene ());

		}
	}

	IEnumerator EndScene () {
		yield return new WaitForSeconds(1);
		Manager.instance.MapSelAgain ();
	}

}
