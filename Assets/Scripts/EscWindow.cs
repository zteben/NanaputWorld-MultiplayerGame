using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class EscWindow : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject quitMenu;
    public TextMeshProUGUI roomName;

    private void Awake()
    {
        roomName.text = "Room Code: " + PhotonNetwork.CurrentRoom.Name;
    }

    private void Start()
    {
        CloseWindow();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            ToggleWindow();
        }
    }

    private void ToggleWindow()
    {
        if (optionsMenu.activeSelf || quitMenu.activeSelf)
        {
            CloseWindow();
        }
        else
        {
            OpenWindow();
        }
    }

    public void CloseWindow()
    {
        optionsMenu.SetActive(false);
        quitMenu.SetActive(false);

        Cursor.visible = false;
    }

    public void OpenWindow()
    {
        optionsMenu.SetActive(true);

        Cursor.visible = true;
    }

    public void QuitGame()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Game quit successfully.");
        Application.Quit();
    }
}
