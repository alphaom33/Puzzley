using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject body;

    public bool canDie;

    [Serializable]
    public class Level
    {
        public string sceneName;
    }

    public List<Level> levels;

    private GameObject start;
    public GameObject currentPlayer;

    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance != null) return instance;

        instance = Instantiate((GameObject)Resources.Load("GameManager")).GetComponent<GameManager>();
        instance.InitLevel();
        return instance;
    }

    private void InitLevel()
    {
        start = GameObject.FindWithTag("Start");
        SpawnPlayer();
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(levels[index].sceneName);
        StartCoroutine(oh());
        IEnumerator oh()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            InitLevel();
        }
    }

    private void SpawnPlayer()
    {
        currentPlayer = Instantiate(player, start.transform.position, Quaternion.identity);
    }

    public void KillPlayer()
    {
        if (!canDie) return;

        Instantiate(body, currentPlayer.transform.position, Quaternion.identity);
        Destroy(currentPlayer);
        SpawnPlayer();
    }
}
