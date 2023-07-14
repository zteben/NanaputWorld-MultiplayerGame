using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class PlayerLoader : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform[] spawnPoints;

    
    private void Start()
    {

        Player[] players = PhotonNetwork.PlayerList;
        Array.Sort(players, (p1, p2) => p1.ActorNumber.CompareTo(p2.ActorNumber));
        int playerNum = Array.IndexOf(players, PhotonNetwork.LocalPlayer);
        GameObject characterToSpawn = characterPrefabs[(int)PhotonNetwork.LocalPlayer.CustomProperties["character"] - 1];
        PhotonNetwork.Instantiate(characterToSpawn.name, spawnPoints[playerNum].position, Quaternion.identity);
    }

}
