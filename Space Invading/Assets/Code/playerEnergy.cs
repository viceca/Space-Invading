using UnityEngine;
using System.Collections;

public class playerEnergy : MonoBehaviour {

	public float downSpeed;

	private Rigidbody2D rb2d;
	private

	// Use this for initialization
	void Start () {
		//set variables values
		downSpeed = 3f;


		//set working values
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = new Vector2(0f, downSpeed);
	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D collision) {
		switch(collision.gameObject.tag) {
		case "wall":
			Destroy (gameObject);
			break;
		}
	}

}
