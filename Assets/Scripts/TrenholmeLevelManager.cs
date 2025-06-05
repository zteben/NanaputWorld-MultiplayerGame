using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class TrenholmeLevelManager : MonoBehaviourPunCallbacks
{

    public Transform bossSpawnPoint;
    public string bossPrefabName;
    private int enemiesRemaining;
    private GameObject bossInstance;


    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(LevelFlow());
    }

    IEnumerator LevelFlow()
    {
        yield return new WaitForSeconds(2f);

        yield return StartCoroutine(SpawnBoss());
    }

    IEnumerator SpawnBoss()
    {
        enemiesRemaining = 1;
        bossInstance = PhotonNetwork.Instantiate(bossPrefabName, bossSpawnPoint.position, Quaternion.identity);
        yield break;
    }




    /*
    public string squirrelPrefabName;
    public Transform[] squirrelSpawnPoints;
    public string bossPrefabName;
    public Transform bossSpawnPoint;

    private int enemiesRemaining;
    private GameObject bossInstance;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            StartCoroutine(LevelFlow());
    }

    IEnumerator LevelFlow()
    {
        // Sync level name display
        photonView.RPC(nameof(RPC_DisplayLevelName), RpcTarget.All);
        yield return new WaitForSeconds(5f);

        yield return StartCoroutine(SpawnWave(1));
        yield return StartCoroutine(SpawnWave(2));
        yield return StartCoroutine(SpawnBoss());
    }

    [PunRPC]
    void RPC_DisplayLevelName()
    {
        // Replace this with your UI call
        Debug.Log("TRENHOLME");
        // Example: LevelUI.Instance.ShowLevelName("TRENHOLME");
    }

    IEnumerator SpawnWave(int waveNumber)
    {
        int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
        int squirrelCount = waveNumber switch
        {
            1 => Mathf.Clamp(playerCount * 1, 3, 7),
            2 => Mathf.Clamp(playerCount * 2, 3, 7),
            _ => 5
        };

        enemiesRemaining = squirrelCount;

        for (int i = 0; i < squirrelCount; i++)
        {
            Transform spawnPoint = squirrelSpawnPoints[i % squirrelSpawnPoints.Length];
            GameObject squirrel = PhotonNetwork.Instantiate(squirrelPrefabName, spawnPoint.position, Quaternion.identity);
            Enemy enemy = squirrel.GetComponent<Enemy>();
            enemy.OnEnemyDeath += OnEnemyDefeated;
        }

        // Wait until wave is cleared
        while (enemiesRemaining > 0)
            yield return null;
    }

    IEnumerator SpawnBoss()
    {
        enemiesRemaining = 1;
        bossInstance = PhotonNetwork.Instantiate(bossPrefabName, bossSpawnPoint.position, Quaternion.identity);
        Enemy boss = bossInstance.GetComponent<Enemy>();
        boss.OnEnemyDeath += OnBossDefeated;

        while (enemiesRemaining > 0)
            yield return null;

        yield return new WaitForSeconds(5f);
        photonView.RPC(nameof(RPC_LoadEndScreen), RpcTarget.All);
    }

    void OnEnemyDefeated()
    {
        enemiesRemaining--;
    }

    void OnBossDefeated()
    {
        enemiesRemaining--;
    }

    [PunRPC]
    void RPC_LoadEndScreen()
    {
        if (PhotonNetwork.IsMasterClient)
            SceneManager.LoadScene("EndScreen");
    }
    */
}
