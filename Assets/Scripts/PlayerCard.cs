using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


public class PlayerCard : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    private List<Color> colorList = new List<Color>() { Color.red, Color.blue, Color.yellow, Color.green };

    public void SetPlayerInfo(Player _player, int playerNum)
    {
        if (_player.NickName == "") 
        {
            playerName.text = "Player " + playerNum;
        }
        else 
        { 
            playerName.text = _player.NickName; 
        }

        playerName.color = colorList[playerNum - 1];
        
    }
}
