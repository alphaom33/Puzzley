using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.CompareTag("Player")) GameManager.GetInstance().LoadNextLevel();
    }
}
