using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to Players.
/// 
/// This script accesses the PlayerDatabase script
/// in the GameManager to access the PlayerList.
/// 
/// This script accesses the SpawnScript to check which
/// team this player is on.
/// 
/// This script accesses the ScoreTable script for 
/// incrementing the team score.
/// 
/// This script is accessed by the HealthAndDamage script.
/// </summary>


public class PlayerScore : MonoBehaviour {
	
	//Variables Start___________________________________
	
	public string destroyedEnemyName;
	
	public bool iDestroyedAnEnemy = false;
	
	public int enemiesDestroyedInOneHit;
	
	public int myScore;
	
	//Variables End_____________________________________
	

	// Use this for initialization
	void Start () 
	{
		if(networkView.isMine == true)
		{
			//When the player spawns they need to access their PlayerList
			//on their game instance and retrieve their score from the list.
			//Otherwise their score would reset to 0 everytime they respawned.
			
			GameObject gameManager = GameObject.Find("GameManager");
			
			PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
			
			for(int i = 0; i < dataScript.PlayerList.Count; i++)
			{
				if(dataScript.PlayerList[i].networkPlayer == int.Parse(Network.player.ToString()))
				{
					myScore = dataScript.PlayerList[i].playerScore;
					
					//When players are destroyed their RPCs are deleted so we need to tell
					//the PlayerDatabase to update itself across the network
					//so that new players will see this player's score correctly.
					
					UpdateScoreInPlayerDatabase(myScore);
				}
			}
		}
		
		else
		{
			enabled = false;	
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//When the player destroys an enemy increment the player's score and
		//save their current score in the PlayerDatabase script.
		
		if(iDestroyedAnEnemy == true)
		{
			//enemiesDestroyedInOneHit is a counter and takes into account
			//if the player destroyed more than one enemy in a single hit. If
			//they have then their score should be incremented for each enemy 
			//destroyed. This can happen with rockets.
			
			for(int i = 0; i < enemiesDestroyedInOneHit; i++)
			{
				myScore++;
				
				UpdateScoreInPlayerDatabase(myScore);
				
				TellScoreTableToUpdateTeamScore();
			}
			
			enemiesDestroyedInOneHit = 0;
			
			iDestroyedAnEnemy = false;
		}
	}
	
	
	void UpdateScoreInPlayerDatabase (int score)
	{
		GameObject gameManager = GameObject.Find("GameManager");
			
		PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();	
		
		dataScript.scored = true;
		
		dataScript.playerScore = score;
	}
	
	
	//Inform the local ScoreTable script that the team this player is on
	//should get a score point because this player has scored. The ScoreTable
	//script will then send out an RPC across the network incrementing this team's
	//score.
	
	void TellScoreTableToUpdateTeamScore ()
	{
		GameObject spawnManager = GameObject.Find("SpawnManager");
		
		SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
		
		
		GameObject gameManager = GameObject.Find("GameManager");
		
		ScoreTable tableScript = gameManager.GetComponent<ScoreTable>();
		
		
		if(spawnScript.amIOnTheBlueTeam == true)
		{
			tableScript.updateBlueScore = true;
			
			tableScript.enemiesDestroyedInOneHit = enemiesDestroyedInOneHit;
		}
		
		if(spawnScript.amIOnTheRedTeam == true)
		{
			tableScript.updateRedScore = true;
			
			tableScript.enemiesDestroyedInOneHit = enemiesDestroyedInOneHit;
		}
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
