using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class SelectController : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomName;
    public List<PlayerCard> playerList = new List<PlayerCard>();
    public PlayerCard cardPrefab;
    public Transform cardParent;


    private void UpdatePlayerList()
    {
        int playerNum = 0;

        foreach (PlayerCard card in playerList)
        {
            Destroy(card.gameObject);
        }
        playerList.Clear();

        if (PhotonNetwork.CurrentRoom != null)
        {
            foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
            {
                playerNum++;
                PlayerCard newCard = Instantiate(cardPrefab, cardParent);
                newCard.SetPlayerInfo(player.Value, playerNum);
                playerList.Add(newCard); 
            }
        }
    }

    private void Awake()
    {
        roomName.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }

}
