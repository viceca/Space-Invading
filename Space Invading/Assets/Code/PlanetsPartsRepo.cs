using UnityEngine;
using System.Collections;

public class PlanetsPartsRepo : MonoBehaviour {


	public Sprite[] partsBase;
	public Sprite[] partsLand;
	public Sprite[] partsAtmosphere;

	public static PlanetsPartsRepo instance = null;

	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy (this);
	}
}
