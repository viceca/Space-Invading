using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Planet : MonoBehaviour {

	public int finalName;
	public int[] planetPartIdx;
	public int[] enemyIdx;

	private int i;
	private Image[] sr;
	private int tempSize;

	// Use this for initialization
	void Start () {
		tempSize = ApplicationModel.namesList.Length;
		finalName = Random.Range (0, tempSize);
		enemyIdx = new int[4];

		sr = GetComponentsInChildren<Image> ();
		tempSize = PlanetsPartsRepo.instance.partsBase.Length;
		planetPartIdx = new int[sr.Length];
		for (i = 0; i < sr.Length; i++) {
			planetPartIdx[i] = Random.Range(0, tempSize);
			switch (sr [i].gameObject.name) {
			case "Base":
				sr [i].sprite = PlanetsPartsRepo.instance.partsBase [planetPartIdx[i]];
				break;
			case "Land":
				sr [i].sprite = PlanetsPartsRepo.instance.partsLand [planetPartIdx[i]];
				break;
			case "Atmosphere":
				sr [i].sprite = PlanetsPartsRepo.instance.partsAtmosphere [planetPartIdx[i]];
				break;
			}
		}

		for (i = 0; i < enemyIdx.Length - 1; i++) {
			enemyIdx [i] = Random.Range (0, EnemiesPool.instance.partsFront.Length);
		}
		enemyIdx [3] = 1;
	}

	public void CallManager () {
		if (!MapManager.instance.pausedSel) {
			MapManager.instance.PlanetButton (finalName, planetPartIdx, 
				GameObject.Find ("Main Camera").GetComponent<Camera> ().ScreenToWorldPoint (GetComponent<RectTransform> ().position));
			EnemiesPool.instance.MapUpdate (enemyIdx);
			ApplicationModel.EnemyTemp = enemyIdx;
		}
	}
}
