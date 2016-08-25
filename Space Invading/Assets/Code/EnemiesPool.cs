using UnityEngine;
using System.Collections;

public class EnemiesPool : MonoBehaviour {

	public static EnemiesPool instance = null;

	//public Stack enemyPool;
	public GameObject enemyBase;
	public GameObject[] enemyObjPool = new GameObject[5];
	public SpriteRenderer[] sr;
	public Sprite[] partsCore;
	public Sprite[] partsWings;
	public Sprite[] partsFront;
	public int[] partsIdx;
	/*Indexes for each part selected
	 * [0] = core
	 * [1] = wings
	 * [2] = front
	*/

	public Sprite[] partsMotherCore;
	public Sprite[] partsMotherRim;

	private int i;

	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);

		switch (ApplicationModel.gameType) {
		case 10:
			CreateMenuDef ();
			break;
		case 0:
			StartMapDummy ();
			break;
		default:
			StartGamePool ();
			break;
		}
	}

	void StartMapDummy () {
		enemyBase = GameObject.FindGameObjectWithTag ("enemy");
		enemyBase.SetActive (false);
	}

	public void MapUpdate (int[] Idx) {
		enemyBase.SetActive (true);
		sr = enemyBase.GetComponentsInChildren<SpriteRenderer> ();
		foreach (SpriteRenderer child in sr) {
			switch (child.gameObject.name) {
			case "Core":
				child.sprite = partsCore [Idx [0]];
				break;
			case "Wings":
				child.sprite = partsWings [Idx [1]];
				break;
			case "Front":
				child.sprite = partsFront [Idx [2]];
				break;
			}
		}
	}

	void CreateMenuDef () {
		int tempVar;

		enemyObjPool = GameObject.FindGameObjectsWithTag ("enemy");
		for (i = 0; i < enemyObjPool.Length; i++) {
			sr = enemyObjPool [i].GetComponentsInChildren<SpriteRenderer> ();
			foreach (SpriteRenderer child in sr) {
				tempVar = Random.Range (0, partsFront.Length);
				switch (child.gameObject.name) {
				case "Core":
					child.sprite = partsCore [tempVar];
					break;
				case "Wings":
					child.sprite = partsWings [tempVar];
					break;
				case "Front":
					child.sprite = partsFront [tempVar];
					break;
				}
			}
		}
	}

	void StartGamePool () {
		sr = enemyBase.GetComponentsInChildren<SpriteRenderer> ();

		if (ApplicationModel.EnemyTemp [3] == 0)
			for (i = 0; i < partsIdx.Length; i++)
				partsIdx [i] = Random.Range (0, partsCore.Length);
		else
			for (i = 0; i < partsIdx.Length; i++)
				partsIdx [i] = ApplicationModel.EnemyTemp [i];
		Picker (sr);

		for (i = 0; i < enemyObjPool.Length; i++) {
			Instantiate(enemyBase, new Vector2(2f * i, 20f), Quaternion.identity);
		}
		enemyObjPool = GameObject.FindGameObjectsWithTag ("enemy");

		for (i = 0; i < enemyObjPool.Length; i++) {
			enemyObjPool [i].SetActive (false);
		}
	}

	public GameObject PoolRunner () {
		for (i = 0; i < enemyObjPool.Length; i++) {
			if (!enemyObjPool [i].activeInHierarchy){
				enemyObjPool [i].SetActive (true);
				return enemyObjPool [i];
			}
		}
		return enemyObjPool [i];
	}

	public void MapDummyEnabler (bool state) {
		enemyBase.SetActive (state);
	}

	public void RePicker () {
		for (i = 0; i < partsIdx.Length; i++)
			partsIdx [i] = Random.Range (0, partsCore.Length);
		for (i = 0; i < enemyObjPool.Length; i++) {
			sr = enemyObjPool [i].GetComponentsInChildren<SpriteRenderer> ();
			Picker (sr);
		}
	}

	void Picker (SpriteRenderer[] srArray) {
		foreach (SpriteRenderer child in srArray) {
			switch (child.gameObject.name){
			case "Wings":
				child.sprite = partsWings [partsIdx[1]];
				break;
			case "Front":
				child.sprite = partsFront [partsIdx[2]];
				break;
			case "Core":
				child.sprite = partsCore [partsIdx[0]];
				break;
			}
		}
	}

}
