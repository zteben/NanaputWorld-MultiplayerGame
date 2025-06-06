using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    void Awake()
    {
        PhotonNetwork.SendRate = 45;
        PhotonNetwork.SerializationRate = 30;
    }

}
