using UnityEngine;
using System.Collections;
using System.Collections.Generic; 
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class SaveLoad {

	static public Game thisOne = new Game();

	public static void Save() {
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + "/gameData.oyosg");
		bf.Serialize(file, SaveLoad.thisOne);
		file.Close();
	}

	public static void TotalZero () {
		thisOne.planetsInvaded = 0;
		ZeroOut (thisOne.atmospheres, 3);
		ZeroOut (thisOne.bases, 3);
		ZeroOut (thisOne.lands, 3);
		ZeroOut (thisOne.names, 3);
		ZeroOut (thisOne.highscore, 3);
		ZeroOut (thisOne.gameSettings, 3);
		Save ();
	}

	static void ZeroOut (int[] arrayZero, int size) {
		Debug.Log ("I Ran");
		arrayZero = new int[size];
		for (int i = 0; i < arrayZero.Length; i++)
			arrayZero [i] = 0;
	}

	public static void Load () {
		if (File.Exists (Application.persistentDataPath + "/gameData.oyosg")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream saveFile = File.Open (Application.persistentDataPath + "/gameData.oyosg", FileMode.Open);
			thisOne = (Game)bf.Deserialize (saveFile);
			saveFile.Close ();
			TestVariable ();
		} else {
			TotalZero ();
		}
	}

	public static void TestVariable () {
		for (int i = 0; i < thisOne.GetType ().GetFields ().Length; i++) {
			thisOne.GetType ().GetFields () [i].GetValue(thisOne);
			Debug.Log(thisOne.GetType ().GetFields () [i].Name+" type is "+ thisOne.GetType ().GetFields () [i].FieldType + " and the value is "+ thisOne.GetType ().GetFields () [i].GetValue (thisOne));
			if (thisOne.GetType ().GetFields () [i].GetValue (thisOne) == null) {
				if (thisOne.GetType ().GetFields () [i].FieldType == typeof(int[]))
					thisOne.GetType ().GetFields () [i].SetValue (thisOne, new int[3] {0,0,0});
				else
					thisOne.GetType ().GetFields () [i].SetValue (thisOne, 0);
			}
		}
	}


}

