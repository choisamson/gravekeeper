using UnityEngine;
using System.Collections;

/// <summary>
/// This script is attached to the Trigger GameObject on the player and
/// it manages the health of the player across the network and applies
/// damage to the player across the network.
/// 
/// This script accesses the PlayerDatabase script to check the PlayerList.
/// 
/// This script is accessed by the BlasterScript.
/// </summary>

public class HealthAndDamage : MonoBehaviour {
	
	//Variables Start___________________________________
	
	private GameObject parentObject;
	
	
	//Used in figuring out on who's computer the damage should be applied.
	
	public string myAttacker;
	
	public bool takeDamage;
	
	
	//These variables are used in figuring out what the player has been hit
	//by and how much damage to apply.
	
	public bool steppedOnTrap = false;
	
	private float trapDamage = maxHealth / 5 ;
	
	
	//This is used to prevent the player from getting hit while they are
	//undergoing destruction.
	
	private bool destroyed = false;
	
	
	//These variables are used in managing the player's health.
	
	public float myHealth = 100;
	
	public float maxHealth = 100;

	
		
	//Variables End_____________________________________
	
	
	// Use this for initialization
	void Start () 
	{
		//The trigger GameObject is used in hit detection but it is
		//the parent that needs to be destroyed if the player's health
		//falls below 0.
		
		parentObject = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the player is hit by an opposing team projectile, 
		//then that projectile will have set takeDamage to true.
		
		if(takeDamage == true)
		{
			GameObject gameManager = GameObject.Find("GameManager");
			
			PlayerDatabase dataScript = gameManager.GetComponent<PlayerDatabase>();
			
			
			//Sift through the player list and only carry out hit detection if the 
			//attacking player is the one running this game instance.
			
			for(int i = 0; i < dataScript.PlayerList.Count; i++)
			{
				if(myAttacker == dataScript.PlayerList[i].playerName)
				{
					if(int.Parse(Network.player.ToString()) == dataScript.PlayerList[i].networkPlayer)
					{
						//Check what the player was hit by and apply damage.
						
						if(steppedOnTrap == true && destroyed == false)
						{
							myHealth = myHealth - trapDamage;	
							
							//Send out an RPC so that this player's attacker is 
							//updated across the network. This way the attacker
							//can receive a score for destroying the enemy player.
							
							networkView.RPC("UpdateMyCurrentAttackerEverywhere",
							                RPCMode.Others, myAttacker);
							
							
							//Send out an RPC so that this player's health is reduced
							//across the network.
							
							networkView.RPC("UpdateMyCurrentHealthEverywhere",
							                RPCMode.Others, myHealth);
						}
					}
				}
			}
			
			takeDamage = false;
			
		}
		
		
		//Each player is responsible for destroying themselves.
		
		if(myHealth <= 0 && networkView.isMine == true)
		{
			//Remove this player's RPCs. If we didn't do this a
			//ghost of this player would remain in the game which
			//would be very confusing to players just connecting
			
			Network.RemoveRPCs(Network.player);
			
			
			//Send out an RPC to destroy our player across the network.
			
			networkView.RPC("DestroySelf", RPCMode.All);
		}
		
		
		//Regen the player's health if it is below the max health.
		
		if(myHealth < maxHealth)
		{
			myHealth = myHealth ;
		}
		
		
		//If the player's health exceeds the max health while regenerating
		//then set it back to the max health.
		
		if(myHealth > maxHealth)
		{
			myHealth = maxHealth;	
		}
	
	}
	
	
	[RPC]
	void UpdateMyCurrentAttackerEverywhere (string attacker)
	{
		myAttacker = attacker;	
	}
	
	
	[RPC]
	void UpdateMyCurrentHealthEverywhere (float health)
	{
		myHealth = health;	
	}
	
	
	[RPC]
	void DestroySelf ()
	{
		Destroy(parentObject);	
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}
