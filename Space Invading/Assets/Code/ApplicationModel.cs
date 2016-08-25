public class ApplicationModel {

	static public bool paoDuro = true;

	// This is just to pass information back and forth scenes loading
	static public int gameType = 10;
	/* Game Types Library
	 * 0 = Map
	 * 1 = Original
	 * 2 = Arcade
	 * 3 = Tutorial
	 * 10 = Main Menu
	 */

	static public int selection = 0;

	static public int planetsInvaded = 0;

	static public bool okToShoot = true;
	//this variable is used in the tutorial to say when it's ok for the player to shoot, it's read in Player.Start()

	static public int tutorialEntry = 1;
	/* Tutorial Entries (how did the Player started the tutorial?)
	 * 1 = First time playing the game - after finishing the tutorial startes the original mode
	 * 2 = Opening the tutorial from the main menu - after finishing the tutorial opens up the main menu
	*/

	static public int tempPoints = 0;

	static public int[] tempPlanet = new int[4];
	/*
	 * 0 = name
	 * 1 = base
	 * 2 = land
	 * 3 = atmosphere
	*/

	static public string[] namesList = new string[] {"Qugloaruta", "Bebreylara", "Duspeon", "Wodryke", "Ieter", "Ualara", "Fronuyama", "Smabophus", "Troth M8M", "Smyke D4Yr", "Osloathea", "Abliohines", "Mostrars", "Puwhillon", "Neovis", "Teocury", "Skafutania", "Broqagawa", "Sleon S92", "Scosie Ne4Y", "Zegraurilia", "Kocloilia", "Cospilles", "Bacrichi", "Hoytania", "Huemia", "Spagocarro", "Glootov", "Shoria V9A", "Smapus Mjt6", "Gesweatera", "Cuscuonia", "Osterth", "Iudrion", "Cuepra", "Uylara", "Grequtania", "Proabos", "Flao 5Fbh", "Blolla O16", "Lapliania", "Cacrougantu", "Fustrapus", "Yupleon", "Iuegantu", "Eihiri", "Shunolea", "Slanohines", "Whuna I90", "Glippe Jz6S", "Oskuotera", "Tasmoyrilia", "Mugriuq", "Rablade", "Aopra", "Keanus", "Skugenus", "Plujelia", "Whagua 03Od", "Clomia Ol6G", "Apreamia", "Seswuiphus", "Tawhilia", "Kotrinda", "Ieiyama", "Johiri", "Blayuruta", "Scokonerth", "Snyria L", "Smolla 26", "Nhenequinhesis"};

	static public int[] EnemyTemp = new int[] {0,0,0,0};

	static public bool gameBegan = false;
}
