using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject start;

    private void Start()
    {
        start = GameObject.FindWithTag("Start");

    }

    public void KillPlayer()
    {

    }
}
