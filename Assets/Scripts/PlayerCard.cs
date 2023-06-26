using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class PlayerCard : MonoBehaviourPun
{
    
    public TextMeshProUGUI playerName;
    public Image playerCharacter;
    public Sprite[] characters;

    Player player;
    

    public void SetPlayerInfo(Player _player, int playerNum)
    {
        player = _player;

        if (player.NickName == "") 
        {
            playerName.text = "Player " + playerNum;
        }
        else 
        { 
            playerName.text = player.NickName; 
        }

        if (PhotonNetwork.LocalPlayer == player)
        {  
            playerName.color = Color.yellow;
        }

        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (player.CustomProperties.TryGetValue("character", out object value))
        {
            playerCharacter.sprite = characters[(int)value];
        }
        else
        {
            playerCharacter.sprite = characters[0];
        }
    }
}
