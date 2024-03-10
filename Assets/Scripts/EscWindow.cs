using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EscWindow : MonoBehaviour
{
    public GameObject window;

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
        window.SetActive(!window.activeSelf);

        Cursor.lockState = window.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = window.activeSelf;
    }

    public void CloseWindow()
    {
        window.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
        PhotonNetwork.Disconnect();
        Debug.Log("Game quit successfully.");
        Application.Quit();
    }
}
