using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    
    public GameObject player;
    public GameObject body;

    private GameObject start;
    private GameObject currentPlayer;
    private GameObject MainCamera;

    public bool CanDie;

    private void Start()
    {
        MainCamera = GameObject.FindWithTag("MainCamera");
        CanDie = true;
        start = GameObject.FindWithTag("Start");
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        currentPlayer = Instantiate(player, start.transform.position, Quaternion.identity);
        MainCamera.GetComponent<FollowPlayer>().Player = currentPlayer.transform;
        
    }

    public void KillPlayer()
    {
        if (CanDie)
        {
            CanDie = false;
            Instantiate(body, currentPlayer.transform.position, Quaternion.identity);
            Destroy(currentPlayer);
            SpawnPlayer();
        }
       
    }
}
