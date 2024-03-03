using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using TMPro;


public class MenuController : MonoBehaviourPunCallbacks
{

    public TMP_InputField nameInput;
    public TMP_InputField codeInput;

    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";


    private string GenerateRandomCode(int length)
    {
        StringBuilder stringBuilder = new StringBuilder(length);
        System.Random random = new System.Random();

        for (int i = 0; i < length; i++)
        {
            int index = random.Next(Alphabet.Length);
            stringBuilder.Append(Alphabet[index]);
        }

        return stringBuilder.ToString();
        
    }

    public void DisplayName()
    {
        nameInput.text = PhotonNetwork.NickName;
    }

    public void ChangeName()
    {
        PhotonNetwork.NickName = nameInput.text;
        Debug.Log("Player name changed.");
    }

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
            
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Debug.Log("Connected");
    }

    public void CreateGame() 
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            string randomCode = GenerateRandomCode(6);
            PhotonNetwork.CreateRoom(randomCode, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true });
        }

    }

    public void JoinGame()
    {
       if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinRoom(codeInput.text);
        }
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public override void OnJoinRoomFailed(short returnCode,string message)
    {
        Debug.Log("Room does not exist.");
    }

    public void QuitGame()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Game quit successfully.");
        Application.Quit();
    }

}
