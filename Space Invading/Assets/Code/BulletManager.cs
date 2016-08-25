using UnityEngine;
using System.Collections;

public class BulletManager : MonoBehaviour {

	//this is the code for the bullet display

	public float scale; 					//scale of the cover
	public RectTransform rectTransform;		//holder for the Rect Transform
	public int maxBullet;					//max number of bullets per time
	public int startBullet;					//the number of bullets when starting the game

	public static BulletManager instance = null;

	//setting the instance reference
	void Awake() {
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);
	}

	void Start () {
		// theses lines define the maximun bullet and the scale used for the cover over the bullets count image
		rectTransform = GetComponent<RectTransform>();
		maxBullet = 10;
		startBullet = Repo.instance.startinBullets;
		scale = 1 - (((float) startBullet) / ((float) maxBullet));

	}

	// Update is called once per frame
	void Update() {
		//this just set the scale (size) of the cover
		rectTransform.localScale = new Vector3 (scale, rectTransform.localScale.y, rectTransform.localScale.z);
	}

	public void ShootBullet(int bullets){
		//updates the scale
		scale = 1 - (((float) bullets) / ((float) maxBullet));
	}

}
