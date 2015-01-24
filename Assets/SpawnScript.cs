using UnityEngine;
using System.Collections;
/// <summary>
/// This script is attached to the SpawnManager and it allows the player to spawn into the
/// the multiplayer game.
/// </summary>
public class SpawnScript : MonoBehaviour {
	//Variable start
	//Used to determine if player needs to spawn into the game
	private bool justConnectedToServer =false;

	//Used to determine player's team
	public bool amIMonster = false;
	public bool amIHuman = false;

	//Define the JoinTeamWindow

	private Rect joinTeam;
	private string joinTeamWindowTitle = "Team Selection";
	private int joinTeamWindowWidth = 330;
	private int joinTeamWindowHeight = 100;
	private int joinTeamLeftIndent;
	private int joinTeamTopIndent;
	private int buttonHeight = 40;

	//PlayerPrefabs connected to these
	public Transform monster;
	public Transform human;
	private int monsterGroup = 0;
	private int humanGroup = 1;

	//Used to reference spawn points
	private GameObject[]monsterSpawnPoints;
	private GameObject[]humanSpawnPoints;

	//Variable end

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnConnectedToServer (){
		justConnectedToServer = true;
	}

	void JoinTeamWindow (int windowID){
		//If the player clicks Monster Button then make them Monster
		if (GUILayout.Button ("Monster", GUILayout.Height (buttonHeight))) {

			amIMonster = true;
			justConnectedToServer = false;
			SpawnMonster();
		}
		//If the player clicks Human Button then make them Human
		if (GUILayout.Button ("Human", GUILayout.Height (buttonHeight))) {
			
			amIHuman = true;
			justConnectedToServer = false;
			SpawnHuman();
		}
	}

	void OnGUI(){
		//Ifthe player has just connected to the server then draw the JoinTeamWindow.
		if (justConnectedToServer == true) {

			joinTeamLeftIndent = Screen.width/2 - joinTeamWindowWidth/2;
			joinTeamTopIndent = Screen.height/2 -joinTeamWindowHeight/2;

			joinTeam = new Rect (joinTeamLeftIndent, joinTeamTopIndent, joinTeamWindowWidth, joinTeamWindowHeight);

			joinTeam = GUILayout.Window(0, joinTeam, JoinTeamWindow, joinTeamWindowTitle);
		}
	}

	void SpawnMonster(){
		//Find Monster Spawns and place reference to them in the array monsterSpawnPoints
		monsterSpawnPoints = GameObject.FindGameObjectsWithTag ("SpawnMonster");

		//Randomly select one of those spawn points
		GameObject randomMonsterSpawn = monsterSpawnPoints [Random.Range (0, monsterSpawnPoints.Length)];

		//Instantiate player at randomly selected spawn point.

		Network.Instantiate (monster, randomMonsterSpawn.transform.position, randomMonsterSpawn.transform.rotation, monsterGroup);
	}

	void SpawnHuman(){
		//Find Human Spawns and place reference to them in the array monsterSpawnPoints
		humanSpawnPoints = GameObject.FindGameObjectsWithTag ("SpawnHuman");
		
		//Randomly select one of those spawn points
		GameObject randomHumanSpawn = humanSpawnPoints [Random.Range (0, humanSpawnPoints.Length)];
		
		//Instantiate player at randomly selected spawn point.
		
		Network.Instantiate (monster, randomHumanSpawn.transform.position, randomHumanSpawn.transform.rotation, humanGroup);
	}
		
}
