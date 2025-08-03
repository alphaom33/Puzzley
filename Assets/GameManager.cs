using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Cinemachine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject body;

    public ScreenCover wipe;

    public GameObject pauseMenuPrefab;
    public GameObject pauseMenu;

    public bool canDie;
    public bool canMove = true;

    public List<Level> levels;
    public int maxLevel;
    public int currentLevel;

    public GameObject start;
    public GameObject currentPlayer;

    public AudioSource spawnSound;
    public AudioSource deathSound;

    private static GameManager instance;
    public static GameManager GetInstance()
    {
        if (instance != null)
        {
            return instance;
        }

        GameManager g = Instantiate((GameObject)Resources.Load("GameManager")).GetComponent<GameManager>();
        g.StartCoroutine(g.InitLevel());
        return g;
    }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Load();
        DontDestroyOnLoad(gameObject);
        pauseMenuPrefab = (GameObject)Resources.Load("PauseMenu");
    }

    public void Update()
    {
        if (SceneManager.GetActiveScene().name != "StartMenu")
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                KillPlayer();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
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
    }

    private IEnumerator InitLevel()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        canMove = true;
        for (; start == null; start = GameObject.FindWithTag("Start"))
        {
            yield return new WaitForEndOfFrame();
        }
        SpawnPlayer();
    }

    public void LoadLevel(int index)
    {
        currentLevel = index;
        maxLevel = currentLevel > maxLevel ? currentLevel : maxLevel;
        StartCoroutine(Wipe());
        IEnumerator Wipe() {
            Tween enter = wipe.Enter();
            yield return enter.WaitForCompletion(); // yes this is inefficient, but I don't believe that Unity has a way to prevent scene load completion, so it's also unfortunately necessary

            SceneManager.LoadScene(levels[index].sceneName);
            StartCoroutine(InitLevel());

            wipe.Exit();
        }
    }

    public void LoadNextLevel() => LoadLevel(currentLevel + 1);

    private void SpawnPlayer()
    {
        spawnSound.Play();

        currentPlayer = Instantiate(player, start.transform.position, Quaternion.identity);
        GameObject.FindWithTag("MainCamera").GetComponent<CinemachineVirtualCamera>().Follow = currentPlayer.transform;
        StartCoroutine(EnableDeath());
    }

    public void KillPlayer()
    {
        if (!canDie) return;
        deathSound.Play();

        canDie = false;
        canMove = false;

        GameObject newBody = Instantiate(body, currentPlayer.transform.position, Quaternion.identity);
        newBody.transform.localScale = new Vector2(Mathf.Sign(currentPlayer.transform.localScale.x), 1);
        Destroy(currentPlayer);

        Time.timeScale = 0;

        StartCoroutine(Respawn());
        IEnumerator Respawn() {
            Tween enter = wipe.Enter();
            yield return enter.WaitForCompletion();

            Time.timeScale = 1;
            SpawnPlayer();

            yield return new WaitForEndOfFrame();
            wipe.Exit();
        }
    }

    private IEnumerator EnableDeath()
    {
        yield return new WaitForSeconds(.1f);
        canDie = true;
        spawnSound.Play();
        yield return new WaitWhile(() => Input.GetAxisRaw("Horizontal") != 0);
        canMove = true;
    }

    private void Load() 
    {
        try 
        {
            maxLevel = int.Parse(File.ReadAllText(Application.persistentDataPath + "/save"));
        }
        catch (IOException)
        {

        }
    }

    public void Quit() 
    {
        Application.Quit();
    }

    public void OnApplicationQuit() 
    {
        File.WriteAllText(Application.persistentDataPath + "/save", "" + maxLevel);
    }
}