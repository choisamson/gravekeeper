using UnityEngine;
using System.Collections;
/// <summary>
/// Multiplayer script is attached to the MultiplayerManager object.
/// </summary>
public class Server : MonoBehaviour
{
	// Variables_______________________
	private string titleMessage = "GIJAM2 TEST";
	private string connectToIP = "127.0.0.1";
	private int connectionPort = 26500;
	private bool useNat = false;
	private string ipAddress;
	private string port;
	private int numOfPlayers = 10;
	public string playerName;
	public string serverName;
	public string severNameForClient;
	private bool wantToSetupServer = false;
	private bool wantToConnectToServer = false;
	
	// These variables used to define the main window
	private Rect connectionWindowRect;
	private int connectionWindowWidth = 400;
	private int connectionWindowHeight = 280;
	private int buttonHeight = 60;
	private int leftIndent;
	private int topIndent;
	
	//These variables are for Server shutdown window
	private Rect serverDisWindow;
	private int serverDisWindowWidth = 300;
	private int serverDisWindowHeight = 150;
	private int serverDisWindowLeftIndent = 10;
	private int serverDisWindowTopIndent = 10;
	
	private bool showServerDisWindow = false;
	
	//These variables are used to define the client disconnect window
	private Rect clientDisWindow;
	private int clientDisWindowWidth = 300;
	private int clientDisWindowHeight = 170;
	private bool showDisconnectWindow = false;
	
	public Texture2D moonTexture;
	public Texture2D sunTexture;
	private double startTime = 0d;
	private Rect timerWindow;
	private bool started = false;
	
	//Variables End______________________
	
	// Use this for initialization
	void Start ()
	{
		// Load the last used serverName from the register
		// Use name Server as default
		serverName = PlayerPrefs.GetString ("serverName");
		
		if (serverName == "") {
			serverName = "Server";
		}
		// Load the last used playerName from the register
		// Use name Generic Name as default
		playerName = PlayerPrefs.GetString ("playerName");
		
		if (playerName == "") {
			playerName = "Generic Name";
		}
		
		//		started = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			showDisconnectWindow = !showDisconnectWindow;
			showServerDisWindow = !showServerDisWindow;
		}
		
