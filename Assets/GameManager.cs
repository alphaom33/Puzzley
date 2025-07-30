using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject body;

    private GameObject start;
    private GameObject currentPlayer;

    private void Start()
    {
        start = GameObject.FindWithTag("Start");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        currentPlayer = Instantiate(player, start.transform.position, Quaternion.identity);
    }

    public void KillPlayer()
    {
        Instantiate(body, currentPlayer.transform.position, Quaternion.identity);
        Destroy(currentPlayer);
        SpawnPlayer();
    }
}
