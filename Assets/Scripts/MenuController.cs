using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Text;
using TMPro;


public class MenuController : MonoBehaviourPunCallbacks
{

    public TMP_InputField nameInput;
    public TMP_InputField codeInput;

    public AudioMixer audioMixer;
    public Slider musicSlider;

    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private bool playerIsConnected = false;

    /*
    #############################################
    -----------------PLAYER NAME-----------------
    #############################################
    */
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
        if (PhotonNetwork.NickName == nameInput.text) return;

        if (nameInput.text.Length <= 10)
        {
            PhotonNetwork.NickName = nameInput.text;
            Debug.Log("Player name changed.");
        }
        else
        {
            Debug.Log("Player name must be less than or equal to 10 characters");
        }
    }

    public void SaveName()
    {
        PlayerPrefs.SetString("PlayerName", PhotonNetwork.NickName);
    }

    public void LoadName()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("PlayerName", "");
    }

    /*
    #############################################
    ---------------------AUDIO-------------------
    #############################################
    */
    public void SetMusicVolume(float sliderVolume)
    {
        float volume = Mathf.Log10(Mathf.Clamp(sliderVolume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("MusicVolume", volume);
        audioMixer.SetFloat("MusicVolume", volume);
    }

    /*
    public void SetSFXVolume(float sliderVolume)
    {
        float volume = Mathf.Log10(Mathf.Clamp(sliderVolume, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat("SFXVolume", volume);
    }
    */

    public void SaveVolume()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
    }

    public void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    /*
    #############################################
    ------------------MAIN MENU------------------
    #############################################
    */
    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings();
        }
            
    }

    private void Start()
    {
        LoadName();
        LoadVolume();
        MusicManager.Instance.PlayMusic("MainMenu");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        playerIsConnected = true;
        Debug.Log("Connected");
    }

    public void CreateGame() 
    {
        if (PhotonNetwork.IsConnectedAndReady && playerIsConnected)
        {
            string randomCode = GenerateRandomCode(6);
            PhotonNetwork.CreateRoom(randomCode, new RoomOptions() { MaxPlayers = 4, BroadcastPropsChangeToAll = true });
        }

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room: " + message);
    }

    public void JoinGame()
    {
       if (PhotonNetwork.IsConnectedAndReady && playerIsConnected)
        {
            PhotonNetwork.JoinRoom(codeInput.text.ToUpper());
        }
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("CharacterSelect");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room: " + message);
    }

    public void QuitGame()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Game quit successfully.");
        Application.Quit();
    }

}