		if (GameObject.Find ("Human(Clone)") != null && GameObject.Find ("Monster(Clone)") != null && !started) {
			startTime = Network.time;
			started = true;
			Debug.Log ("Game started");
		}
	}
	
	void ConnectWindow (int windowID)
	{
		// leave a gap from the header.
		GUILayout.Space (15);
		
		//When the player launchhse game they have option to create or join sever
		//The variables wantToSetupSever and wantToConnectToServer start as false
		//player given two buttons to choose from
		
		if (wantToSetupServer == false && wantToConnectToServer == false) {
			if (GUILayout.Button ("Setup a server", GUILayout.Height (buttonHeight))) {
				wantToSetupServer = true;
			}
			GUILayout.Space (10);
			
			if (GUILayout.Button ("Connect to a server", GUILayout.Height (buttonHeight))) {
				wantToConnectToServer = true;
			}
			GUILayout.Space (10);
			
			if (Application.isWebPlayer == false && Application.isEditor == false) {
				if (GUILayout.Button ("Exit Prototype", GUILayout.Height (buttonHeight))) {
					Application.Quit ();
				}
			}
		}
		if (wantToSetupServer == true) {
			// the user can type name for their server into textfield
			GUILayout.Label ("Enter a name for your server");
			serverName = GUILayout.TextField(serverName);
			
			GUILayout.Space (5);
			
			//the user can type in the port number for their server.
			GUILayout.Label ("Server Port");
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			
			GUILayout.Space(10);
			
			if(GUILayout.Button("Start my own server", GUILayout.Height (30))){
				//Create the server
				
				Network.InitializeServer (numOfPlayers, connectionPort, useNat);
				
				//Save the serverName using PLayerPrefs.
				PlayerPrefs.SetString("serverName", serverName);
				
				wantToSetupServer = false;
			}
			
			if (GUILayout.Button ("Go Back", GUILayout.Height (30))){
				wantToSetupServer = false;
			}
		}
		if (wantToConnectToServer == true) {
			
			// User can type their player name
			GUILayout.Label ("Enter your player name");
			
			playerName = GUILayout.TextField (playerName);
			
			GUILayout.Space (5);
			
			// Player can type the IP address of server
			GUILayout.Label ("Type in Server IP");
			
			connectToIP = GUILayout.TextField(connectToIP);
			
			GUILayout.Space(5);
			
			//Player can type in port number for server they want to connect to into the text field
			GUILayout.Label ("Server Port");
			
			connectionPort = int.Parse(GUILayout.TextField(connectionPort.ToString()));
			
			GUILayout.Space(5);
			
			if (GUILayout.Button("Connect", GUILayout.Height (25))){
				if (playerName == ""){
					playerName = "Generic Name";
				}
				
				if (playerName != ""){
					Network.Connect(connectToIP, connectionPort);
					PlayerPrefs.SetString("playerName", playerName);
				}
			}
			
			if (GUILayout.Button ("Go Back", GUILayout.Height (25))){
				wantToConnectToServer = false;
			}
		}
	}
	
	void ServerDisconnectWindow(int windowID){
		GUILayout.Label ("Server Name: " + serverName);
		
		//Show number of players connected
		GUILayout.Label ("Number of Players Connected: " + Network.connections.Length);
		
		//If there is at least one connection then show average ping
		if (Network.connections.Length >= 1) {
			GUILayout.Label ("Ping: " + Network.GetAveragePing (Network.connections [0]));
		}
		
		//Shutdown server if user clicks button.
		if (GUILayout.Button ("ShutdownServer")) {
			Network.Disconnect ();
		}
		if (GUILayout.Button ("Return to Game", GUILayout.Height (25))) {
			showServerDisWindow = false;
		}
	}
	void ClientDisconnectWindow (int windowID){
		//Show the player the server they are connected to and the ping of their connection
		GUILayout.Label ("Connected to Server: " + serverName);
		GUILayout.Label ("ping: " + Network.GetAveragePing (Network.connections [0]));
		
		GUILayout.Space (7);
		
		//Disconnect player if player chooses to disconnect
		if (GUILayout.Button ("Disconnect", GUILayout.Height (25))) {
			Network.Disconnect();
		}
		
		GUILayout.Space (5);
		
		//Button allows webplayer who as gone fullscreen to be able to return to the game using esc key
		if (GUILayout.Button ("Return to Game", GUILayout.Height (25))) {
			showDisconnectWindow = false;
		}
	}
	void OnDisconnectedFromServer()
	{
		//If a player loses the connection or leaves the scene then
		//the level is restarted on their computer.
		
		Application.LoadLevel(Application.loadedLevel);
	}
	
	
	void OnPlayerDisconnected(NetworkPlayer networkPlayer)
	{
		//When the player leaves the server delete them across the network
		//along with their RPCs so that other players no longer see them.
		
		Network.RemoveRPCs(networkPlayer);
		
		Network.DestroyPlayerObjects(networkPlayer);	
	}
	
	void OnGUI(){
		//If the player is disconnected then run the COnnectWindow function
		if (Network.peerType == NetworkPeerType.Disconnected) {
			//Determine the position of the window based on the width and height of the screen
			//window will be placed in the middle of the screen
			
			leftIndent = (Screen.width / 2) - connectionWindowWidth / 2 ;
			
			topIndent = Screen.height / 2 - connectionWindowHeight / 2;
			
			connectionWindowRect = new Rect (leftIndent, topIndent, connectionWindowWidth, connectionWindowHeight);
			
			connectionWindowRect = GUILayout.Window (0, connectionWindowRect,ConnectWindow, titleMessage);
		}
		
		//If the game is running as a server then run the serverDisconnecWindow Function
		if (Network.peerType == NetworkPeerType.Server && showServerDisWindow ==true){
			//Defining the Rect for the server's disconnect window
			serverDisWindow =  new Rect (serverDisWindowLeftIndent, serverDisWindowTopIndent, 
			                             serverDisWindowWidth, serverDisWindowHeight);
			serverDisWindow = GUILayout.Window(1, serverDisWindow, ServerDisconnectWindow, "");
		}
		
		if(Network.peerType == NetworkPeerType.Client && showDisconnectWindow == true)
		{
			clientDisWindow = new Rect(Screen.width / 2 - clientDisWindowWidth / 2,
			                           Screen.height / 2 - clientDisWindowHeight / 2,
			                           clientDisWindowWidth, clientDisWindowHeight);
			
			clientDisWindow = GUILayout.Window(1, clientDisWindow, ClientDisconnectWindow, "");
		}
		
		if (started) {
			Rect timerWindow = new Rect (Screen.width - 40f, 10f, 50f, 20f);
			int time = 90 - (int)(Network.time - startTime);
			Client client = GameObject.Find ("client").GetComponent<Client>();
			
			if (time > 0 && ! client.GameOver) {
				GUI.DrawTexture(new Rect(Screen.width - 70f, 10f, 20f, 20f), moonTexture, ScaleMode.ScaleToFit, true, 0f);
				GUI.Label (timerWindow, time + " s");
			} else {
				GUI.DrawTexture(new Rect(Screen.width - 70f, 10f, 20f, 20f), sunTexture, ScaleMode.ScaleToFit, true, 0f);
				GUI.Label (timerWindow, "Time");
				
				client.GameOver = true;
			}
		}
	}
}
