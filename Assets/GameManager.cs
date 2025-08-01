using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject body;

    public GameObject pauseMenuPrefab;
    public GameObject pauseMenu;

    public bool canDie;
    public bool canMove = true;

    public List<Level> levels;

    public GameObject start;
    public GameObject currentPlayer;

    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance != null)
        {
            return instance;
        }

        GameManager tmp = Instantiate((GameObject)Resources.Load("GameManager")).GetComponent<GameManager>();
        tmp.StartCoroutine(tmp.InitLevel());
        DontDestroyOnLoad(tmp);
        return tmp;
    }

    public void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        pauseMenuPrefab = (GameObject)Resources.Load("PauseMenu");
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "StartMenu")
        {
            if (pauseMenu == null)
            {
                pauseMenu = Instantiate(pauseMenuPrefab);
                canMove = false;
            }
            else
            {
                pauseMenu.GetComponent<PauseMenu>().UnPause();
            }
        }
    }

    private IEnumerator InitLevel()
    {
        canMove = true;
        for (; start == null; start = GameObject.FindWithTag("Start"))
        {
            yield return new WaitForEndOfFrame();
        }
        SpawnPlayer();
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(levels[index].sceneName);
        StartCoroutine(InitLevel());
    }

    private void SpawnPlayer()
    {
        currentPlayer = Instantiate(player, start.transform.position, Quaternion.identity);
        GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>().Follow = currentPlayer.transform;
    }

    public void KillPlayer()
    {
        if (!canDie) return;
        canDie = false;
        canMove = false;
        Instantiate(body, currentPlayer.transform.position, Quaternion.identity);
        Destroy(currentPlayer);
        SpawnPlayer();
    }
}
