using UnityEngine;
using System.Collections;

public class LiveManager : MonoBehaviour {

	//lives drawing managing
	public float scale;
	public RectTransform rectTransform;
	public float maxLives;

	public static LiveManager instance = null;

	void Awake()
	{
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);
		maxLives = 5f;
	}

	// Use this for initialization
	void Start () {

		rectTransform = GetComponent<RectTransform>();
		scale = 1 - (((float) Repo.instance.startinLives) / (maxLives));

	}
	
	// Update is called once per frame
	void Update() {
		rectTransform.localScale = new Vector3 (scale, rectTransform.localScale.y, rectTransform.localScale.z);
	}

	public void UpdateLife(int lives){
		scale = 1 - (((float) lives) / (maxLives));
	}

}
