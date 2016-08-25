using UnityEngine;
using System.Collections;

public class ExplodesHolder : MonoBehaviour {

	public static ExplodesHolder instance = null;

	public GameObject[] explodePool = new GameObject[8];
	public GameObject explodeBase;

	private int i;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	}

	// Use this for initialization
	void Start () {
		for (i = 0; i<explodePool.Length; i++) 
			Instantiate (explodeBase, new Vector3 (0, 20, 0), Quaternion.identity);
		explodePool = GameObject.FindGameObjectsWithTag ("explode");

		for (i = 0; i < explodePool.Length; i++)
			explodePool [i].SetActive (false);
	}

	public void PoolRunner (Vector3 tempPos, int tempScale) {
		GameObject tempHolder = null;
		for (i = 0; i < explodePool.Length; i++) {
			if (!explodePool [i].activeInHierarchy){
				explodePool [i].SetActive (true);
				tempHolder = explodePool [i];
			}
		}
		if(tempHolder == null) 
			tempHolder = explodePool [i-1];

		tempHolder.transform.position = tempPos;
		tempHolder.transform.localScale = new Vector3(tempScale,tempScale,1);
		tempHolder.SetActive (true);
	}
}
