using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game { 

	public int planetsInvaded;
	public int[] names = new int[3];
	public int[] bases = new int[3];
	public int[] lands = new int[3];
	public int[] atmospheres = new int[3];
	public int[] highscore = new int[3];
	public int[] gameSettings = new int[3];
	/* store bool in integers
	 * 0 = played before
	 * 1 = mute music
	 * 2 = mute game sounds
	 * */

	public Game () {

	}

}