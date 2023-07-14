using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;


public class SelectController : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI roomName;
    public PlayerCard cardPrefab;
    public Transform cardParent;
    public Button lockinButton;
    public GameObject grid;
    public Button startButton;
    List<PlayerCard> cardList = new List<PlayerCard>();

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    private void UpdatePlayerList()
    {
        foreach (PlayerCard card in cardList)
        {
            Destroy(card.gameObject);
        }
        cardList.Clear();

        if (PhotonNetwork.CurrentRoom != null)
        {
            Player[] players = PhotonNetwork.PlayerList;
            Array.Sort(players, (p1, p2) => p1.ActorNumber.CompareTo(p2.ActorNumber));
            int playerNum = 0;

            foreach (Player player in players)
            {
                playerNum++;
                PlayerCard newCard = Instantiate(cardPrefab, cardParent);
                newCard.SetPlayerInfo(player, playerNum);
                cardList.Add(newCard); 
            }
        }
    }

    public void CheckPlayersReady()
    {
        bool allPlayersLockedIn = true;

        Player[] players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            if (!player.CustomProperties.ContainsKey("lockedin") || !(bool)player.CustomProperties["lockedin"])
            {
                allPlayersLockedIn = false;
                break;
            }
        }
        if (allPlayersLockedIn)
        {
            startButton.interactable = true;
        }
        else
        {
            startButton.interactable = false;
        }
    }

    private void Awake()
    {
        UpdatePlayerList();
        playerProperties["lockedin"] = false;
        playerProperties["character"] = 0;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        roomName.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;

        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void LeaveRoom()
    {
        playerProperties["lockedin"] = false;
        playerProperties["character"] = 0;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Menu");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        CheckPlayersReady();
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        UpdatePlayerList();
        CheckPlayersReady();
        if (PhotonNetwork.IsMasterClient)
        {
            startButton.gameObject.SetActive(true);
        }
    }
    
    public void SelectCharacter(int characterID)
    {
        playerProperties["character"] = characterID;
        lockinButton.interactable = true;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void LockIn()
    {
        Button[] buttons = grid.GetComponentsInChildren<Button>();

        if ((int)playerProperties["character"] != 0)
        {
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
            lockinButton.gameObject.SetActive(false);
            playerProperties["lockedin"] = true;
            PhotonNetwork.SetPlayerCustomProperties(playerProperties);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProperties)
    {
        if (changedProperties.ContainsKey("character"))
        {
            foreach (PlayerCard card in cardList)
            {
                card.UpdateSprite();
            }
        }

        if (changedProperties.ContainsKey("lockedin"))
        {
            CheckPlayersReady();
        }

    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel("Trenholme");
    }
}
