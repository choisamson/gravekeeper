using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

	private bool justConnectedToServer = false;
	
	public Transform humanPrefab;
	public Transform monsterPrefab;

	private int numHuman = 0;
	private int numMonster = 0;
	private int maxMonster ;
	
	public bool monsterExists = false;
	public bool humanExists = false;

	//Used to determine which team the player is on.
	public bool isMonster = false;
	public bool isHuman = false;
	
	
	//Used to define the JoinTeamWindow.
	
	private Rect joinTeamMenu;
	private string joinTeamWindowTitle = "Team Selection";
	private int joinTeamWindowWidth = 330;
	private int joinTeamWindowHeight = 100;
	private int joinTeamLeftIndent;
	private int joinTeamTopIndent;
	private int buttonHeight = 40;
	public bool matchRestart = false;
	public bool firstSpawn = false;
	private bool gameOver = false;

	void Start(){

		maxMonster = numHuman / 2;

	}

	void OnServerInitialized(){ 
		justConnectedToServer = true;
	}

	void OnConnectedToServer(){ 
		justConnectedToServer = true;
	}
	// New_______________________________________________________
	void Update(){
		if (!monsterExists) {
			GameObject monster = GameObject.Find ("Monster(Clone)");
			if(monster != null){
				monsterExists = true;
			}
		}
		
		if (!humanExists) {
			GameObject human = GameObject.Find ("Human(Clone)");
			if(human != null){
				humanExists = true;
			}
		}
	
	}

	void JoinTeamWindow (int windowID)
	{
		if (justConnectedToServer == true || matchRestart == true) {
		//If the player clicks on the Stay Human button then
		//assign them to the humans team and spawn them into the game.

		if (!humanExists){

			if (GUILayout.Button ("Stay Human", GUILayout.Height (buttonHeight))) {

				isHuman = true;
				
				justConnectedToServer = false;
				
				matchRestart = false;

				SpawnHuman ();	
				
				firstSpawn = true;
				
								
			}

			GUI.enabled =true;
			
		}
						//If the player clicks on the Join Blue Team button then
						//assign them to the blue team and spawn them into the game.
			if(!monsterExists){

				if (GUILayout.Button ("Become a Monster", GUILayout.Height (buttonHeight))) {

					isMonster = true;
				
					justConnectedToServer = false;
				
					matchRestart = false;

					SpawnMonster ();

					firstSpawn = true;
				}
				GUI.enabled = true; 
			}

		}
	}
	//New End___________________________________________________
	void SpawnMonster(){ 

		Network.Instantiate (monsterPrefab, new Vector3 (36, 0, 0), transform.rotation, 0);
	}

	void SpawnHuman(){
		Network.Instantiate (humanPrefab, new Vector3(-36, 0, 0), transform.rotation, 0);
	}

	void OnGUI()
	{
		//If the player has just connected to the server then draw the 
		//Join Team window.
		
		if(justConnectedToServer == true)
		{	
			
			joinTeamLeftIndent = Screen.width / 2 - joinTeamWindowWidth / 2;
			
			joinTeamTopIndent = Screen.height / 2 - joinTeamWindowHeight / 2;
			
			joinTeamMenu = new Rect(joinTeamLeftIndent, joinTeamTopIndent,
			                        joinTeamWindowWidth, joinTeamWindowHeight);
			
			joinTeamMenu = GUILayout.Window(0, joinTeamMenu, JoinTeamWindow,
			                                joinTeamWindowTitle);
		}

		if(gameOver){
			HumanMovement human = GameObject.Find("Human(Clone)").GetComponent<HumanMovement>();
			MonsterMovement monster = GameObject.Find("Monster(Clone)").GetComponent<MonsterMovement>();

			if (human.Alive && monster.Alive) {
				if(isHuman){
					GUILayout.Label("MONSTER HATH FAILETH TO EATETH THOU ARSE");
				} else if (isMonster) {
					GUILayout.Label("YOU HATH FAILETH TO EATETH THE WIZARD");
				}
			} else if (human.Alive && !monster.Alive) {
				if(isHuman){
					GUILayout.Label("YOU HATH SLAIN THE ENEMY");
				} else if (isMonster) {
					GUILayout.Label("YOU HATH BEEN SLAIN");
				}
			} else if (!human.Alive && monster.Alive) {
				if(isHuman){
					GUILayout.Label("YOU HATH BEEN MAULETH TO DEATH");
				} else if (isMonster) {
					GUILayout.Label("YOU HATH EATETH THE WIZARD");
				}
			} else {
				GUILayout.Label("SUICIDE CULT?");
			}

			if(GUILayout.Button("End Match",GUILayout.Height (buttonHeight))){
				Network.Disconnect();
			}
		}
	}

	public bool GameOver {
		get{
			return gameOver;
		}
		set {
			gameOver = value;
		}
	}
}
