using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the ReloadLevel GameObject.
/// 
/// This script acceses the PlayerDatabase.
/// 
/// This script accesses the SpawnScript.
/// 
/// This script is accessed by the ScoreTable script.
/// </summary>

public class ReloadLevelScript : MonoBehaviour {
		
	//Variables Start___________________________________
	
	public bool reloadLevel = false;
	
	public bool iAmRestartingMatch = false;
	
	public float waitTime = 0.1f;
	
	private static bool created = false;
	
	//Variables End_____________________________________
	
	
	void Awake ()
	{
		//The ReloadLevel GameObject will be the only GameObject to remain
		//when the level reloads. Everything else will be re created and we
		//need to ensure that a new copy of this ReloadLevel GameObject is not
		//duplicated each time the level reloads.
		
		if(created == false)
		{
			DontDestroyOnLoad(gameObject);
			
			created = true;
		}
		
		else
		{
			Destroy(gameObject);	
		}
	}
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//The ScoreTable script will set reloadLevel to true when a team
		//wins and this happens on the server only. The server tells all the
		//players to reload the level.
		
		if(reloadLevel == true && Network.isServer)
		{
			//Send an RPC to everyone telling them to restart the match.
			
			networkView.RPC("RestartMatch", RPCMode.All);
			
			reloadLevel = false;
		}
		
		if(iAmRestartingMatch == true)
		{
			//Access the PlayerDatabase and tell it that the match is restarting.
			//The PlayerDatabase will then have each of the players added back into
			//the player list.
			
			GameObject gameManager = GameObject.Find("GameManager");
			
			PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
			
			dataScript.matchRestarted = true;
			
						
			//Access the SpawnScript and tell it that the match is restarting. The
			//SpawnScript will then allow the player to choose a team again.
			
			GameObject spawnManager = GameObject.Find("SpawnManager");
			
			SpawnScript spawnScript = spawnManager.GetComponent<SpawnScript>();
			
			spawnScript.matchRestart = true;
			
			
			iAmRestartingMatch = false;
		}
	}
	
	
	[RPC]
	void RestartMatch ()
	{
		//Delete all RPCs and stop network communications.
		
		Network.RemoveRPCs(Network.player);
		
		Network.SetSendingEnabled(0, false);
		
		Network.SetSendingEnabled(1, false);
		
		Network.isMessageQueueRunning = false;
		
		Application.LoadLevel("Series 1 Prototype");
		
		
		//Use a coroutine to give the level a few moments to load before allowing 
		//network communications to resume.
		
		StartCoroutine(Delay());
		
	}
	
	
	IEnumerator Delay()
	{
		yield return new WaitForSeconds(waitTime);
		
		Network.isMessageQueueRunning = true;
		
		Network.SetSendingEnabled(0, true);
		
		Network.SetSendingEnabled(1, true);
		
		
		iAmRestartingMatch = true;
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
