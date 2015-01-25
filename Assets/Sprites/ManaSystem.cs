using UnityEngine;
using System.Collections;
/// <summary>
/// this is attached to the players characters to define their properties
/// Need to attach code that causes the characters to either consume mana or lose HP
/// to the traps and the on Contact conditions.
/// </summary>

public class ManaSystem : MonoBehaviour {
	//Variables
	public float energy;
	public float baseEnergy = 100;
	private float regenRate = 20;
	// Use this for initialization
	void Start () {
		if (networkView.isMine == true) {
			energy = baseEnergy;
		}
		else{
			enabled = false;
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (energy < baseEnergy) {
			energy = energy + regenRate * Time.deltaTime;
		}
		if (energy > baseEnergy) {
			energy = baseEnergy;
		}

		if (energy < 0) {
			energy = 0;		
		}

		}

}
