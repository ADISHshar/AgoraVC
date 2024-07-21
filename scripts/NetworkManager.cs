using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public string gameVersion = "1";
    public GameObject playerPrefab; // Player
    public Transform[] spawnPoints; //Random Spawn points for players
    public Text roomNameText; // for room name 

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        ConnectToPhoton();
    }

    private void ConnectToPhoton()
    {
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        string roomName = GenerateRoomName();
        PhotonNetwork.JoinOrCreateRoom(roomName, 
        new RoomOptions { MaxPlayers = 4 },TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        Debug.Log("Current Room Name: " + PhotonNetwork.CurrentRoom.Name);

        if (roomNameText != null)
        {
            roomNameText.text = "Room: " + PhotonNetwork.CurrentRoom.Name;
            Debug.Log("Room name text updated.");
        }
        else
        {
            Debug.LogWarning("Room Name Text is not assigned in the inspector.");
        }
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            Debug.Log("Player prefab is not null");
            // Choose a random spawn point
            Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)];

            // Instantiate player at chosen spawn point
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation);
            Debug.Log("Instantiated player at spawn point: " + spawnPoint.position);
        }
        else
        {
            Debug.LogError("Player prefab is not assigned!");
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        // Optionally, handle leaving the room here (e.g., load a different scene)
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon: " + cause);
        // Optionally, handle reconnection or load a different scene here
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void LoadScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }
    private static readonly List<string> Adjectives = new List<string>
    {
        "Wacky", "Silly", "Quirky", "Mighty", "Funky", "Zany", "Epic", "Nifty", "Jolly", "Bizarre"
    };

    private static readonly List<string> Nouns = new List<string>
    {
        "Penguin", "Ninja", "Robot", "Dragon", "Waffle", "Unicorn", "Pizza", "Alien", "Pirate", "Hamster"
    };
        public static string GenerateRoomName()
    {
        string adjective = Adjectives[Random.Range(0, Adjectives.Count)];
        string noun = Nouns[Random.Range(0, Nouns.Count)];

        return $"{adjective} {noun}";
    }
}
