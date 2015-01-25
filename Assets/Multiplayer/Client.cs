using UnityEngine;
using System.Collections;

public class Client : MonoBehaviour {

	private bool justConnectedToServer = false;
	
	public Transform humanPrefab;
	public Transform monsterPrefab;

	private Transform human;
	private Transform monster;

	private int numHuman = 0;
	private int numMonster = 0;
	private int maxMonster ;

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

	void Start(){

		maxMonster = numHuman / 2;

	}

	void OnServerInitialized(){ 
		justConnectedToServer = true;
	}


	void OnConnectedToServer(){ 
		justConnectedToServer = true;
	}
	void JoinTeamWindow (int windowID)
	{
		if (justConnectedToServer == true) {
						//If the player clicks on the Join Red Team button then
						//assign them to the red team and spawn them into the game.

			if (numMonster == 0 && numHuman > 0){
				GUI.enabled = false;
			}

			if (GUILayout.Button ("Stay Human", GUILayout.Height (buttonHeight))) {

				isHuman = true;
				
				justConnectedToServer = false;
				
				SpawnHuman ();
									
				numHuman++;
								
			}
			GUI.enabled =true;
			
			
						//If the player clicks on the Join Blue Team button then
						//assign them to the blue team and spawn them into the game.
			
			if (numMonster > maxMonster){
				GUI.enabled = false;
			}

			if (GUILayout.Button ("Become a Monster", GUILayout.Height (buttonHeight))) {
					
					isMonster = true;
				
					justConnectedToServer = false;
				
					SpawnMonster ();
									
					numMonster ++;
			}
			GUI.enabled = true;
		}
	}

	void SpawnMonster(){ 

		monster = (Transform)Network.Instantiate (monsterPrefab, new Vector3 (36, 0, 0), transform.rotation, 0);
	}
	void SpawnHuman(){
		human = (Transform)Network.Instantiate (humanPrefab, new Vector3(-36, 0, 0), transform.rotation, 0);
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
	}
}
