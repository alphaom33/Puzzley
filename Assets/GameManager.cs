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

    [Serializable]
    public class Level
    {
        public string sceneName;
    }

    public List<Level> levels;

    private GameObject start;
    private GameObject currentPlayer;
    private GameObject MainCamera;

    private void Start()
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
        MainCamera.GetComponent<FollowPlayer>().Player = currentPlayer.transform;
        
    }

    public void KillPlayer()
    {
        Instantiate(body, currentPlayer.transform.position, Quaternion.identity);
        Destroy(currentPlayer);
        SpawnPlayer();
    }
}
