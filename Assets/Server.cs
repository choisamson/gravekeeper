using UnityEngine;
using System.Collections;

public class Server : MonoBehaviour {

	public string connectionIP = "10.20.201.188";
	private const int CONNECTION_PORT = 25001;
	private const string GAME_NAME = "Steed_TestServer";
	private bool isClient = false;
	private bool listFetched = false;
	private HostData[] serverList;

	void OnGUI() {
		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Disconnected");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Search For Hosts"))
			{
				MasterServer.RequestHostList(GAME_NAME);
				isClient = true;
			}
			if (GUI.Button(new Rect(10, 50, 120, 20), "Initialize Server"))
			{
				Network.incomingPassword = "steed1234";
				Network.InitializeServer(32, CONNECTION_PORT, !Network.HavePublicAddress());
				MasterServer.RegisterHost(GAME_NAME, "TestServer", "Testing network capabilities");
				isClient = false;
			}
			if (isClient && listFetched){
				for(int i = 0; i < serverList.Length; i ++){
					string tmpIp = "";
					HostData serverData = serverList[i];
					for(int j = 0; j < serverData.ip.Length; j ++) {
						tmpIp = serverData.ip[i] + " ";
					}
					if (GUI.Button(new Rect(10, 70 + i*20, 120, 20), "Host: " + serverData.gameName)) {
						Network.Connect(serverData.ip, CONNECTION_PORT, "steed1234");
						isClient = true;
					}
				}
			}
		}
		else if (Network.peerType == NetworkPeerType.Client)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Client");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Disconnect"))
			{
				Network.Disconnect(200);
			}
		}
		else if (Network.peerType == NetworkPeerType.Server)
		{
			GUI.Label(new Rect(10, 10, 300, 20), "Status: Connected as Server");
			if (GUI.Button(new Rect(10, 30, 120, 20), "Disconnect"))
			{
				Network.Disconnect(200);
			}
			MasterServer.RequestHostList(GAME_NAME);
		}
	}

	void OnMasterServerEvent(MasterServerEvent mSE){
		if (mSE == MasterServerEvent.RegistrationSucceeded) {
			Debug.Log ("Registered Server");
		} else if (mSE == MasterServerEvent.HostListReceived) {
			Debug.Log(MasterServer.PollHostList().Length);
			serverList = MasterServer.PollHostList();
			if (serverList.Length > 0){
				listFetched = true;
				string tmpIp = "";
				HostData serverData = MasterServer.PollHostList()[0];
				for(int i = 0; i < serverData.ip.Length; i ++) {
					tmpIp = serverData.ip[i] + " ";
				}
				Debug.Log (tmpIp);
				Debug.Log(serverData.gameName);
			} else {
				MasterServer.RequestHostList(GAME_NAME);
				listFetched = false;
			}
		} else {
			Debug.Log (mSE.ToString());
		}
	}
}
